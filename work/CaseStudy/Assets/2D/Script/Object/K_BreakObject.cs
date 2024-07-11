using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class K_BreakObject : MonoBehaviour
{
    [Header("�󂷂̂ɕK�v�ȓG�̐�"), SerializeField]
    private uint iBreakNum = 3;

    [Header("�e�L�X�g�T�C�Y"), SerializeField]
    private int iTexSize = 100;

    [Header("�e�L�X�g�̗h��̃X�s�[�h"), SerializeField]
    private float UpDownSpeed = 1.0f;

    [Header("�e�L�X�g�̗h�ꕝ"), SerializeField]
    private float fShakingRange = 0.1f;

    [Header("�ϋv�l��\�����邩?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    [Header("�ϋv�l�\���̍��W"), SerializeField]
    private Vector2Int TextOffset;

    [Header("�j�󎞂̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("�j�Ђ̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_BrokenPiece;

    [Header("�j�А�����(����30)"), SerializeField]
    private int GenerateNum = 30;

    [Header("������"), SerializeField]
    private AudioClip audioclip;

    private ParticleSystem.Burst burst;

    private bool isQuitting;

    private GameObject DisplayNumObj;
    Vector3 InitTransForm;

    /// <summary>
    /// �O�D���ĒǋL�J�����I�u�W�F�N�g
    /// </summary>
    GameObject camera;

    private void Start()
    {
        if(IsDisplayBreakNum == true)
        {
            //�v���n�u���̉�
            GameObject cd = transform.GetChild(0).gameObject; ;
            GameObject gcd = cd.GetComponent<Transform>().transform.GetChild(0).gameObject;
        }

        if (Eff_BrokenPiece) {
            burst.count = GenerateNum;
            Eff_BrokenPiece.GetComponent<ParticleSystem>().emission.SetBurst(0,burst);
        }

        //�O�D���ĒǋL�J�����T��
        camera = GameObject.FindWithTag("MainCamera");

        if (!camera)
        {
            Debug.Log("�J������������܂���");
        }
        
        //�����\���I�u�W�F�N�g�T��
        for(int i=0;i< transform.childCount;i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if(obj.name== "K_Suuji")
            {
                DisplayNumObj = obj;
            }
        }

        if(!DisplayNumObj)
        {
            Debug.Log("���̃I�u�W�F�N�g��������");
        }
        else
        {
            DisplayNumObj.GetComponent<K_DisplaySuuji>().SetNum((int)iBreakNum);
            InitTransForm = DisplayNumObj.gameObject.transform.position;
        }
    }

    private void Update()
    {
        //if (this.gameObject == null)
        //{
        //}
        //else
        //{
        //   if (IsDisplayBreakNum == true)
        //   {
        //        // �I�u�W�F�N�g�ʒu�Ƀe�L�X�g�v�f��Ǐ]������
        //        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //        TextEndurance.transform.position = new Vector3(screenPos.x + TextOffset.x, screenPos.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange) + TextOffset.y, screenPos.z); // �K�؂ȃI�t�Z�b�g����������
        //    }
        //}
        DisplayNumObj.transform.position= new Vector3(InitTransForm.x, InitTransForm.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange), InitTransForm.z);
    }

    public uint GetBreakNum()
    {
        return iBreakNum;
    }

    void OnApplicationQuit()
    {

        isQuitting = true;

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�I�u�W�F�N�g�擾
        GameObject HitObj = collision.transform.gameObject;
        //�I�u�W�F�N�g����S_EnemyBall�擾
        S_EnemyBall enemyBall = HitObj.GetComponent<S_EnemyBall>();

        //�����S_EnemyBall�X�N���v�g���t���Ă�����
        if (enemyBall)
        {
            //�G�򐔎擾�A�I�u�W�F�N�g�̑ϋv�l�Ɣ�r
            int checkNum = enemyBall.GetStickCount();
            if (this.GetBreakNum() <= checkNum && enemyBall.GetisPushing())
            {
                //�����G�t�F�N�g�Đ�
                if (Eff_Explosion)
                {
                    Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    // �V������̃Q�[���I�u�W�F�N�g���쐬
                    Debug.Log("�Đ��I�u�W�F�N�g���A�����[�b!");
                    GameObject newGameObject = new GameObject("BreakObbbj");
                    // �ʒu��ݒ�
                    newGameObject.transform.position = transform.position;
                    newGameObject.AddComponent<AudioSource>();
                    newGameObject.GetComponent<AudioSource>().PlayOneShot(audioclip);
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
                // �j�ЃG�t�F�N�g����
                if (Eff_BrokenPiece)
                {
                    Instantiate(Eff_BrokenPiece, transform.position, Quaternion.identity);
                }
                if (camera)
                {
                    camera.GetComponent<M_CameraShake>().Shake();
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //�I�u�W�F�N�g�擾
        GameObject HitObj = collision.transform.gameObject;
        //�I�u�W�F�N�g����S_EnemyBall�擾
        S_EnemyBall enemyBall = HitObj.GetComponent<S_EnemyBall>();

        //�����S_EnemyBall�X�N���v�g���t���Ă�����
        if (enemyBall)
        {
            //�G�򐔎擾�A�I�u�W�F�N�g�̑ϋv�l�Ɣ�r
            int checkNum = enemyBall.GetStickCount();
            if (this.GetBreakNum() <= checkNum && enemyBall.GetisPushing())
            {
                //�����G�t�F�N�g�Đ�
                if (Eff_Explosion)
                {
                    Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    // �V������̃Q�[���I�u�W�F�N�g���쐬
                    Debug.Log("�Đ��I�u�W�F�N�g���A�����[�b!");
                    GameObject newGameObject = new GameObject("BreakObbbj");
                    // �ʒu��ݒ�
                    newGameObject.transform.position = transform.position;
                    newGameObject.AddComponent<AudioSource>();
                    newGameObject.GetComponent<AudioSource>().PlayOneShot(audioclip);
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
                // �j�ЃG�t�F�N�g����
                if (Eff_BrokenPiece)
                {
                    Instantiate(Eff_BrokenPiece, transform.position, Quaternion.identity);
                }
                if (camera)
                {
                    camera.GetComponent<M_CameraShake>().Shake();
                }
                Destroy(gameObject);
            }
        }
    }
}
