using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class K_EnemyStan : MonoBehaviour
{
    [Header("何マス以上からの落下でスタンさせるか"), SerializeField]
    private int iStunDistance = 2;

    [Header("タイルマップのオブジェクト名"), SerializeField]
    private string sTilemapObjectName = "Tilemap";

    [Header("スタン時間(秒)"), SerializeField]
    private float fStunTime = 2.0f;

    private  Tilemap tTilemap; // タイルマップ

    private bool IsStunned = false; // スタン状態を示すフラグ

    private Vector3 GroundPos;//直前フレームに接触していた地面の座標

    private float fElapsedTime = 0f;//経過時間

    Vector2 Vec2DefaultSpeed;

    [Header("床タイルの名前(接地判定取得に必要)"), SerializeField]
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
        tTilemap = tilemapObject.GetComponent<Tilemap>();

        //tilemapObjectNameと同名のオブジェクトが見つからなかったらエラーログ出力
        if (tTilemap == null)
        {
            Debug.LogError("Tilemap not found on object: " + sTilemapObjectName);
        }

        //接地中の座標を初期位置に
        GroundPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 敵の位置を取得する
        Vector3Int playerCellPosition = tTilemap.WorldToCell(transform.position);
        // 敵の下にあるタイルの位置を計算する
        Vector3Int floorCellPosition = playerCellPosition + Vector3Int.down;

        // 敵の下にあるタイルを取得
        TileBase tile = tTilemap.GetTile(floorCellPosition);

        //接地中かか判断する
        for (int i = 0; i < sFloorTileNames.Length; i++)
        {
            // タイルが存在し、床タイルであれば
            if (tile != null && tile.name == sFloorTileNames[i])
            {//接地している

                //直前フレームに接触していた座標よりもiStunDistance以上低い値であれば
                if (GroundPos.y - transform.position.y >= iStunDistance - 1)//-1しているのはレイの範囲を考慮したため
                {
                    IsStunned = true;
                    Debug.Log(this.gameObject.name + "スタン");
                    fElapsedTime = 0f;
                }
                //直前フレームに接触していた地面の座標を更新
                GroundPos = transform.position;
            }
        }
        
        //スタン中
        if(IsStunned==true)
        {
            // 経過時間を加算する
            fElapsedTime += Time.deltaTime;

            //スタン解除
            if(fElapsedTime>fStunTime)
            {
                Debug.Log(this.gameObject.name + "スタン解除");
                IsStunned = false;
            }
        }
    }

    //スタン状態ぼげったー
    public bool GetIsStunned()
    {
        return IsStunned;
    }
}
