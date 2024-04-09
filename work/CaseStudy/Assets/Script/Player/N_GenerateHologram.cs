using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ホログラムを作る
// 右クリックした場所にプレイヤーのホログラム
// 左クリックしながら選択した範囲内に
// タイルマップのマスごとに当たり判定のない壁、床のホログラム

public class N_GenerateHologram : MonoBehaviour
{
    // 生成するオブジェクト
    [Header("ホロプレイヤー"), SerializeField]
    private GameObject Holo_Player;

    [Header("ホロ壁"), SerializeField]
    private GameObject Holo_Wall;

    [Header("ホロ床"), SerializeField]
    private GameObject Holo_Floor;

    [Header("タイルのサイズ"), SerializeField]
    private float fTileScale = 1.0f;

    [Header("生成方法"), SerializeField]
    private bool bGenerationMethosd = true;

    /// <summary>
    /// マウスが押し込まれた位置
    /// </summary>
    private Vector3 StartPos = Vector3.zero;

    /// <summary>
    /// マウスが押し込まれた位置
    /// </summary>
    private Vector3 EndPos = Vector3.zero;

    /// <summary>
    /// ボタンが離れたか
    /// </summary>
    private bool isRelease = false;

    // Update is called once per frame
    void Update()
    {
        //-----------------------------------------------------------
        // マウス入力取得(0左、1右、2ホイール)
        // 左ボタンが押された瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // スクリーン座標をワールド座標に変換して格納
            StartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        // 左ボタンが離された瞬間
        if (Input.GetMouseButtonUp(0))
        {
            EndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isRelease = true;
        }

        //-----------------------------------------------------------
        // ボタンが離された瞬間だけ入る
        if (isRelease == true)
        {
            // マスごとにホログラムを設置する

            // プレイヤーホログラム
            // 一つのタイル内におさまっていたら
            //if(Mathf.Abs(StartPos.x - EndPos.x) < fTileScale && Mathf.Abs(StartPos.y - EndPos.y) < fTileScale)


            // startの方が小さい値になるようにする
            if(StartPos.x > EndPos.x)
            {
                (StartPos.x, EndPos.x) = (EndPos.x, StartPos.x);
            }
            if (StartPos.y > EndPos.y)
            {
                (StartPos.y, EndPos.y) = (EndPos.y, StartPos.y);
            }

            // 小数点切り捨て
            Vector3Int IntStartPos = new Vector3Int(Mathf.FloorToInt(StartPos.x), Mathf.FloorToInt(StartPos.y), Mathf.FloorToInt(StartPos.z));
            Vector3Int IntEndPos = new Vector3Int(Mathf.FloorToInt(EndPos.x), Mathf.FloorToInt(EndPos.y), Mathf.FloorToInt(EndPos.z));
            // タイルサイズの半分
            float HalfScale = fTileScale / 2.0f;

            int SubVertical = Mathf.Abs(IntStartPos.y - IntEndPos.y); // 縦の差分
            int SubHorizontal = Mathf.Abs(IntStartPos.x - IntEndPos.x); // 横の差分

            if (IntStartPos.x == IntEndPos.x && IntStartPos.y == IntEndPos.y)
            {
                // 生成位置
                Vector3 SpawnPos = new Vector3(IntStartPos.x + HalfScale, IntStartPos.y + HalfScale, 0.0f);

                // プレイヤーホログラム生成
                Instantiate(Holo_Player, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
            }

            // 縦の差分と横の差分を比較、横の方がおおきければホロ床を生成、横と縦の差分が同じなら壁優先になる
            else if(SubHorizontal > SubVertical) // 横 > 縦
            {
                Vector3 SpawnPos;

                // ホロ床生成
                for (int numX = 0; numX < SubHorizontal + 1; numX++)
                {
                    if (bGenerationMethosd)
                    {
                        for (int numY = 0; numY < SubVertical + 1; numY++)
                        {
                            SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale + numY, 0.0f);
                            // 床ホログラム生成
                            Instantiate(Holo_Floor, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    else
                    {
                        SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale, 0.0f);
                        // 床ホログラム生成
                        Instantiate(Holo_Floor, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }
            else
            {
                // ホロ壁生成
                Vector3 SpawnPos;

                // ホロ床生成
                for (int numY = 0; numY < SubVertical + 1; numY++)
                {
                    if (bGenerationMethosd)
                    {
                        for (int numX = 0; numX < SubHorizontal + 1; numX++)
                        {
                            SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale + numY, 0.0f);
                            // 床ホログラム生成
                            Instantiate(Holo_Wall, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    else
                    {
                        SpawnPos = new Vector3(IntStartPos.x + HalfScale, IntStartPos.y + HalfScale + numY, 0.0f);

                        // 床ホログラム生成
                        Instantiate(Holo_Wall, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }

            isRelease = false;
        }

    }


}
