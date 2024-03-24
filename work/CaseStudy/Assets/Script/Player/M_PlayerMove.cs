using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動関連の処理
public class M_PlayerMove : MonoBehaviour
{
    [Header("移動速度"), SerializeField]
    private float fMoveSpeed = 10.0f;

    private Rigidbody2D rbPlayer;

    /// <summary>
    /// 移動可能か
    /// </summary>
    private bool isMove = true;

    public bool GetIsMove() { return isMove; }
    public void SetIsMove(bool _move) { isMove = _move; }

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {        
        if(!isMove)
        {
            return;
        }

        // キーボード入力を受け取る
        float fHorizontalInput = Input.GetAxis("Horizontal");

        // 入力に基づいて移動する
        Vector2 vecMoveDirection = new Vector2(fHorizontalInput * fMoveSpeed, rbPlayer.velocity.y);
        rbPlayer.velocity = vecMoveDirection;
    }
}
