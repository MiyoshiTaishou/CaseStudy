using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//ステージのタイルマップの最小、最大座標を基に背景を変更するよ

public class K_BackGround : MonoBehaviour
{
    [Header("タイルマップ"), SerializeField]
    private Tilemap tilemap; // タイルマップ

    GameObject CanvasObj;

    
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
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;

        if (tilemap)
        {
            // タイルマップの範囲を計算
            CalculateBounds();
        }
        //マップの中心座標計算
        Vector3 pos;
        pos.x = (minBounds.x + maxBounds.x) / 2.0f;
        pos.y = (minBounds.y + (maxBounds.y + camHalfHeight)) / 2.0f;
        pos.z = gameObject.transform.position.z;
        gameObject.transform.position = pos;

        //サイズ設定
        RectTransform canvasRectTransform = CanvasObj.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        float originalWidth = canvasRectTransform.rect.width;
        float width = (maxBounds.x + camHalfWidth) - (minBounds.x - camHalfWidth);
        float ReductionRatio = width / originalWidth;
        this.gameObject.transform.localScale=new Vector3(ReductionRatio, ReductionRatio, ReductionRatio);
    }

    // Update is called once per frame
    void Update()
    {
        
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
