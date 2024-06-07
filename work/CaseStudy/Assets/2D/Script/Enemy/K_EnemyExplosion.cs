using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyExplosion : MonoBehaviour
{
    [Header("�G���U�̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_Scatter;

    [Header("�G������"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    [Header("�q�b�g�X�g�b�v"), SerializeField]
    private float fHitStop = 10;

    //�G���U�p
    private ParticleSystem particle;
    private ParticleSystem.Burst burst;


    void Start()
    {
        //�p�[�e�B�N�����擾
        particle = Eff_Scatter.GetComponent<ParticleSystem>();

        //S_EnemyBall�X�N���v�g�擾
        enemyBall = this.GetComponent<S_EnemyBall>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�I�u�W�F�N�g�擾
        GameObject HitObj = collision.transform.gameObject;

        //�����K_BreakObject�X�N���v�g���t���Ă�����
        if (HitObj.GetComponent<K_BreakObject>())
        {
            //�G�򐔎擾�A�I�u�W�F�N�g�̑ϋv�l�Ɣ�r
            int checkNum = enemyBall.GetStickCount();
            if(HitObj.GetComponent<K_BreakObject>() .GetBreakNum()<= checkNum && GetComponent<S_EnemyBall>().GetisPushing())
            {
                StartCoroutine(HitStop(HitObj));
                Debug.Log("�j��");
                //// �G���U�G�t�F�N�g
                //burst.count = enemyBall.GetStickCount();
                //particle.emission.SetBurst(0, burst);
                //Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                //// �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
                //if (audioclip)
                //{
                //    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                //}

                //Destroy(HitObj);
                //Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //�I�u�W�F�N�g�擾
        GameObject HitObj = collision.transform.gameObject;
        K_BreakObject Break = HitObj.GetComponent<K_BreakObject>();

        //�����K_BreakObject�X�N���v�g���t���Ă�����
        if (Break)
        {
            //�G�򐔎擾�A�I�u�W�F�N�g�̑ϋv�l�Ɣ�r
            int checkNum = enemyBall.GetStickCount();
            if (Break.GetBreakNum() <= checkNum && GetComponent<S_EnemyBall>().GetisPushing())
            {
                StartCoroutine(HitStop(HitObj));
                // �G���U�G�t�F�N�g
                //burst.count = enemyBall.GetStickCount();
                //particle.emission.SetBurst(0, burst);
                //Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                //// �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
                //if (audioclip)
                //{
                //    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                //}

                //Destroy(HitObj);
                //Destroy(gameObject);
            }
        }
    }
    IEnumerator HitStop(GameObject obj)
    {

        Debug.Log("aaaa");
        //�w��̃t���[���҂�
        yield return new WaitForSecondsRealtime(fHitStop / 60);
        Debug.Log("�~�܂鎞");
        // �G���U�G�t�F�N�g
        burst.count = enemyBall.GetStickCount();
        particle.emission.SetBurst(0, burst);
        Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

        // �I�u�W�F�N�g�폜�Ɠ����Ɍ��ʉ���炷����
        if (audioclip)
        {
            AudioSource.PlayClipAtPoint(audioclip, transform.position);
        }

        Destroy(obj);
        Destroy(gameObject);
    }
}