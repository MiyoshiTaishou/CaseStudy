using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class M_CameraSlideIn : MonoBehaviour
{
    [Header("カメラ移動のイージング関数"),SerializeField]
    private M_Easing.Ease easeCamMove;

    [Header("開始地点のオブジェクト"), SerializeField]
    private GameObject StartObj;

    [Header("終了地点のオブジェクト"), SerializeField]
    private GameObject EndObj;

    [Header("移動にかける時間")]
    [SerializeField] private float durationMove = 1.0f;

    [Header("カメラズームアウトのイージング関数"), SerializeField]
    private M_Easing.Ease easeCamOut;

    [Header("どれだけズームアウトするか"), SerializeField]
    private float outDis;

    [Header("ズームアウトにかける時間")]
    [SerializeField] private float durationOut = 1.0f;

    /// <summary>
    /// 移動の計測時間
    /// </summary>
    private float fTimeMove;

    /// <summary>
    /// 移動の計測時間
    /// </summary>
    private float fTimeOut;

    /// <summary>
    /// カメラのZ座標
    /// </summary>
    private float camPosZ;

    /// <summary>
    /// カメラコンポーネント
    /// </summary>
    private Camera cam;

    /// <summary>
    /// 開始時の距離
    /// </summary>
    private float camZoom;

    // Start is called before the first frame update
    void Start()
    {
        camPosZ = this.transform.position.z;
        cam = GetComponent<Camera>();
        camZoom = cam.orthographicSize;

        this.transform.position = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        //アウト処理
        fTimeOut += Time.deltaTime;
        if(fTimeOut > durationOut)
        {
            fTimeOut = durationOut;

            //移動処理
            fTimeMove += Time.deltaTime;
            if (fTimeMove > durationMove)
            {
                fTimeMove = durationMove;
            }
            EasingMove();
        }
        else
        {
            EasingOut();
        }             
    }

    //移動のイージング
    private void EasingMove()
    {
        float t = Mathf.Clamp01(fTimeMove / durationMove);
             
        var func = M_Easing.GetEasingMethod(easeCamMove);

        Vector3 startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        Vector3 endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);
        this.transform.position = startPos + (endPos - startPos) * func(t);
    }

    //アウトイージング
    private void EasingOut()
    {
        float t = Mathf.Clamp01(fTimeOut / durationOut);

        var func = M_Easing.GetEasingMethod(easeCamOut);

        cam.orthographicSize = camZoom - outDis * func(t);
    }
}
