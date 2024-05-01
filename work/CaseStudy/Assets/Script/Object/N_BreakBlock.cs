using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//�ϋv�l�\���p(2024/04/25�@�ؑ��ǉ�)

public class N_BreakBlock : MonoBehaviour
{
    // �I�u�W�F�N�g��j��ł���悤�ɂȂ鐔
    [Header("�󂹂�悤�ɂȂ�G�̐�"), SerializeField]
    private int iBreakNum = 3;

    /// <summary>
    /// ���g�̃I�u�W�F�N�g��j�󂷂�I�u�W�F�N�g�����^�O��
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "EnemyBall";

    [Header("�j�󎞂̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("�G���U�̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_Scatter;

    [Header("AudioClip"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    private Transform trans_Block;

    //�ϋv�l�\���p(2024/04/25�@�ؑ��ǉ�)
    private TextMeshProUGUI TextEndurance;

    // Start is called before the first frame update
    void Start()
    {
        trans_Block = gameObject.transform;

        //�ϋv�l�\���p(2024/04/25�@�ؑ��ǉ�)
        GameObject cd = transform.GetChild(0).gameObject; ;
        GameObject gcd = cd.GetComponent<Transform>().transform.GetChild(0).gameObject;
        TextEndurance = gcd.GetComponent<TextMeshProUGUI>();
        TextEndurance.text = iBreakNum.ToString();
        Debug.Log(TextEndurance.text);
    }

    //�ϋv�l�\���p(2024/04/25�@�ؑ��ǉ�)
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z); // �K�؂ȃI�t�Z�b�g����������
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

                // �u���b�N����
                Instantiate(Eff_Explosion, pos, Quaternion.identity);

                // �G���U
                pos = collision.transform.position;
                Instantiate(Eff_Scatter, pos, Quaternion.identity);

                // �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
                AudioSource.PlayClipAtPoint(audioclip, pos);

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
            if (checkNum >= iBreakNum)
            {
                Vector3 pos = trans_Block.position;

                // �u���b�N����
                Instantiate(Eff_Explosion, pos, Quaternion.identity);

                // �G���U
                pos = collision.transform.position;
                Instantiate(Eff_Scatter, pos, Quaternion.identity);

                // �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
                AudioSource.PlayClipAtPoint(audioclip, pos);

                Destroy(gameObject);
                Destroy(collision.gameObject);

            }
        }
    }
}
