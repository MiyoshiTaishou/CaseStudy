using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

//ダクトの移動処理
public class M_DuctWarp : MonoBehaviour
{
    //設定したダクトに移動する
    [Header("上ダクト"), SerializeField]
    private GameObject UpDuct;

    [Header("下ダクト"), SerializeField]
    private GameObject DownDuct;

    [Header("左ダクト"), SerializeField]
    private GameObject LeftDuct;

    [Header("右ダクト"), SerializeField]
    private GameObject RightDuct;

    [Header("移動にかかる時間"), SerializeField]
    private float fMoveTime = 1.0f;

    [Header("表示するUI"), SerializeField]
    private GameObject UIObj;

    [Header("入る時に鳴らすSE"), SerializeField]
    private AudioSource SEDuctEnter;

    [Header("移動時に鳴らすSE"), SerializeField]
    private AudioSource SEDuctMove;

    [Header("アニメーション"), SerializeField]
    private Vector3 animScale = Vector3.one;

    [Header("開始時イージング"), SerializeField]
    private M_Easing.Ease easeStart;

    [Header("終了時イージング"), SerializeField]
    private M_Easing.Ease easeEnd;

    [Header("アニメーション時間"), SerializeField]
    private float fAnimPlayTime = 0.3f;

    /// <summary>
    /// アニメーション時間
    /// </summary>
    private float fAnimTime = 0.0f;

    /// <summary>
    /// ダクトに触れている
    /// </summary>
    private bool isTouch;

    /// <summary>
    /// ダクトに触れている
    /// </summary>
    private Vector3 saveScale;

    /// <summary>
    /// ダクトマネージャ
    /// </summary>
    private GameObject DuctManager;    

    // 20240407 二宮追記
    /// <summary>
    /// 対象追跡カメラスクリプト
    /// </summary>
    private N_TrackingPlayer trackingPlayer=null;

    private K_5_4Camera trackingPlayer2=null;

    private bool isReverse = false;

    private float fTime;

    public bool isRock = false;

    [Header("Animation Duration")]
    [SerializeField] private float duration = 0.5f;

    [Header("ダクトに重なる壊れるブロックあれば")]
    [SerializeField] private GameObject RockObj;

    private bool init = false;

    private Animator animator;

    public bool GetisRock()
    {
        return isRock;
    }

    public void SetisRock(bool _rock)
    {
        isRock = _rock;
    }

    // Start is called before the first frame update
    void Start()
    {
        DuctManager = GameObject.Find("DuctManager");

        if (!UpDuct)
        {
            Debug.Log("上ダクトが見つかりません");
        }

        if (!DownDuct)
        {
            Debug.Log("下ダクトが見つかりません");
        }

        if (!LeftDuct)
        {
            Debug.Log("左ダクトが見つかりません");
        }

        if (!RightDuct)
        {
            Debug.Log("右ダクトが見つかりません");
        }

        //UI非表示
        UIObj.SetActive(false);

        // 20240407 二宮追記
        if (GameObject.Find("Main Camera").GetComponent<N_TrackingPlayer>())
        {
            trackingPlayer = GameObject.Find("Main Camera").GetComponent<N_TrackingPlayer>();
        }
        if(GameObject.Find("Main Camera").GetComponent<K_5_4Camera>())
        {
            trackingPlayer2 = GameObject.Find("Main Camera").GetComponent<K_5_4Camera>();
        }

        if (!trackingPlayer)
        {
            Debug.Log("NUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUL");
        }
        if (!trackingPlayer2)
        {
            Debug.Log("NUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUL");
        }

        saveScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            // ロックオブジェクトが設定されていたら
            if (RockObj)
            {
                isRock = true;
            }

            animator = GameObject.Find("Player").transform.GetChild(3).GetComponent<Animator>();

            init = true;
        }

        // ロック中に手前にあるオブジェクトがなくなったら
        if (isRock)
        {
            if(RockObj == null)
            {
                // ロック解除
                isRock = false;
            }
        }

        //ダクト移動中は処理しない
        if (trackingPlayer&&(DuctManager.GetComponent<M_DuctManager>().GetIsMove() || trackingPlayer.GetisWarp()))
        {
            // アニメーション完了後、元のスケールに戻す
            transform.localScale = saveScale;
            return;
        }

