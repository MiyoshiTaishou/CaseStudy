using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    ///ボタンが押されたかどうか
    private bool isButtonDown = false;

    //敵が視界内にいるかどうか
    private bool isSerch = false;

    [Header("KeyType"),SerializeField]
    KeyCode playerChangeKey = KeyCode.C;

    [Header("変身時間"), SerializeField]
    float fWaitTime = 10.0f;

    //変身中かどうか
    private bool isChanging = false;
    bool GetisChanging() { return isChanging; }

    //変身前の見た目(Player)
    private Sprite PlayerSprite;

    //変身後の見た目(Enemy)
    private Sprite EnemySprite;

    private SpriteRenderer srEnemy = null;

    private SpriteRenderer srPlayer = null;

    //変身の当たり判定内にいる敵のリスト
    private List<GameObject> colList= new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        srPlayer = transform.root.GetComponent<SpriteRenderer>();
        if(!srPlayer)
        {
            Debug.LogError("SpriteRendererがありません");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //変身ボタンが押されたかどうか判定
        isButtonDown = Input.GetKeyDown(playerChangeKey);
        Debug.Log("ボタン押下状況=" + isButtonDown);
        
        if (isButtonDown &&colList.Count != 0) 
        {
            Debug.Log("へんしんしたい");
            //変身の条件を満たしている敵の中で一番距離が近い敵を探す
            float distance = 0.0f;
            for(int i=0;i< colList.Count;i++)
            {
                float temp = Vector2.Distance(transform.root.position , colList[i].transform.position);
                if (distance > temp||distance == 0.0f) 
                {
                    distance = temp;
                    srEnemy = colList[i].GetComponent<SpriteRenderer>();
                }
            }
            EnemySprite = srEnemy.sprite;
            StartCoroutine(Change());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //接触したものが敵かつめくらまし状態であればリストに追加
        if(other.tag=="Enemy")
        {
            M_BlindingMove blindingMove = other.GetComponent<M_BlindingMove>();
            isSerch = blindingMove.GetIsBlinding();

            //リストに追加したことがないものだけリストに追加
            GameObject collidedObject = other.gameObject;
            if (!colList.Contains(collidedObject) && isSerch)
            {
                colList.Add(collidedObject);
                Debug.Log("Object追加"+colList.Count);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //リストから取り除く
        GameObject collidedObject = other.gameObject;
        if(colList.Contains(collidedObject))
        {
            colList.Remove(collidedObject);
        }
    }

    //コルーチン。
    IEnumerator Change()
    {
        //終わるまで待ってほしい処理を書く
        //変身中でなければプレイヤーと敵の見た目を入れ替える、変身中であれば変身時間をリセットするだけ
        if (!isChanging)
        {
            //現在の状況に応じてタグと見た目を変更
            transform.root.tag = "Enemy";
            PlayerSprite = srPlayer.sprite;
            //見た目の変更
            srPlayer.sprite = EnemySprite;
            srEnemy.sprite = PlayerSprite;
            isChanging= true;
        }
        //指定の秒数待つ
        yield return new WaitForSeconds(fWaitTime);
        //再開してから実行したい処理を書く

        //見た目を元に戻す
        srPlayer.sprite = PlayerSprite;
        srEnemy.sprite = EnemySprite;
        Debug.Log("変身解除");
        transform.root.tag = "Player";
        isChanging= false;
    }
}
