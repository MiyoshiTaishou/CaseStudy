using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�l�~�[�̃A�j���[�V�����Đ�
//�h���b�v�ȊO����邩��
public class M_DropEnemy : MonoBehaviour
{
    private Animator m_Animator;

    private bool isGround = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.SetBool("isDrop", false);
    }

    private void Update()
    {
        //���C���΂��ĉ��ɉ����Ȃ����
        // ���g�̉�������Ray���Ǝ˂��Ēn�ʂ��`�F�b�N
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
       
        if (hit.collider != null)
        {
            isGround = true; // �n�ʂɐڒn���Ă���
        }
        else
        {
            isGround = false; // �n�ʂɐڒn���Ă��Ȃ�
        }

        Debug.Log(isGround);

        // �n�ʂɐڒn���Ă��Ȃ���΃h���b�v��Ԃɐݒ�
        m_Animator.SetBool("isDrop", !isGround);
    }
}
