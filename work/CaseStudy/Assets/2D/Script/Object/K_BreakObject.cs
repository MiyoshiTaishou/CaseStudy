using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class K_BreakObject : MonoBehaviour
{
    [Header("壊すのに必要な敵の数"), SerializeField]
    private uint iBreakNum = 3;

    [Header("テキストサイズ"), SerializeField]
    private int iTexSize = 100;

    [Header("耐久値を表示するか?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    private TextMeshProUGUI TextEndurance;

    [Header("破壊時のエフェクト"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("爆発音"), SerializeField]
    private AudioClip audioclip;

    [Header("ヒットストップ"), SerializeField]
    float fHitStop = 0;

    private bool isQuitting;

    private void Start()
    {
        if(IsDisplayBreakNum == true)
        {
            //プレハブ実体化
            GameObject cd = transform.GetChild(0).gameObject; ;
            GameObject gcd = cd.GetComponent<Transform>().transform.GetChild(0).gameObject;
            TextEndurance = gcd.GetComponent<TextMeshProUGUI>();
            TextEndurance.text = iBreakNum.ToString();
            TextEndurance.fontSize = iTexSize;
        }
    }

    private void Update()
    {
        if (this.gameObject == null)
        {
            TextEndurance.text = null;
        }
        else
        {
           if (IsDisplayBreakNum == true)
           {
               // オブジェクト位置にテキスト要素を追従させる
               Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
               TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z); // 適切なオフセットを持たせる
           }
        }
    }

    public uint GetBreakNum()
    {
        return iBreakNum;
    }

    void OnApplicationQuit()
    {

        isQuitting = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("EnemyBall")&&collision.transform.GetComponent<S_EnemyBall>().GetisPushing())
        {
            StartCoroutine(HitStop());
        }
    }

    private void OnDestroy()
    {
        if(!isQuitting)
        {
            //StartCoroutine(HitStop());
        }
    }

    IEnumerator HitStop()
    {
        Debug.Log("止まっている");
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(fHitStop / 60);
        //爆発エフェクト再生
        if (Eff_Explosion)
        {
            Debug.Log("エフェクト再生");
            Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
        }
        if (audioclip)
        {
            AudioSource.PlayClipAtPoint(audioclip, transform.position);
        }
    }
}
