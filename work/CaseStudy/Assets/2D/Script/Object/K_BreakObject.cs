using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class K_BreakObject : MonoBehaviour
{
    [Header("�󂷂̂ɕK�v�ȓG�̐�"), SerializeField]
    private uint iBreakNum = 3;

    [Header("�e�L�X�g�T�C�Y"), SerializeField]
    private int iTexSize = 100;

    [Header("�e�L�X�g�̗h��̃X�s�[�h"), SerializeField]
    private float UpDownSpeed = 1.0f;

    [Header("�e�L�X�g�̗h�ꕝ"), SerializeField]
    private float fShakingRange = 1.0f;

    [Header("�ϋv�l��\�����邩?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    private TextMeshProUGUI TextEndurance;

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
            TextEndurance = gcd.GetComponent<TextMeshProUGUI>();
            TextEndurance.text = iBreakNum.ToString();
            TextEndurance.fontSize = iTexSize;
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

    }

    private void Update()
    {
        if (this.gameObject == null)
        {
            TextEndurance.text = null;
        }
        else
        {
           if (IsDisplayBreakNum == true)
           {
                // �I�u�W�F�N�g�ʒu�Ƀe�L�X�g�v�f��Ǐ]������
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y + Mathf.PingPong(Time.time * UpDownSpeed, fShakingRange), screenPos.z); // �K�؂ȃI�t�Z�b�g����������
            }
        }
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
