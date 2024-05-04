using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class K_PlayerCreateFallacies : MonoBehaviour
{
    //レイの長さ(仮置き)
    private const float fRayLength = 0.5f;
    //プレイヤー半径(仮置き)
    private const float fPlayerRad = 1.0f;
    //プレイヤーの向き(-1は左、1は右を向いている)
    private int iPlayreDirection;

    // タイルマップオブジェクト
    private Tilemap Tilemap;

    [Header("タイルマップのオブジェクト名"), SerializeField]
    public string sTilemapObjectName = "Tilemap";

    [Header("床タイルの名前(タイル名を指定しないと消えない)"), SerializeField]
    public string[] sFloorTileNames =
    {
        "douro_0",
        "douro_1",
        "douro_2",
        "douro_3",
        "douro_4",
    };

    void Start()
    {
        // シーン内のタイルマップオブジェクトを取得
        GameObject tilemapObject = GameObject.Find(sTilemapObjectName);

        // タイルマップオブジェクトからTilemapコンポーネントを取得
        Tilemap = tilemapObject.GetComponent<Tilemap>();

        //tilemapObjectNameと同名のオブジェクトが見つからなかったらエラーログ出力
        if (Tilemap == null)
        {
            Debug.LogError("Tilemap not found on object: " + sTilemapObjectName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // タイルマップが見つからない場合は処理を中止
        if (Tilemap == null)
        {
            return;
        }

        //プレイヤーの向きを決める
        float fHorizontalInput = Input.GetAxis("Horizontal");
        if (fHorizontalInput < 0.0f)
        {
            iPlayreDirection = -1;
        }
        if (0.0f < fHorizontalInput)
        {
            iPlayreDirection = 1;
        }

        //Fが押されたら
        if (Input.GetKeyDown(KeyCode.F))
        {
            // プレイヤーの位置を取得する
            Vector3Int playerCellPosition = Tilemap.WorldToCell(transform.position);
            // プレイヤーの下にあるタイルの位置を計算する
            Vector3Int floorCellPosition = playerCellPosition + Vector3Int.down;

            // プレイヤーの下にあるタイルを取得
            TileBase tile = Tilemap.GetTile(floorCellPosition);

            //プレイヤーの下にあるタイルが床タイルか判断する
            for (int i = 0; i < sFloorTileNames.Length; i++)
            {
                // タイルが存在し、床タイルであれば
                if (tile != null && tile.name == sFloorTileNames[i])
                {
                    // プレイヤーの下にある床の隣の床の位置を計算する
                    Vector3Int neighborFloorCellPosition = floorCellPosition + new Vector3Int(iPlayreDirection, 0, 0);

                    // プレイヤーの下にある床の隣の床を取得
                    tile = Tilemap.GetTile(neighborFloorCellPosition);

                    //実際に床を消す処理
                    for (int j = 0; j < sFloorTileNames.Length; j++)
                    {
                        // タイルが存在し、指定した名前のタイルであるかを確認して床を削除する
                        if (tile != null && tile.name == sFloorTileNames[i])
                        {
                            Tilemap.SetTile(neighborFloorCellPosition, null);
                        }
                    }
                }
            }
        }
    }
}