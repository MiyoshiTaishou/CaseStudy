using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_TrackingPlayer : MonoBehaviour
{

    // 画面端オブジェクトセット
    [Header("CameraEnd　左上"), SerializeField]
    private GameObject LeftUp;

    [Header("CameraEnd　右下"), SerializeField]
    private GameObject RightDpwn;

    [Header("追跡対象"), SerializeField]
    private GameObject Target;

    /// <summary>
    /// カメラの描画範囲
    /// </summary>
    [Header("カメラの描画範囲"), SerializeField]
    private float fViewSize = 5;

    /// <summary>
    /// カメラの描画範囲
    /// </summary>
    [Header("エリアチェックするか"), SerializeField]
    private bool isAreaCheck = true;

    /// <summary>
    /// カメラコンポーネント
    /// </summary>
    private Camera MainCamera;

    /// <summary>
    /// カメラの座標等情報
    /// </summary>
    private Transform CameraTransform;

    /// <summary>
    /// 左上座標等情報
    /// </summary>
    private Transform trans_LeftUp;

    /// <summary>
    /// 右下座標等情報
    /// </summary>
    private Transform trans_RightDown;

    /// <summary>
    /// 追跡対象座標等情報
    /// </summary>
    private Transform trans_Target;

    /// <summary>
    /// 画面端オブジェクトがセットされているか
    /// </summary>
    private bool bSet_LeftUp = false;

    /// <summary>
    /// 画面端オブジェクトがセットされているか
    /// </summary>
    private bool bSet_RightDown = false;

    /// <summary>
    /// 追跡方法ステートマシン列挙型
    /// </summary>
    private enum TrackingMethod
    {
        NORMAL,
        WARP,
    }

    /// <summary>
    /// 追跡方法ステートマシン
    /// </summary>
    private TrackingMethod _trackingmethod = TrackingMethod.NORMAL;

    /// <summary>
    /// 待ち時間
    /// </summary>
    private float waitTime = 0.0f;

    /// <summary>
    /// ワープ前座標
    /// </summary>
    private Vector2 BeforeWarpPos;

    /// <summary>
    /// ワープ後座標
    /// </summary>
    private Vector2 AfterWarpPos;

    /// <summary>
    /// ワープ追跡にかかる時間
    /// </summary>
    [Header("ワープ追跡にかかる時間"), SerializeField]
    private float warpTrackTime = 0.5f;

    /// <summary>
    /// ワープ追跡経過時間
    /// </summary>
    private float warpElapsedTime = 0.0f;

    private Vector2 WarpTargetVec;

    private bool isWarp = false;

    public bool GetisWarp()
    {
        return isWarp;
    }

    // =================================================================================

    // Start is called before the first frame update
    void Start()
    {
        // カメラコンポーネント取得
        MainCamera = this.gameObject.GetComponent<Camera>();
        // カメラ描画範囲設定
        MainCamera.orthographicSize = fViewSize;
        // 座標情報取得
        CameraTransform = this.gameObject.transform;
        trans_LeftUp = LeftUp.GetComponent<Transform>();
        trans_RightDown = RightDpwn.GetComponent<Transform>();

        // オブジェクトがセットされているかチェック
        if (Target == null)
        {
            Debug.Log("追跡対象となるオブジェクトをセットしてください");
        }
        // 追跡対象の座標情報取得
        trans_Target = Target.GetComponent<Transform>();

        // オブジェクトがセットされているかチェック
        if (LeftUp != null)
        {
            bSet_LeftUp = true;
        }
        if (RightDpwn != null)
        {
            bSet_RightDown = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 状態によって追跡方法を変更
        switch (_trackingmethod)
        {
            case TrackingMethod.NORMAL:
                NormalTracking();
                break;

            case TrackingMethod.WARP:
                WarpTracking();
                break;
        }

        //Debug.Log(_trackingmethod);

        // 画面端が両方セットされていたら
        if (bSet_LeftUp && bSet_RightDown && isAreaCheck)
        {
            // 範囲チェック
            AreaCheck();
        }
    }

    // 通常時の対象追跡
    private void NormalTracking()
    {
        // 追跡対象の座標をカメラにセット
        CameraTransform.position = new Vector3(trans_Target.position.x, trans_Target.position.y, CameraTransform.position.z);
    }

    // ワープ時の対象追跡
    private void WarpTracking()
    {
        // 初期化処理
        if (warpElapsedTime == 0.0f)
        {
            // プレイヤーからワープ先へのベクトル計算
            float subX = AfterWarpPos.x - BeforeWarpPos.x;
            float subY = AfterWarpPos.y - BeforeWarpPos.y;

            isWarp = true;

            // 記憶
            WarpTargetVec = new Vector2(subX, subY);
        }

        // 時間経過
        warpElapsedTime += Time.deltaTime;

        // 移動量
        Vector2 moveVec = WarpTargetVec * warpElapsedTime / warpTrackTime;

        // カメラの移動
        CameraTransform.position = new Vector3(
            BeforeWarpPos.x + moveVec.x,
            BeforeWarpPos.y + moveVec.y,
            CameraTransform.position.z);

        // 終了処理
        if (warpElapsedTime >= warpTrackTime)
        {
            // 通常時追跡処理に移行
            _trackingmethod = TrackingMethod.NORMAL;

            // 初期化
            warpElapsedTime = 0.0f;
            WarpTargetVec = Vector2.zero;
            AfterWarpPos = Vector2.zero;
            BeforeWarpPos = Vector2.zero;
            isWarp = false;
        }
    }

    // 外部からワープ追跡時に必要なオブジェクトをセット
    public void SetWarpInfo(float _waitTime, GameObject _obj)
    {
        // ワープ追跡に移行
        _trackingmethod = TrackingMethod.WARP;

        // ワープ後のオブジェクトの座標取得
        AfterWarpPos = new Vector2(_obj.transform.position.x, _obj.transform.position.y);

        // カメラ移動完了時間がワープ前の待ち時間より短いと挙動がおかしくなるため
        waitTime = _waitTime;
        if (warpTrackTime < waitTime)
        {
            warpTrackTime = waitTime + 0.1f;
        }

        // ワープ前の座標を取得
        BeforeWarpPos = new Vector2(CameraTransform.position.x, CameraTransform.position.y);

        // プレイヤーがワープするフレームから処理を開始するための無理やり呼び出し
        WarpTracking();
    }

    // 描画エリアを外れないようチェック
    private void AreaCheck()
    {
        const float adjust = 2.25f;

        // 上の制限範囲から出ようとしていたら
        if (CameraTransform.position.y + MainCamera.orthographicSize >= trans_LeftUp.position.y)
        {
            CameraTransform.position = new Vector3(
                CameraTransform.position.x,
                trans_LeftUp.position.y - MainCamera.orthographicSize,
                CameraTransform.position.z);
        }

        // 左の制限範囲から出ようとしていたら
        if (CameraTransform.position.x - MainCamera.orthographicSize * adjust <= trans_LeftUp.position.x)
        {
            CameraTransform.position = new Vector3(
                trans_LeftUp.position.x + MainCamera.orthographicSize * adjust,
                CameraTransform.position.y,
                CameraTransform.position.z);
        }

        // 下の制限範囲から出ようとしていたら
        if (CameraTransform.position.y - MainCamera.orthographicSize <= trans_RightDown.position.y)
        {
            CameraTransform.position = new Vector3(
                CameraTransform.position.x,
                trans_RightDown.position.y + MainCamera.orthographicSize,
                CameraTransform.position.z);
        }

        // 右の制限範囲から出ようとしていたら
        if (CameraTransform.position.x + MainCamera.orthographicSize * adjust >= trans_RightDown.position.x)
        {
            CameraTransform.position = new Vector3(
                trans_RightDown.position.x - MainCamera.orthographicSize * adjust,
                CameraTransform.position.y,
                CameraTransform.position.z);
        }
    }
}
