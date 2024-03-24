using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̈ړ��֘A�̏���
public class M_PlayerMove : MonoBehaviour
{
    [Header("�ړ����x"), SerializeField]
    private float fMoveSpeed = 10.0f;

    private Rigidbody2D rbPlayer;

    /// <summary>
    /// �ړ��\��
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

        // �L�[�{�[�h���͂��󂯎��
        float fHorizontalInput = Input.GetAxis("Horizontal");

        // ���͂Ɋ�Â��Ĉړ�����
        Vector2 vecMoveDirection = new Vector2(fHorizontalInput * fMoveSpeed, rbPlayer.velocity.y);
        rbPlayer.velocity = vecMoveDirection;
    }
}
