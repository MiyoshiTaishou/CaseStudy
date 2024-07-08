using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class K_CamMoveRStick : MonoBehaviour
{
    [Header("Rスティックで動けるか"), SerializeField]
    private bool IsMovableRStick = true;

    private float fSpeed = 30.0f;

    private bool IsMovingRStick = false;

    public bool GetIsMovingRStick() { return IsMovingRStick; }

    private Tilemap tilemap; // タイルマップ

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    //ニュートラル状態になってからの経過時間
    private float NutoralDeltatime;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<N_TrackingPlayer>())
        {
            tilemap = GetComponent<N_TrackingPlayer>().GetTilemap();
        }
        else if (GetComponent<K_5_4Camera>())
        {
            tilemap = GetComponent<K_5_4Camera>().GetTilemap();
        }
        IsMovingRStick = false;
        if (tilemap)
        {
            CalculateBounds();
        }
        NutoralDeltatime = 0.0f;
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMovableRStick)
        {
            var gamepad = Gamepad.current;
            Vector2 rightStick = gamepad.rightStick.ReadValue();
            Vector3 MoveVec = new Vector3(rightStick.x, rightStick.y, 0.0f);

            Vector3 TrackPlayerPos = GetComponent<N_TrackingPlayer>().GetTrackngCamPos();
            if (rightStick.magnitude>0.3f)
            {
                GetComponent<N_TrackingPlayer>().SetIsTrackingPlayer(false);
                transform.position = transform.position + MoveVec * Time.deltaTime * fSpeed;
                Vector3 newPosition = transform.position;
                float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
                float clampedY = Mathf.Clamp(newPosition.y, minBounds.y + camHalfHeight, maxBounds.y + camHalfHeight);
                transform.position = new Vector3(clampedX, clampedY, transform.position.z);
                IsMovingRStick = true;
                NutoralDeltatime = 0.0f;
            }
            else
            {

                NutoralDeltatime += Time.deltaTime;
                if(NutoralDeltatime>0.1f)
                {
                    Vector3 MoveDir = TrackPlayerPos - this.transform.position;
                    this.transform.position += MoveDir * 0.1f;
                    if (MoveDir.magnitude<0.5f)
                    {
                        GetComponent<N_TrackingPlayer>().SetIsTrackingPlayer(true);
                        IsMovingRStick = false;
                    }
                }
            }
        }
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
