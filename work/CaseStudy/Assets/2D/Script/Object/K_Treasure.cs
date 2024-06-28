using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Treasure : MonoBehaviour
{
    [Header("�����S�擾SE(NULL�ł�������)"), SerializeField]
    private AudioClip audioclip;

    private GameObject CaseObj;

    void Start()
    {
        //�P�[�X�����݂�����R���C�_�[������
        CaseObj = GameObject.Find("Case");
        if(CaseObj)
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            Debug.Log("�P�[�X����");
        }
        else
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
            Debug.Log("�P�[�X�Ȃ�");
        }
    }

    private void Update()
    {
        if (!CaseObj)
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
            Debug.Log("�󔻒�L����");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[���Ԃ����Ă�����
        if (collision.transform.CompareTag("Player"))
        {
            //���炷
            if (audioclip)
            {
                Debug.Log("����񂿂��");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("�Ԃ����Ă���");
            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �v���C���[���Ԃ����Ă�����
        if (collision.transform.CompareTag("Player"))
        {
            //���炷
            if (audioclip)
            {
                Debug.Log("����񂿂��");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("�Ԃ����Ă���");

            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �v���C���[���Ԃ����Ă�����
        if (collision.transform.CompareTag("Player"))
        {
            //���炷
            if (audioclip)
            {
                Debug.Log("����񂿂��");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("�Ԃ����Ă���");

            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // �v���C���[���Ԃ����Ă�����
        if (collision.transform.CompareTag("Player"))
        {
            //���炷
            if (audioclip)
            {
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }
            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }
}
