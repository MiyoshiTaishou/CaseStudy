using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動関連の処理
public class M_PlayerMove : MonoBehaviour
{

    [Header("移動速度"), SerializeField]
    private float fMoveSpeed = 10.0f;

    private Rigidbody2D rbPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // キーボード入力を受け取る
        float fHorizontalInput = Input.GetAxis("Horizontal");
        float fVerticalInput = Input.GetAxis("Vertical");

        // 入力に基づいて移動する
        Vector2 vecMoveDirection = new Vector2(fHorizontalInput, fVerticalInput);
        rbPlayer.velocity = vecMoveDirection.normalized * fMoveSpeed;
    }
}
