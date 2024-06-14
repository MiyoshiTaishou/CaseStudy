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

    [Header("テキストの揺れのスピード"), SerializeField]
    private float UpDownSpeed = 1.0f;

    [Header("テキストの揺れ幅"), SerializeField]
    private float fShakingRange = 1.0f;

    [Header("耐久値を表示するか?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    private TextMeshProUGUI TextEndurance;

    [Header("破壊時のエフェクト"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("破片のエフェクト"), SerializeField]
    private GameObject Eff_BrokenPiece;

    [Header("破片生成数(初期30)"), SerializeField]
    private int GenerateNum = 30;

    [Header("爆発音"), SerializeField]
    private AudioClip audioclip;

    private ParticleSystem.Burst burst;

    private bool isQuitting;

    /// <summary>
    /// 三好大翔追記カメラオブジェクト
    /// </summary>
    GameObject camera;

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

        if (Eff_BrokenPiece) {
            burst.count = GenerateNum;
            Eff_BrokenPiece.GetComponent<ParticleSystem>().emission.SetBurst(0,burst);
        }

        //三好大翔追記カメラ探す
        camera = GameObject.FindWithTag("MainCamera");

        if (!camera)
        {
            Debug.Log("カメラが見つかりません");
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
                TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange), screenPos.z); // 適切なオフセットを持たせる
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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;
        //オブジェクトからS_EnemyBall取得
        S_EnemyBall enemyBall = HitObj.GetComponent<S_EnemyBall>();

        //相手にS_EnemyBallスクリプトが付いていたら
        if (enemyBall)
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if (this.GetBreakNum() <= checkNum && enemyBall.GetisPushing())
            {
                //爆発エフェクト再生
                if (Eff_Explosion)
                {
                    Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
                // 破片エフェクト生成
                if (Eff_BrokenPiece)
                {
                    Instantiate(Eff_BrokenPiece, transform.position, Quaternion.identity);
                }
                if (camera)
                {
                    camera.GetComponent<M_CameraShake>().Shake();
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;
        //オブジェクトからS_EnemyBall取得
        S_EnemyBall enemyBall = HitObj.GetComponent<S_EnemyBall>();

        //相手にS_EnemyBallスクリプトが付いていたら
        if (enemyBall)
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if (this.GetBreakNum() <= checkNum && enemyBall.GetisPushing())
            {
                //爆発エフェクト再生
                if (Eff_Explosion)
                {
                    Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
                // 破片エフェクト生成
                if (Eff_BrokenPiece)
                {
                    Instantiate(Eff_BrokenPiece, transform.position, Quaternion.identity);
                }
                if (camera)
                {
                    camera.GetComponent<M_CameraShake>().Shake();
                }
                Destroy(gameObject);
            }
        }
    }
}