        if(trackingPlayer2&&(DuctManager.GetComponent<M_DuctManager>().GetIsMove() || trackingPlayer2.GetisWarp()))
        {
            // アニメーション完了後、元のスケールに戻す
            transform.localScale = saveScale;
            return;
        }

        //入る処理
        if (Input.GetButtonDown("Duct") && isTouch && M_GameMaster.GetGamePlay() && !animator.GetBool("ductFinish"))
        {
            //マネージャに自身のダクトにプレイヤーが入ったことを知らせる
            DuctManager.GetComponent<M_DuctManager>().SetContains(this.gameObject, true);

            StartCoroutine(IEDuctAnimStart(fAnimPlayTime));

            DuctManager.GetComponent<M_DuctManager>().PlayDuctInSE();
        }

        if (DuctManager.GetComponent<M_DuctManager>().GetValue(gameObject))
        {
            InDuctMove();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = true;
            UIObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = false;

            UIObj.SetActive(false);
        }
    }

    //ダクト同士を移動する
    void InDuctMove()
    {
        Debug.Log(gameObject);

        var func = M_Easing.GetEasingMethod(easeStart);

        if (isReverse)
        {
            fTime -= Time.deltaTime;
        }
        else
        {
            fTime += Time.deltaTime;
        }

        if (fTime > duration)
        {
            fTime = duration;
            isReverse = true;
        }
        else if (fTime < 0.0f)
        {
            fTime = 0.0f;
            isReverse = false;
        }

        float t = Mathf.Clamp01(fTime / duration);

        // アニメーションを適用
        this.gameObject.transform.localScale = saveScale + animScale * func(t);

        // キーボード入力を受け取る
        float fHorizontalInput = Input.GetAxis("Horizontal");
        float fVerticalInput = Input.GetAxis("Vertical");

        //上ダクトに移動
        if (fVerticalInput > 0.3f&& UpDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, UpDuct));
        }

        //下ダクトに移動
        if (fVerticalInput < -0.3f && DownDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, DownDuct));
        }

        //左ダクトに移動
        if (fHorizontalInput < -0.3f && LeftDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, LeftDuct));
        }

        //右ダクトに移動
        if (fHorizontalInput > 0.3f && RightDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, RightDuct));
        }
    }

    //ダクトに移動するメソッド
    IEnumerator IEMoveDuct(float _waitTime, GameObject _obj)
    {
        DuctManager.GetComponent<M_DuctManager>().SetIsMove(true);

        // 20240407 二宮追記
        if(trackingPlayer)
        {
            trackingPlayer.SetWarpInfo(_waitTime, _obj);
        }
        else if (trackingPlayer2)
        {
            trackingPlayer2.SetWarpInfo(_waitTime, _obj);
        }


        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        //ダクトマネージャのワープ処理を呼ぶ
        // アニメーション完了後、元のスケールに戻す
        transform.localScale = saveScale;
        DuctManager.GetComponent<M_DuctManager>().DuctWarp(_obj, this.gameObject);

        DuctManager.GetComponent<M_DuctManager>().SetIsMove(false);
    }

    IEnumerator IEDuctAnimStart(float _waitTime)
    {
        var func = M_Easing.GetEasingMethod(easeStart);

        // アニメーションが開始された時間を記録する
        float startTime = Time.time;

        while (Time.time - startTime < _waitTime)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / _waitTime);           

            // アニメーションを適用
            this.gameObject.transform.localScale = saveScale + animScale * func(t);

            yield return null;
        }

        StartCoroutine(IEDuctAnimEnd(_waitTime));
    }

    IEnumerator IEDuctAnimEnd(float _waitTime)
    {
        var func = M_Easing.GetEasingMethod(easeEnd);

        // アニメーションが開始された時間を記録する
        float startTime = Time.time;

        while (Time.time - startTime < _waitTime)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / _waitTime);

            t = 1 - t;

            // アニメーションを適用
            this.gameObject.transform.localScale = saveScale + animScale * func(t);

            yield return null;
        }

        // アニメーション完了後、元のスケールに戻す
        transform.localScale = saveScale;
    }

    public void RessetSize()
    {
        //元のスケールに戻す
        transform.localScale = saveScale;
    }
}
