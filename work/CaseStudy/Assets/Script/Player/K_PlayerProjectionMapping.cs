using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//ホログラムを行うためのコードです。
//マウスでD&Dした方向に応じて当たり判定のない壁または床が生成されます。また、右クリックするとプレイヤーのホログラムが生成されます。

public class K_PlayerProjectionMapping : MonoBehaviour
{
    [Header("プロジェクションマッピング用タイルマップ"), SerializeField]
    private Tilemap ProjectionMappingTileMap;

    [Header("壁タイル"), SerializeField]
    private TileBase wall;

    [Header("床タイル"), SerializeField]
    private TileBase floor;

    [Header("プロジェクションマッピング解除キー"), SerializeField]
    private KeyCode ResetKey;

    [Header("ホログラムの寿命（秒）"), SerializeField]
    private float fTileLifetime = 5f;

    [Header("プレイヤーホログラムのPrefab"), SerializeField]
    private GameObject SpritePrefab;


    private Vector3Int startTilemapPos; // マウスが押され始めた位置

    private Dictionary<Vector3Int, float> activeTiles = new Dictionary<Vector3Int, float>(); // 描画中のタイルとその寿命

    void Start()
    {
        ProjectionMappingTileMap.ClearAllTiles();
    }

    void Update()
    {
        // タイルの寿命を減少させる
        foreach (var key in new List<Vector3Int>(activeTiles.Keys))
        {
            activeTiles[key] -= Time.deltaTime;
            if (activeTiles[key] <= 0)
            {
                ProjectionMappingTileMap.SetTile(key, null); // タイルを消去
                activeTiles.Remove(key); // activeTilesから削除
            }
        }

        if (Input.GetMouseButtonDown(0)) // マウスの左ボタンが押された瞬間を検出
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウスの位置をワールド座標に変換
            startTilemapPos = ProjectionMappingTileMap.WorldToCell(mouseWorldPos); // ワールド座標からタイルマップの座標に変換
        }
        else if (Input.GetMouseButtonUp(0)) // マウスの左ボタンが離された瞬間を検出
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウスの位置をワールド座標に変換
            Vector3Int endTilemapPos = ProjectionMappingTileMap.WorldToCell(mouseWorldPos); // ワールド座標からタイルマップの座標に変換

            // 押され始めた位置から離された位置までのタイルを描画
            DrawTiles(ProjectionMappingTileMap,startTilemapPos, endTilemapPos); 
        }

        if (Input.GetKeyDown(ResetKey)) // 解除キーが入力されたら
        {//全部消す
            ProjectionMappingTileMap.ClearAllTiles();
        }

        // マウスの左クリックがされた場合
        if (Input.GetMouseButtonDown(1))
        {
            // クリックした位置を取得
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // Z軸は使わないので、0に固定

            // スプライトを表示する
            GameObject newSprite = Instantiate(SpritePrefab, clickPosition, Quaternion.identity);

            // スプライトを一定時間後に破棄する
            StartCoroutine(DestroySpriteAfterDelay(newSprite, fTileLifetime));
        }
    }


    //実際に壁/床を生成する関数
    void DrawTiles(Tilemap tilemap,Vector3Int start, Vector3Int end)
    {
        //必要な色々な数値を求めるよ
        int deltaX = Mathf.Abs(end.x - start.x);
        int deltaY = Mathf.Abs(end.y - start.y);
        int signX = start.x < end.x ? 1 : -1;
        int signY = start.y < end.y ? 1 : -1;
        int error = deltaX - deltaY;

        int x = start.x;
        int y = start.y;

        while (true)
        {
            //開始位置と終了位置を比較、ΔXとΔYどちらが大きいか比較
            if (deltaX < deltaY)
            {//Y成分のほうが大きかったら、壁を生成
                //タイル描画
                tilemap.SetTile(new Vector3Int(x, y, 0), wall);

                //寿命設定
                activeTiles[new Vector3Int(x, y, 0)] = fTileLifetime;

                //最後まで書いたら脱出
                if (y == end.y)
                    break;

                //次に描画するタイルの座標を求める
                int error2 = error * 2;
                if (error2 < deltaX)
                {
                    error += deltaX;
                    y += signY;
                }
            }
            else
            {//X成分のほうが大きかったら、床を生成
                //タイル描画
                tilemap.SetTile(new Vector3Int(x, y, 0), floor);

                //寿命設定
                activeTiles[new Vector3Int(x, y, 0)] = fTileLifetime;

                //最後まで書いたら脱出
                if (x == end.x)
                    break;

                //次に描画するタイルの座標を求める
                int error2 = error * 2;
                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    x += signX;
                }
            }
        }
    }

    IEnumerator DestroySpriteAfterDelay(GameObject spriteObject, float delay)
    {
        // 指定した時間を待つ
        yield return new WaitForSeconds(delay);

        // スプライトを破棄する
        Destroy(spriteObject);
    }
}


