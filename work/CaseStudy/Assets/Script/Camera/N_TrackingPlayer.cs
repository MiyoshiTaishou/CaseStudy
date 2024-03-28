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
        if(LeftUp != null)
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
        // 追跡対象の座標をカメラにセット
        CameraTransform.position = new Vector3(trans_Target.position.x, trans_Target.position.y, CameraTransform.position.z);

        // 画面端が両方セットされていたら
        if (bSet_LeftUp && bSet_RightDown)
        {
            // 範囲チェック
            AreaCheck();
        }
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
