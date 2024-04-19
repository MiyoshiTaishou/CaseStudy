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

    [Header("�j�󎞂̃G�t�F�N�g"), SerializeField]
    private GameObject Effect;

    [Header("AudioClip"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    private Transform trans_Block;

    // Start is called before the first frame update
    void Start()
    {
        trans_Block = gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ŏ������X�N���v�g�擾
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �w�肵���^�O���������I�u�W�F�N�g���Ԃ����Ă�����
        if(collision.collider.tag == sBreakTag)
        {
            enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

            // �Ԃ����Ă����G��̐����擾
            int checkNum = (int)enemyBall.GetStickCount();

            // �w��l�ȏ�̉򂪂Ԃ����Ă�����
            if (checkNum >= iBreakNum)
            {
                Vector3 pos = trans_Block.position;

                Instantiate(Effect, pos, Quaternion.identity);

                // �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
                AudioSource.PlayClipAtPoint(audioclip, pos);

                Destroy(gameObject);
            }
        }
    }
}
