using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// カメラを揺らす処理
/// </summary>
public class M_CameraShake : MonoBehaviour
{
    [Header("揺らす強さ"), SerializeField]
    private float m_Power = 1.0f;

    [Header("揺らす時間"), SerializeField]
    private float m_TimeLimit = 1.0f;

   
    private Tilemap tilemap; // タイルマップ

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        tilemap = GetComponent<N_TrackingPlayer>().GetTilemap();
        if(tilemap)
        {
            CalculateBounds();
        }
    }

    /// <summary>
    /// 他のスクリプトで呼ぶと揺れる
    /// </summary>
    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    /// <summary>
    /// 揺れる処理
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShake()
    {
        //揺れる前のカメラの座標
        Vector3 initPos = transform.position;

        //経過時間計測
        float countTime = 0.0f;

        //揺れる時間の間処理する
        while (countTime < m_TimeLimit)
        {
            //カメラの位置をランダムで決める
            float camX = initPos.x + Random.Range(-m_Power, m_Power);
            float camY = initPos.y + Random.Range(-m_Power, m_Power);

            if (tilemap)
            {
                // カメラの半分の高さと幅を計算
                camHalfHeight = Camera.main.orthographicSize;
                camHalfWidth = camHalfHeight * Camera.main.aspect;
                if (camY <= minBounds.y + camHalfHeight)
                {
                    camY = minBounds.y + camHalfHeight;
                }
                Vector3 newPosition = transform.position;
                camX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            }


            transform.position = new Vector3(camX, camY, initPos.z);
            countTime += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;

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
