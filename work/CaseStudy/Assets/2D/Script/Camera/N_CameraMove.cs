using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_CameraMove : MonoBehaviour
{
    // �ړ����x
    [Header("�ړ����x(�P�b�Ɉړ����鋗��)"), SerializeField]
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

        // �L�[�{�[�h���͂��󂯎��
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
