using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Respawn : MonoBehaviour
{
    [Header("復活できる回数"), SerializeField]
    int nRespawn = 0;

    [Header("ヒットストップ"), SerializeField]
    int nHitStop = 0;

    [Header("復活時SE"),SerializeField]
    AudioClip acRespawn= null;
    [Header("敵接触時SE"),SerializeField]
    AudioClip acHit= null;
    [Header("チェックポイント取得時SE"),SerializeField]
    AudioClip acCheckPoint= null;

    [Header("復活演出時間"), SerializeField]
    private float RespwanTime = 0.0f;

    private float ElapsedRespawnTime = 0.0f;

    [Header("煙幕エフェクト"), SerializeField]
    private GameObject SmokePrefab;

    [Header("煙幕出現位置オフセット"), SerializeField]
    private Vector2 SmokeOffset = Vector2.zero;

    [Header("煙幕SE"), SerializeField]
    AudioClip acSmokeScreen = null;

    private GameObject effectPanel;

    public bool isRespawn = false;

    private GameObject smoke;

    private bool Init = false;

    private Animator TransitionAnimator;

    //コルーチン中かどうか
    bool isCoroutine = false;

    //復活位置
    Vector2 vecRespawnPos = Vector2.zero;

    //ゲッターセッターここから
    public
    Vector2 GetRespawnPos() { return vecRespawnPos; }
    public
    void SetReapawnPos(Vector2 _pos) { vecRespawnPos = _pos; }
    //ここまで

    AudioSource audiosource=null;

    private static bool IsFounded = false;
    public static void SetIsFounded(bool _IsFounded) { IsFounded = _IsFounded; }

    // Start is called before the first frame update
    void Start()
    {
        IsFounded = false;
    }

    public void Retry()
    {
        isRespawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Init)
        {
            vecRespawnPos = transform.position;
            audiosource = GetComponent<AudioSource>();
            if (!audiosource)
            {
                Debug.LogError("プレイヤーのスポーンのやつにAudioSourceがないって話だよね");
            }

            if (!SmokePrefab)
            {
                Debug.LogError("煙幕エフェクトがセットされてないって話だよん");
            }

            effectPanel = GameObject.Find("SceneEffect_Panel");
            if (!effectPanel)
            {
                Debug.Log("トランジションのパネルがシーンにないってハナシ");
            }
            else
            {
                TransitionAnimator = effectPanel.GetComponent<Animator>();
            }
        }

        if (isRespawn)
        {
            // 初期化
            if(ElapsedRespawnTime == 0.0f)
            {
                Vector3 initpos = new Vector3(
                    transform.position.x + SmokeOffset.x,
                    transform.position.y + SmokeOffset.y, 
                    SmokePrefab.transform.position.z);

                // 煙幕エフェクト生成
                smoke = Instantiate(SmokePrefab, initpos, Quaternion.identity);
                //smoke.transform.parent = gameObject.transform;
                smoke.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                audiosource.PlayOneShot(acSmokeScreen);
            }

            if (ElapsedRespawnTime > RespwanTime / 3.0f)
            {
                // プレイヤー非表示
                transform.parent.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }

            if (!smoke)
            {
                // 画面遷移アニメーション
                TransitionAnimator.SetBool("End", true);
            }

            if(ElapsedRespawnTime >= RespwanTime)
            {
                Debug.Log("リスポオオオオオオオオオオオオオオオオオオオオオオオオオオオオん");
                // 現在のSceneを取得
                Scene loadScene = SceneManager.GetActiveScene();
                // 現在のシーンを再読み込みする
                SceneManager.LoadScene(loadScene.name);
            }

            ElapsedRespawnTime += Time.deltaTime;
        }
        else
        {
            ElapsedRespawnTime = 0.0f;
            transform.parent.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //当たったのが敵なら復活
        if (collision.transform.CompareTag("Enemy") &&  //タグチェック
            isCoroutine == false&&
            IsFounded)                       //コルーチン中かチェック
        {
            StartCoroutine(HitStop());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //当たったのが敵なら復活
        if (collision.transform.CompareTag("Enemy") &&  //タグチェック
            isCoroutine == false &&
            IsFounded)                       //コルーチン中かチェック
        {
            StartCoroutine(HitStop());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たったのがチェックポイントなら復活位置更新
        if (collision.transform.CompareTag("Respawn"))
        {
            audiosource.PlayOneShot(acCheckPoint);
            SetReapawnPos(collision.transform.position);
            Destroy(collision.gameObject);
        }
    }

    //ヒットストップのコルーチン
    IEnumerator HitStop()
    {
        isCoroutine= true;
        isRespawn = true;

        //敵接触時音声再生
        audiosource.PlayOneShot(acHit);

        //Positionに関するコンポーネントのオンオフ
        transform.root.GetComponent<M_PlayerMove>().enabled = false;
        //transform.root.GetComponent<M_PlayerPush>().enabled = false;
        //transform.root.GetComponent<N_ProjecterSympathy>().enabled = false;
        transform.root.GetComponent<BoxCollider2D>().enabled = false;
        transform.root.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        
        //指定のフレーム待つ
        yield return new WaitForSeconds(nHitStop / 60);

        //transform.root.GetComponent<BoxCollider2D>().enabled = true;
        //transform.root.GetComponent<M_PlayerMove>().enabled = true;

        M_GameMaster.SetDethCount(1);

       // //復活orでっど
       // if (nRespawn > 0)
       // {
       //     //復活位置に転送
       //     transform.root.position = vecRespawnPos;
       // }
       // else if(nRespawn <= 0)
       // {
       //     //ゲームオーバーのフラグをオンにするとかの処理が入るのかもしれないよねって話だよね
       //     //デストロイ
       //     Destroy(transform.root.gameObject);
       // }
       // nRespawn--;

       ////復活時音声再生
       //audiosource.PlayOneShot(acRespawn);

       isCoroutine= false;
    }
}
