using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//ステージのタイルマップの最小、最大座標を基に背景を変更するよ

public class K_BackGround : MonoBehaviour
{
    [Header("タイルマップ"), SerializeField]
    private Tilemap tilemap; // タイルマップ

    [Header("カメラの描画範囲"), SerializeField]
    private float fViewSize = 7;

    //キャンバス
    GameObject CanvasObj;

    //使う変数共
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        //キャンバス取得
        CanvasObj = transform.GetChild(0).gameObject;

        // カメラの半分の高さと幅を計算
        camHalfHeight = fViewSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
        //Debug.Log(Camera.main.orthographicSize * Camera.main.aspect);
        if (tilemap)
        {
            // タイルマップの範囲を計算
            CalculateBounds();
        }

        //サイズ設定
        RectTransform canvasRectTransform = CanvasObj.GetComponentInParent<Canvas>().GetComponent<RectTransform>(); //キャンバス情報取得
        float CanvasWidth = canvasRectTransform.rect.width;  //キャンバス幅
        float OnScreenWidth = Mathf.Abs((maxBounds.x + camHalfWidth) - (minBounds.x- camHalfWidth)); //スクリーン上で設定したい幅
        float ReductionRatio = OnScreenWidth / CanvasWidth;   //上二つの割合を求める
        this.gameObject.transform.localScale=new Vector3(ReductionRatio, ReductionRatio, ReductionRatio);   //サイズ変更

        //マップの中心座標計算
        Vector3 pos;
        pos.x = (minBounds.x + maxBounds.x) / 2.0f; //右端と左端の真ん中
        float CanvasHalfHeighth = canvasRectTransform.rect.height * ReductionRatio / 2.0f;  //キャンバス高さ
        pos.y = CanvasHalfHeighth+ minBounds.y;
        pos.z = gameObject.transform.position.z;
        gameObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //タイルマップの最大座標、最小座標を求める
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
        minBounds = new Vector2(minWorld.x, minWorld.y);
        maxBounds = new Vector2(maxWorld.x, maxWorld.y);
    }
}
