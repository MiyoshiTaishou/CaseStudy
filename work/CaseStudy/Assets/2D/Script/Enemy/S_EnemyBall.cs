using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Rendering;
#endif
using UnityEngine;

public class S_EnemyBall : MonoBehaviour
{
    [Header("増幅係数"), SerializeField]
    float fBoost = 1.5f;

    [Header("AudioClip"), SerializeField]
    AudioClip audioclip;

    [Header("ヒットストップ"), SerializeField]
    float fHitStop = 0;

    [Header("制限速度(x)"), SerializeField]
    float fLimitSpeedx = 15.0f;

    [Header("停止判定"), SerializeField]
    float fStopjudge = 0.0f;

    [Header("大きさの段階と必要な吸収数"), SerializeField]
    int[] nGiantNum;

    [Header("振動時間"), SerializeField]
    private float fTime = 0.1f;

    [Header("敵衝突エフェクト"), SerializeField]
    private GameObject Eff_ClashPrefab;
    private GameObject Eff_ClashObj;

    [Header("敵回転エフェクト"), SerializeField]
    private GameObject Eff_RollingPrefab;
    private GameObject Eff_RollingObj;

    //塊になっているかどうか
    private bool isBall = false;
    public bool GetisBall() { return isBall; }

    //プレイヤーによって押されているかどうか
    private bool isPushing = false;
    public bool GetisPushing() { return isPushing; }
    public void SetisPushing(bool _flg) { isPushing = _flg; }

    private GameObject ColObject;

    private Vector3 defaultScale;

    //くっついている個数
    private float fStickCnt = 0;

    private Rigidbody2D rb;

    private Vector2 vel;

    // 隊列から除名されているか
    private bool isDeleteMember = false;

    //左向きに移動しているか
    private bool isLeft = false;

    public bool GetisLeft() { return isLeft; }

    public int GetStickCount() 
    {
        int temp = 0;
        temp = Mathf.FloorToInt(fStickCnt);
        return temp; }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale= transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        if(Eff_RollingPrefab)
        {
            Eff_RollingObj= Instantiate(Eff_RollingPrefab, transform.position, Quaternion.identity);
            Eff_RollingObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Eff_RollingObj)
        {
            Eff_RollingObj.transform.position = this.gameObject.transform.position;
        }
        if (isPushing)
        {
            GetComponent<SEnemyMove>().enabled = false;
            //GetComponent<M_BlindingMove>().enabled = false;
            //GetComponent<N_PlayerSearch>().enabled = false;
            vel = rb.velocity;
            if(vel.x<0)
            {
                isLeft= true;
            }
            else if(vel.x>0) 
            {
                isLeft= false;
            }
            // 隊列から除名
            DeleteMember();
        }
        if(isBall&& Mathf.Abs(rb.velocity.x) < fStopjudge) 
        {
            isPushing = false;
        }
    }

    private void DeleteMember()
    {
        if (!isDeleteMember)
        {
            SEnemyMove sEnemyMove = gameObject.GetComponent<SEnemyMove>();
            N_EnemyManager enemyMana = sEnemyMove.GetManager();

            // 現在の状態取得
            N_EnemyManager.ManagerState state = enemyMana.GetState();

            // 状態変更
            enemyMana.ChangeManagerState(N_EnemyManager.ManagerState.ECPULSION);

            // 除名関数呼び出し(隊列番号、現在のマネージャーの状態)
            enemyMana.EcpulsionMember(sEnemyMove.GetTeamNumber(), state);

            isDeleteMember = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject= _collision.gameObject;
        S_EnemyBall colEnemyBall = ColObject.GetComponent<S_EnemyBall>();
        if (ColObject.CompareTag("Enemy")||ColObject.CompareTag("EnemyBall"))
        {
            if (!colEnemyBall.GetisPushing()||
                (colEnemyBall.GetisPushing()&&fStickCnt > colEnemyBall.GetStickCount()))
            {
                if (Eff_RollingObj)
                {
                    Eff_RollingObj.SetActive(true);
                }

                isBall = true;
                //Destroy(GetComponent<N_PlayerSearch>());
                if (!colEnemyBall.GetisBall())
                {
                    fStickCnt++;
                }
                else if(colEnemyBall.GetisBall())
                {
                    fStickCnt += colEnemyBall.GetStickCount();
                }
                
                if(fStickCnt==1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x += GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;

                // 隊列から除名
                colEnemyBall.DeleteMember();

                Destroy(ColObject);
                rb.AddForce(vel*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch+=0.2f;

                StartCoroutine(HitStop());
                StartCoroutine(M_Utility.GamePadMotor(fTime));
                if (Eff_ClashPrefab != null)
                {
                    Eff_ClashObj = Instantiate(Eff_ClashPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject= _collision.gameObject;
        S_EnemyBall colEnemyBall = ColObject.GetComponent<S_EnemyBall>();
        if (ColObject.CompareTag("Enemy") || ColObject.CompareTag("EnemyBall"))
        {
            if (!colEnemyBall.GetisPushing()||
                (colEnemyBall.GetisPushing()&&fStickCnt > colEnemyBall.GetStickCount()))
            {
                if (Eff_RollingObj)
                {
                    Eff_RollingObj.SetActive(true);
                }

                isBall = true;
                //Destroy(GetComponent<N_PlayerSearch>());
                fStickCnt++;
                if (fStickCnt == 1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x += GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;

                // 隊列から除名
                colEnemyBall.DeleteMember();

                Destroy(ColObject);
                rb.AddForce(rb.velocity*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                StartCoroutine(HitStop());
                if (Eff_ClashPrefab != null)
                {
                    Eff_ClashObj = Instantiate(Eff_ClashPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
    IEnumerator HitStop()
    {
        //速度を保存し、0にする
        Vector2 vel=rb.velocity;
        if(vel.x>fLimitSpeedx)
        {
            vel.x = fLimitSpeedx;
        }
        else if(vel.x<-fLimitSpeedx)
        {
            vel.x = -fLimitSpeedx;
        }
        rb.velocity = Vector2.zero;
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(fHitStop/60);
        //保存した速度で再開する
        rb.velocity = vel;
        isPushing = true;
    }

    //何段階巨大化したかを取得する関数
    private int GetGiantLv()
    {
        int temp = Mathf.FloorToInt(fStickCnt);
        int[] array = nGiantNum;
        int lv = 0;
        int i = 0;
        while(i<nGiantNum.Length) 
        {
            temp -= nGiantNum[i];
            if(temp>=0)
            {
                lv++;
            }
            else if(temp<0)
            {
                break;
            }
            i++;
        }
        return lv;
    }
}
