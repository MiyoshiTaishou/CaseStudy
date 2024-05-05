using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ShowCase : MonoBehaviour
{
    [Header("�P�[�X�ϋv�l"), SerializeField]
    private uint nHardness = 1;

    [Header("BreakObjectTag"), SerializeField]
    private string sBreakTag = "EnemyBall";

    private S_EnemyBall enemyBall;


    private void OnCollisionEnter2D(Collision2D collision)
    {
         // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
         if (collision.collider.tag == sBreakTag)
         {
             enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

             // �Ԃ����Ă����G��̐����擾
             int checkNum = (int)enemyBall.GetStickCount();

             // �w��l�ȏ�̉򂪂Ԃ����Ă�����
             if (checkNum >= nHardness)
             {
                 Destroy(gameObject);
                 Destroy(collision.gameObject);
             }
         }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
         // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
         if (collision.collider.tag == sBreakTag)
         {
             enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

             // �Ԃ����Ă����G��̐����擾
             int checkNum = (int)enemyBall.GetStickCount();

             // �w��l�ȏ�̉򂪂Ԃ����Ă�����
             if (checkNum >= nHardness)
             {
                 Destroy(gameObject);
                 Destroy(collision.gameObject);
             }
         }
    }
}