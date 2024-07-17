using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class M_PlayerPush : MonoBehaviour
{
    /// <summary>
    /// どの状態の時に押せるようにするか
    /// </summary>
    enum MODE
    {
        Back,
        Blinding,
        None,
    }

    [Header("押せる条件"), SerializeField]
    MODE mode;

    [Header("押す力"), SerializeField]
    float fPower = 5.0f;

    [Header("振動時間"), SerializeField]
    private float fTime = 0.5f;

    [Header("風のアニメーション再生タイミング"), SerializeField]
    private float fDlayAnim = 1.0f;

    [Header("ヒットストップのフレーム"), SerializeField]
    float fHitStop = 10.0f;

    [Header("風の音"), SerializeField]
    AudioClip ac = null;

    /// <summary>
    /// プレイヤー
    /// </summary>
    GameObject PlayerObj;

    private M_PlayerMove PlayerMove;

    GameObject AnimBonePlayer;

    /// <summary>
    /// ダクトマネージャ
    /// </summary>
    GameObject DuctManager;

    /// <summary>
    /// 押すオブジェクト
    /// </summary>
    GameObject PushObj;

    List<GameObject> PushList = new List<GameObject>();

    /// <summary>
    /// 押せるかどうか
    /// </summary>
    private bool isPush = false;

    /// <summary>
    ///アニメーション関連
    /// </summary>
    private Animator animator;

    private AudioSource audioSource;

    // 前フレームの入力状態を保持
    private bool wasPushButtonPressed = false;

    private bool isWindAnim = false;

    private float ElapsedRayTime = 0.0f;

    public bool isCheck = false;

    public bool isRaycast = false;

    public bool isSearch = false;

    private bool isTutoPushOK = true;

    public void SetOKPush(bool _push)
    {
        isTutoPushOK = _push;
    }

    [Header("レイヤーマスク設定"), SerializeField]
    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = gameObject.transform.parent.gameObject;

        PlayerMove = PlayerObj.GetComponent<M_PlayerMove>();

        AnimBonePlayer = transform.parent.GetChild(3).gameObject;

        DuctManager = GameObject.Find("DuctManager");

        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!M_GameMaster.GetGamePlay() || DuctManager.GetComponent<M_DuctManager>().ContainsTrueValue() || M_GameMaster.GetGameClear())
        {
            return;
        }

        // 入力の状態を取得
        bool isPushButtonPressed = Input.GetAxis("EnemyPush") > 0.5f;

        // 追跡対象が視野範囲内に入った時
        // 壁を挟んでいるかを判定する
        if (isRaycast/* && !isCheck*/)
        {
            RayCastCheck();
        }

        // プッシュボタンが押されたフレームである
        // 押せる状態である
        // 押すオブジェクトがnullでない
        // 押したときのアニメーションの再生中でない
        // チュートリアルがあるとき、押してもいい状態である
        if (isPushButtonPressed && !wasPushButtonPressed && isPush && PushObj && !animator.GetCurrentAnimatorStateInfo(0).IsName("kaze01") && isTutoPushOK)
        {
            float Distance = 100.0f;
            Vector3 pos = PlayerObj.transform.position;
            for (int i = 0; i < PushList.Count; i++)
            {
                float check = (PushList[i].transform.position - pos).magnitude;
                if (check < Distance)
                {
                    Distance = check;
                    PushObj = PushList[i];
                }
            }
            if (!PushObj)
            {
                PushObj = PushList[0];
            }
            Push(PushObj);
        }

        if (isTutoPushOK)
        {
            if (Input.GetKeyDown(KeyCode.Return) || (isPushButtonPressed && !wasPushButtonPressed))
            {
                // 風のアニメーションが再生中で鳴ければ風を起こす
                if (!isWindAnim)
                {
                    StartCoroutine(IEAnimDlay(fDlayAnim));
                    audioSource.PlayOneShot(ac);
                    if (GetComponent<M_RandomSEPlay>())
                    {
                        GetComponent<M_RandomSEPlay>().PlayRandomSoundEffect();
                    }
                }
            }
        }

        //アニメーション再生中は動かないようにする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("kaze01"))
        {
            isWindAnim = true;
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            animator.SetBool("run", false);
        }
        else
        {
            isWindAnim = false;
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
        }

        // 前フレームの入力状態を更新
        wasPushButtonPressed = isPushButtonPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRaycast || isSearch)
        {
            return;
        }

        if (!collision.isTrigger)
        {
            if (collision.tag == "Enemy" || collision.tag == "EnemyBall")
            {
                isRaycast = true;
            }
        }
    }

    private void RayCastCheck()
    {
        //Debug.Log("例とバス");

        Vector3 startPoint = PlayerObj.transform.position;
        // プレイヤーの向き取得
        Vector2 direction = Vector2.right;
        if(PlayerObj.transform.eulerAngles.y == 180.0f)
        {
            direction = Vector2.left;
        }

        float distance = ElapsedRayTime * 30.0f;
        ElapsedRayTime += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, distance, layerMask);

        Debug.DrawRay(startPoint, direction * distance, Color.black, 0.0f, false);

        // 先に敵に当たったら追跡
        // 壁に当たったらなにもなし
        if (hit.collider != null)
        {
            //Debug.Log("ヒットした");
            ElapsedRayTime = 0.0f;
            isCheck = true;

            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("EnemyBall"))
            {
                //Debug.Log("押せる");
                isSearch = true;

                //isRaycast = false;
            }
            else if (hit.collider.gameObject.CompareTag("Ground") /*|| hit.collider.gameObject.CompareTag("EnemyBall")*/)
            {
                //Debug.Log("壁検知");

                isSearch = false;
                //isRaycast = false;
            }
            else
            {
                //Debug.Log("なんか違うもの");
            }
        }
        else
        {
            //Debug.Log("ヒットなし");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isSearch)
        {
            return;
        }

        if (!collision.isTrigger)
        {
            if (collision.tag == "Enemy" || collision.tag == "EnemyBall")
            {
                //// 敵とヒット中にホログラムの壁を非表示にした際
                //// レイキャストが終了していた場合、押せなくなるのを修正する
                //if (Input.GetButtonDown("SympathyButton"))
                //{
                //    isCheck = false;
                //    isRaycast = true;
                //}

                if (isSearch)
                {
                    isPush = true;
                    //押すオブジェクト代入
                    PushObj = collision.gameObject;
                    if (!PushList.Contains(collision.gameObject))
                    {
                        PushList.Add(collision.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.tag == "Enemy" || collision.tag == "EnemyBall")
            {
                isPush = false;
                PushObj = null;

                isRaycast = false;
                isSearch = false;
                isCheck = false;

                if (PushList.Contains(collision.gameObject))
                {
                    PushList.Remove(collision.gameObject);
                }
            }
        }
    }

    //押す処理
    void Push(GameObject push)
    {
        //押せる条件
        switch (mode)
        {
            //条件なし
            case MODE.None:
                ExecutePush(push);
                break;

            //目くらまし中
            case MODE.Blinding:
                if (push.GetComponent<M_BlindingMove>().GetIsBlinding())
                {
                    ExecutePush(push);
                }
                break;

            //バレていない時
            case MODE.Back:
                N_PlayerSearch search = push.gameObject.transform.GetChild(0).gameObject.GetComponent<N_PlayerSearch>();
                if (!search.GetIsSearch())
                {
                    //Debug.Log("押した");
                    StartCoroutine(HitStop(push));
                    StartCoroutine(M_Utility.GamePadMotor(fTime));
                }
                break;
        }
    }

    void ExecutePush(GameObject push)
    {
        Vector3 dir;

        if (this.transform.eulerAngles.y == 180.0f)
        {
            dir = transform.right;
        }
        else
        {
            dir = -transform.right;
        }

        push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
        push.GetComponent<S_EnemyBall>().SetisPushing(true);
    }

    IEnumerator IEAnimDlay(float _waitTime)
    {        
        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        animator.SetTrigger("Start");
        Animator anim = AnimBonePlayer.GetComponent<Animator>();
        anim.SetTrigger("push");
        anim.SetBool("run", false);
    }

    IEnumerator HitStop(GameObject push)
    {
        Vector3 dir = Vector3.zero;

        if (PlayerObj.transform.eulerAngles.y >= 180.0f)
        {
            dir = transform.right;
        }
        else if (PlayerObj.transform.eulerAngles.y <= 60.0f)
        {
            Debug.Log(PlayerObj.transform.eulerAngles);
            dir = transform.right;
        }
        //audioSource.PlayOneShot(ac);
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(fHitStop / 60);

        push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
        push.GetComponent<S_EnemyBall>().SetisPushing(true);
    }
}
