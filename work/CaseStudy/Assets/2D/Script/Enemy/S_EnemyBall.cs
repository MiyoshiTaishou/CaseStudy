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

    //停止判定X
    float fStopjudge = 1.0f;
    //停止判定Y
    float fStopjudgeY = 1.0f;

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
    public bool isBall = false;
    // たまになって動いているか
    private bool isHitStop = false;
    public bool GetisBall() { return isBall; }

    //プレイヤーによって押されているかどうか
    public bool isPushing = false;
    public bool GetisPushing() { return isPushing; }

    public bool GetisHitStop()
    {
        return isHitStop;
    }
    public void SetisPushing(bool _flg) { isPushing = _flg; }

    private GameObject ColObject;

    private Vector3 defaultScale;

    //くっついている個数
    public float fStickCnt = 0;

    private Rigidbody2D rb;

    private Vector2 vel;

    // 隊列から除名されているか
    private bool isDeleteMember = false;

    //左向きに移動しているか
    private bool isLeft = false;

    public bool GetisLeft() { return isLeft; }

    private BoxCollider2D BocCol2D;
    private Vector3 ColliderSize;
    private Vector3 ColliderOffset;

    private Animator animator;

    public void SetEffectActivation(bool flg)
    {
        if(Eff_RollingObj)
        {
            Eff_RollingObj.SetActive(flg);
        }
    }

    public int GetStickCount()
    {
        int temp = 0;
        temp = Mathf.FloorToInt(fStickCnt);
        return temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        if (Eff_RollingPrefab)
        {
            Eff_RollingObj = Instantiate(Eff_RollingPrefab, transform.position, Quaternion.identity);
            Eff_RollingObj.SetActive(false);
            Eff_RollingObj.transform.parent = gameObject.transform;
        }
        BocCol2D = this.GetComponent<BoxCollider2D>();
        ColliderSize = BocCol2D.size / this.transform.localScale;
        ColliderOffset = BocCol2D.offset;
        Debug.Log(ColliderSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Eff_RollingObj)
        {
            Eff_RollingObj.transform.position = this.gameObject.transform.position;
        }
        if (isPushing)
        {
            GetComponent<SEnemyMove>().enabled = false;
            //GetComponent<M_BlindingMove>().enabled = false;
            //GetComponent<N_PlayerSearch>().enabled = false;
            vel = rb.velocity;
            if (vel.x < 0)
            {
                isLeft = true;
            }
            else if (vel.x > 0)
            {
                isLeft = false;
            }
            // 隊列から除名
            DeleteMember();

        }
        else if (!isPushing && !isBall)
        {
            GetComponent<SEnemyMove>().enabled=true;

            if (isDeleteMember)
            {
                // 押される前にいた隊列に戻る
                SEnemyMove sEnemyMove = gameObject.GetComponent<SEnemyMove>();
                N_EnemyManager enemyMana = sEnemyMove.GetManager();

                // 隊列に追加
                if (enemyMana != null)
                {
                    enemyMana.TeamAddEnemy(this.gameObject);
                }
                else
                {
                    // マネージャー生成
                    GameObject manager = new GameObject();
                    manager.transform.parent = this.gameObject.transform.parent.transform;
                    manager.name = "EnemyManager";
                    N_EnemyManager sc_mana = manager.AddComponent<N_EnemyManager>();
                    sc_mana.InitMoveStatus();
                    sc_mana.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);

                    sc_mana.TeamAddEnemy(this.gameObject);

                }
                isDeleteMember = false;
            }

        }
        if (/*isBall&& */Mathf.Abs(rb.velocity.x) < fStopjudge && Mathf.Abs(rb.velocity.y) < fStopjudgeY) 
        {
            isPushing = false;
        }
        if(isBall && gameObject.GetComponent<Rigidbody2D>().velocity.x >= 1.0f)
        {
            if (Eff_RollingObj)
            {
                Eff_RollingObj.SetActive(false);
            }
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

            // ディレクトリ変更
            transform.parent = gameObject.transform.parent.transform.parent.transform;

            isDeleteMember = true;
        }
    }

    public void SetFallHitChangeBall(GameObject _obj)
    {
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject = _obj.gameObject;
        S_EnemyBall colEnemyBall = ColObject.GetComponent<S_EnemyBall>();
        if (ColObject.CompareTag("Enemy") || ColObject.CompareTag("EnemyBall"))
        {
            if (!colEnemyBall.GetisPushing() ||
                fStickCnt > colEnemyBall.GetStickCount())
            {
                if (Eff_RollingObj)
                {
                    Eff_RollingObj.SetActive(true);
                }

                isBall = true;

                //Destroy(GetComponent<N_PlayerSearch>());

                if (fStickCnt == 0)
                {
                    Debug.Log("とぅるっるるる");
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }

                if (!colEnemyBall.GetisBall())
                {
                    fStickCnt++;
                    Debug.Log("相手が玉ちゃう");
                }
                else if (colEnemyBall.GetisBall())
                {
                    Debug.Log(fStickCnt + colEnemyBall.GetStickCount() + "=");
                    fStickCnt += colEnemyBall.GetStickCount();

                    Debug.Log(fStickCnt);
                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x += GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Vector3 offset = ColliderOffset;
                offset.y -= 0.5f;
                BocCol2D.offset = offset;
                Vector3 size = ColliderSize;
                size.y *= 0.5f;
                BocCol2D.size = size;
                // 隊列から除名
                colEnemyBall.DeleteMember();

                Destroy(ColObject);
                //rb.AddForce(vel * fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch += 0.2f;

                StartCoroutine(HitStop());
                //StartCoroutine(M_Utility.GamePadMotor(fTime));
                if (Eff_ClashPrefab != null)
                {
                    Eff_ClashObj = Instantiate(Eff_ClashPrefab, transform.position, Quaternion.identity);
                }

                isPushing = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject = _collision.gameObject;
        S_EnemyBall colEnemyBall = ColObject.GetComponent<S_EnemyBall>();
        if (ColObject.CompareTag("Enemy") || ColObject.CompareTag("EnemyBall"))
        {
            if (colEnemyBall.GetisPushing() && fStickCnt == colEnemyBall.GetStickCount()&&
                transform.position.y>ColObject.transform.position.y)
            {
                fStickCnt += fStickCnt;
                // 隊列から除名
                colEnemyBall.DeleteMember();

                Destroy(ColObject);
                rb.AddForce(vel * fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch += 0.2f;

                StartCoroutine(HitStop());
                StartCoroutine(M_Utility.GamePadMotor(fTime));
                if (Eff_ClashPrefab != null)
                {
                    Eff_ClashObj = Instantiate(Eff_ClashPrefab, transform.position, Quaternion.identity);
                }
            }
            else if (!colEnemyBall.GetisPushing() ||
                (colEnemyBall.GetisPushing() && fStickCnt > colEnemyBall.GetStickCount()))
            {
                if (Eff_RollingObj)
                {
                    Eff_RollingObj.SetActive(true);
                }

                isBall = true;

                //Destroy(GetComponent<N_PlayerSearch>());

                if (fStickCnt == 0)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }

                if (!colEnemyBall.GetisBall())
                {
                    Debug.Log("相手が玉ちゃう");

                    fStickCnt++;
                }
                else if (colEnemyBall.GetisBall())
                {
                    Debug.Log(fStickCnt.ToString() + colEnemyBall.GetStickCount().ToString() + "=");

                    fStickCnt += colEnemyBall.GetStickCount();

                    Debug.Log(fStickCnt);

                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x += GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Vector3 offset = ColliderOffset;
                offset.y -= 0.5f;
                BocCol2D.offset = offset;
                Vector3 size = ColliderSize;
                size.y *= 0.5f;
                BocCol2D.size = size;
                // 隊列から除名
                colEnemyBall.DeleteMember();

                Destroy(ColObject);
                rb.AddForce(vel * fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch += 0.2f;

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
        if (!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject = _collision.gameObject;
        S_EnemyBall colEnemyBall = ColObject.GetComponent<S_EnemyBall>();
        if (ColObject.CompareTag("Enemy") || ColObject.CompareTag("EnemyBall"))
        {
            if (!colEnemyBall.GetisPushing() ||
                (colEnemyBall.GetisPushing() && fStickCnt > colEnemyBall.GetStickCount()))
            {
                if (Eff_RollingObj)
                {
                    Eff_RollingObj.SetActive(true);
                }

                isBall = true;

                //Destroy(GetComponent<N_PlayerSearch>());
                //fStickCnt++;
                //if (fStickCnt == 1)
                //{
                //    fStickCnt++;
                //    transform.tag = "EnemyBall";
                //}

                if (fStickCnt == 0)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }

                if (!colEnemyBall.GetisBall())
                {
                    Debug.Log("相手が玉ちゃう");

                    fStickCnt++;
                }
                else if (colEnemyBall.GetisBall())
                {
                    Debug.Log(fStickCnt.ToString() + colEnemyBall.GetStickCount().ToString() + "=");

                    fStickCnt += colEnemyBall.GetStickCount();

                    Debug.Log(fStickCnt);

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
                rb.AddForce(rb.velocity * fBoost, ForceMode2D.Impulse);
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
        isHitStop = true;
        //速度を保存し、0にする
        Vector2 vel = rb.velocity;
        if (vel.x > fLimitSpeedx)
        {
            vel.x = fLimitSpeedx;
        }
        else if (vel.x < -fLimitSpeedx)
        {
            vel.x = -fLimitSpeedx;
        }
        rb.velocity = Vector2.zero;
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(fHitStop / 60);
        //保存した速度で再開する
        rb.velocity = vel;
        isPushing = true;
        isHitStop = false;
    }

    //何段階巨大化したかを取得する関数
    private int GetGiantLv()
    {
        int temp = Mathf.FloorToInt(fStickCnt);
        int[] array = nGiantNum;
        int lv = 0;
        int i = 0;
        while (i < nGiantNum.Length)
        {
            temp -= nGiantNum[i];
            if (temp >= 0)
            {
                lv++;
            }
            else if (temp < 0)
            {
                break;
            }
            i++;
        }
        return lv;
    }

    private void OnDestroy()
    {
        if(Eff_RollingObj)
        {
            Destroy(Eff_RollingObj);
        }
    }
}
