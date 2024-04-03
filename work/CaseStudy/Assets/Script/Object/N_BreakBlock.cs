using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_BreakBlock : MonoBehaviour
{
    // �I�u�W�F�N�g��j��ł���悤�ɂȂ鐔
    [Header("�󂹂�悤�ɂȂ�G�̐�"), SerializeField]
    private int iBreakNum = 3;

    [Header("�e�X�g�p�@�G��̐�"), SerializeField]
    private int iTestNum = 1;

    /// <summary>
    /// ���g�̃I�u�W�F�N�g��j�󂷂�I�u�W�F�N�g�����^�O��
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
        if(collision.collider.tag == sBreakTag)
        {
            // �w��l�ȏ�̉򂪂Ԃ����Ă�����
            if(iTestNum >= iBreakNum)
            {
                animator.SetBool("break", true);
            }
        }
    }

    void SetDestroy()
    {
        animator.SetBool("destroy", true);

        // ���g���폜
        Destroy(this.gameObject);
    }
}
