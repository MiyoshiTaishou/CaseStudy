using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̈ړ��֘A�̏���
public class M_PlayerMove : MonoBehaviour
{

    [Header("�ړ����x"), SerializeField]
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
        // �L�[�{�[�h���͂��󂯎��
        float fHorizontalInput = Input.GetAxis("Horizontal");
        float fVerticalInput = Input.GetAxis("Vertical");

        // ���͂Ɋ�Â��Ĉړ�����
        Vector2 vecMoveDirection = new Vector2(fHorizontalInput, fVerticalInput);
        rbPlayer.velocity = vecMoveDirection.normalized * fMoveSpeed;
    }
}
