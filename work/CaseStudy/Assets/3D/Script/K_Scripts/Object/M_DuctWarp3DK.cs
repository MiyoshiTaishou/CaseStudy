using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダクトの移動処理
public class M_DuctWarp3DK : MonoBehaviour
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

    /// <summary>
    /// 移動中
    /// </summary>
    private bool isMoveDuct;

    // 20240407 二宮追記
    /// <summary>
    /// 対象追跡カメラスクリプト
    /// </summary>
    private N_TrackingPlayer trackingPlayer;

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
        trackingPlayer = GameObject.Find("Main Camera").GetComponent<N_TrackingPlayer>();

        saveScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //ダクト移動中は処理しない
        if (isMoveDuct)
        {
            return;
        }

        //入る処理
        if (Input.GetButtonDown("Duct") && isTouch && M_GameMaster.GetGamePlay())
        {
            //マネージャに自身のダクトにプレイヤーが入ったことを知らせる
            DuctManager.GetComponent<M_DuctManager3DK>().SetContains(this.gameObject, true);

            StartCoroutine(IEDuctAnimStart(fAnimPlayTime));
        }

        if (DuctManager.GetComponent<M_DuctManager3DK>().GetValue(gameObject))
        {
            InDuctMove();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = true;

            UIObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
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
        isMoveDuct = true;

        // 20240407 二宮追記
        //trackingPlayer.SetWarpInfo(_waitTime, _obj);

        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        //ダクトマネージャのワープ処理を呼ぶ
        DuctManager.GetComponent<M_DuctManager3DK>().DuctWarp(_obj, this.gameObject);

        isMoveDuct = false;
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
}
