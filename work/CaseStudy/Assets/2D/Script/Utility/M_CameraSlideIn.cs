using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Tilemaps;
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

    private bool isOnce = false;
    private bool isOnce2 = false;

    [Header("タイルマップ"), SerializeField]
    public Tilemap tilemap; // タイルマップ

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    Vector3 startPos;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<N_TrackingPlayer>().enabled = false;

        camPosZ = this.transform.position.z;
        cam = GetComponent<Camera>();
        camZoom = cam.orthographicSize;

        M_GameMaster.SetGamePlay(false);


        if (tilemap)
        {
            // タイルマップの範囲を計算
            CalculateBounds();
        }

        startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);

        this.transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {      
        Debug.Log(M_GameMaster.GetGamePlay());
        //アウト処理
        fTimeOut += Time.deltaTime;
        if(fTimeOut > durationOut)
        {
            fTimeOut = durationOut;

            //移動処理
            fTimeMove += Time.deltaTime;
            if (fTimeMove > durationMove && !isOnce)
            {
                Debug.Log("動いとる？");
                fTimeMove = durationMove;
                GetComponent<N_TrackingPlayer>().enabled = true;
                if (!isOnce2)
                {
                    M_GameMaster.SetGamePlay(true);
                }
                isOnce2 = true;

                GetComponent<M_CameraSlideIn>().enabled = false;

            }

            EasingMove();
        }
        else
        {
            EasingOut();
        }

        if (fTimeMove > durationMove && !isOnce)
        {
            Debug.Log("時間オーバー");
            if(!M_GameMaster.GetGamePlay() && !isOnce)
            {
                fTimeMove = durationMove;
                GetComponent<N_TrackingPlayer>().enabled = true;
                M_GameMaster.SetGamePlay(true);
                isOnce = true;
                //cam.orthographicSize = camZoom;
                GetComponent<M_CameraSlideIn>().enabled = false;
                cam.orthographicSize = camZoom;
                Debug.Log("今じゃ！");
            }
        }
        if(camZoom < cam.orthographicSize)
        {
            cam.orthographicSize = camZoom;
        }

        if (tilemap)
        {
            camHalfHeight = Camera.main.orthographicSize;
            camHalfWidth = camHalfHeight * Camera.main.aspect;
            if (this.transform.position.y <= minBounds.y + camHalfHeight)
            {
                this.transform.position = new Vector3(this.transform.position.x, minBounds.y + camHalfHeight, this.transform.position.z);
            }
            Vector3 newPosition = this.transform.position;
            float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            this.transform.position = new Vector3(clampedX, this.transform.position.y, this.transform.position.z);
        }
    }

    //移動のイージング
    private void EasingMove()
    {
        float t = Mathf.Clamp01(fTimeMove / durationMove);
             
        var func = M_Easing.GetEasingMethod(easeCamMove);

        //Vector3 startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        //Vector3 endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);

        this.transform.position = startPos + (endPos - startPos) * func(t);
    }

    //アウトイージング
    private void EasingOut()
    {
        float t = Mathf.Clamp01(fTimeOut / durationOut);

        var func = M_Easing.GetEasingMethod(easeCamOut);

        cam.orthographicSize = camZoom - outDis * func(t);
    }

    void CalculateBounds()
    {
        // 初期値を非常に大きな/小さな値に設定
        Vector3Int minCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int maxCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        // タイルマップのすべてのセルをチェック
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (pos.x < minCell.x) minCell.x = pos.x;
                if (pos.y < minCell.y) minCell.y = pos.y;
                if (pos.z < minCell.z) minCell.z = pos.z;

                if (pos.x > maxCell.x) maxCell.x = pos.x;
                if (pos.y > maxCell.y) maxCell.y = pos.y;
                if (pos.z > maxCell.z) maxCell.z = pos.z;
            }
        }

        // タイルマップの左下端と右上端のワールド座標を計算
        Vector3 minWorld = tilemap.CellToWorld(minCell);
        Vector3 maxWorld = tilemap.CellToWorld(maxCell) + tilemap.cellSize;

        // オフセットを追加してカメラがステージ外を映さないようにする
        minBounds = new Vector2(minWorld.x, minWorld.y);
        maxBounds = new Vector2(maxWorld.x, maxWorld.y);
    }
}
