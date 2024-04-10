using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_CameraMove : MonoBehaviour
{
    // 移動速度
    [Header("移動速度(１秒に移動する距離)"), SerializeField]
    private float fMoveSpeed = 3.0f;

    private Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         Vector3 MoveVec = Vector3.zero;

        // キーボード入力を受け取る
        if (Input.GetKey(KeyCode.W))
        {
            MoveVec.y += fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveVec.y += -fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveVec.x += -fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveVec.x += fMoveSpeed * Time.deltaTime;
        }

        transform.Translate(MoveVec, Space.World);
    }
}
