using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class K_BreakObject : MonoBehaviour
{
    [Header("壊すのに必要な敵の数"), SerializeField]
    private uint iBreakNum = 3;

    [Header("テキストサイズ"), SerializeField]
    private int iTexSize = 100;

    [Header("テキストの揺れのスピード"), SerializeField]
    private float UpDownSpeed = 1.0f;

    [Header("テキストの揺れ幅"), SerializeField]
    private float fShakingRange = 0.1f;

    [Header("耐久値を表示するか?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    [Header("耐久値表示の座標"), SerializeField]
    private Vector2Int TextOffset;

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

    private GameObject DisplayNumObj;
    Vector3 InitTransForm;

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
        
        //数字表示オブジェクト探す
        for(int i=0;i< transform.childCount;i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if(obj.name== "K_Suuji")
            {
                DisplayNumObj = obj;
            }
        }

        if(!DisplayNumObj)
        {
            Debug.Log("このオブジェクト怪しいぞ");
        }
        else
        {
            DisplayNumObj.GetComponent<K_DisplaySuuji>().SetNum((int)iBreakNum);
            InitTransForm = DisplayNumObj.gameObject.transform.position;
        }
    }

    private void Update()
    {
        //if (this.gameObject == null)
        //{
        //}
        //else
        //{
        //   if (IsDisplayBreakNum == true)
        //   {
        //        // オブジェクト位置にテキスト要素を追従させる
        //        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //        TextEndurance.transform.position = new Vector3(screenPos.x + TextOffset.x, screenPos.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange) + TextOffset.y, screenPos.z); // 適切なオフセットを持たせる
        //    }
        //}
        DisplayNumObj.transform.position= new Vector3(InitTransForm.x, InitTransForm.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange), InitTransForm.z);
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
                    // 新しい空のゲームオブジェクトを作成
                    Debug.Log("再生オブジェクトを、生成ーッ!");
                    GameObject newGameObject = new GameObject("BreakObbbj");
                    // 位置を設定
                    newGameObject.transform.position = transform.position;
                    newGameObject.AddComponent<AudioSource>();
                    newGameObject.GetComponent<AudioSource>().PlayOneShot(audioclip);
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
                    // 新しい空のゲームオブジェクトを作成
                    Debug.Log("再生オブジェクトを、生成ーッ!");
                    GameObject newGameObject = new GameObject("BreakObbbj");
                    // 位置を設定
                    newGameObject.transform.position = transform.position;
                    newGameObject.AddComponent<AudioSource>();
                    newGameObject.GetComponent<AudioSource>().PlayOneShot(audioclip);
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
