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
    /// �ǂ���������Ă��邩
    /// </summary>
    private Vector3 vecDir;

    public Vector3 GetDir() { return vecDir; }
    public void SetDir(Vector3 _dir) { vecDir = _dir; }

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
        vecDir = transform.right;
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

        if(fHorizontalInput > 0.0f)
        {
            vecDir = transform.right;
        }
        else if(fHorizontalInput < 0.0f)
        {
            vecDir = -transform.right;
        }
    }
}
