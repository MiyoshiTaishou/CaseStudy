using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_BreakBlock : MonoBehaviour
{
    // �I�u�W�F�N�g��j��ł���悤�ɂȂ鐔
    [Header("�󂹂�悤�ɂȂ�G�̐�"), SerializeField]
    private int iBreakNum = 3;

    /// <summary>
    /// ���g�̃I�u�W�F�N�g��j�󂷂�I�u�W�F�N�g�����^�O��
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private Animator animator;

    private S_EnemyBall enemyBall;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ŏ������X�N���v�g�擾
        enemyBall = collision.GetComponent<S_EnemyBall>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
        if(collision.collider.tag == sBreakTag)
        {
            // �Ԃ����Ă����G��̐����擾
            int checkNum = (int)enemyBall.GetStickCount();

            // �w��l�ȏ�̉򂪂Ԃ����Ă�����
            if (checkNum >= iBreakNum)
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
