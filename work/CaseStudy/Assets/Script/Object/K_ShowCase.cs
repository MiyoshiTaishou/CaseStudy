using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ShowCase : MonoBehaviour
{
    [Header("�P�[�X�ϋv�l"), SerializeField]
    private uint nHardness = 1;

    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private GameObject CaseObj;
    private BoxCollider2D CaseCollider;
    private GameObject TreasureObj;
    private BoxCollider2D TreasureCollider;
    private bool IsBreaked;
    private S_EnemyBall enemyBall;

    void Start()
    {
        CaseObj = transform.Find("Case").gameObject;
        CaseCollider = CaseObj.GetComponent<BoxCollider2D>();
        TreasureObj = transform.Find("Treasure").gameObject;
        TreasureCollider = TreasureObj.GetComponent<BoxCollider2D>();
        IsBreaked = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�P�[�X���󂳂�Ă��Ȃ�������
        if(IsBreaked==false)
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
                    Destroy(CaseObj);
                    Destroy(collision.gameObject);
                    IsBreaked = true;
                }
            }
        }
        else
        {
            // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
            if (collision.collider.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //�P�[�X���󂳂�Ă��Ȃ�������
        if (IsBreaked == false)
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
                    Destroy(CaseObj);
                    Destroy(collision.gameObject);
                    IsBreaked = true;
                }
            }
        }
        else
        {
            // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
            if (collision.collider.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}