using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 発見されたときにBGM変更
/// 画面演出
/// </summary>
public class M_PlayerWarning3DK : MonoBehaviour
{
    [Header("反映するImage"), SerializeField]
    private Image img;

    [Header("見つかった時の画面の色"), SerializeField]
    private Color color;

    [Header("色の変わる速度"), SerializeField]
    private float fColorSpeed;
   
    /// <summary>
    /// 発見されているか
    /// </summary>
    private bool isFound = false;

    /// <summary>
    /// 指定した色になったか
    /// </summary>
    private bool isColor = false;

    /// <summary>
    /// 時間計測
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// 範囲内にいるエネミーのリスト
    /// </summary>
    private List<GameObject> EnemyList = new List<GameObject>();

    /// <summary>
    /// BGM切り替えオブジェクト
    /// </summary>
    private GameObject BGM;

    /// <summary>
    /// BGM切り替えたか
    /// </summary>
    private bool isSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        //初期は透明にする
        img.color = Color.clear;

        BGM = GameObject.Find("SwitchBGM");
    }

    // Update is called once per frame
    void Update()
    {
        List<int> array = new List<int>();
        int i = 0;
        //範囲内のエネミーに見つかっているかどうかチェックをする
        foreach (GameObject obj in EnemyList)
        {
            if(obj == null)
            {
                array.Add(i);

                // 次のループへスキップ
                continue;
            }
            //if(obj.GetComponent<MPlayerSearch>().GetIsSearch())
            if(obj.GetComponent<N_PlayerSearch>().GetIsSearch())
            {
                //見つかった場合はtrueにして抜ける
                isFound = true;

                //Debug.Log("見つかったよ");
                break;
            }
            else
            {
                isFound = false;
            }
        }

        // 削除されたオブジェクトをリストから除外
        foreach(var num in array)
        {
            EnemyList.RemoveAt(num);
        }
      
        if(isFound)
        {
            Found();

            //Debug.Log("見つかった");

            BGM.GetComponent<M_SwitchBGM>().ChangeBGM(true);           
        }
        else
        {           
            //見つかってない時は透明
            img.color = Color.clear;
                  
            BGM.GetComponent<M_SwitchBGM>().ChangeBGM(false);      
        }      
    }

    /// <summary>
    /// 見つかっている時の演出
    /// </summary>
    void Found()
    {
        fTime += fColorSpeed * Time.deltaTime;

        //色を変える処理
        if(isColor)
        {
            img.color = Color.Lerp(color, Color.clear, fTime);
        }
        else
        {           
            img.color = Color.Lerp(Color.clear, color, fTime);
        }
        
        if(fTime > 1.0f)
        {
            fTime = 0.0f;
            isColor = !isColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //まだリストにない物を入れる
            if (!EnemyList.Contains(collision.gameObject))
            {
                EnemyList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //まだリストにない物を入れる
            if (EnemyList.Contains(collision.gameObject))
            {
                EnemyList.Remove(collision.gameObject);
            }
        }
    }  
}
