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

    [Header("�ϋv�l��\�����邩?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    private TextMeshProUGUI TextEndurance;

    [Header("�j�󎞂̃G�t�F�N�g"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("������"), SerializeField]
    private AudioClip audioclip;

    private bool isQuitting;

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
               TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z); // �K�؂ȃI�t�Z�b�g����������
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

    private void OnDestroy()
    {
        if(!isQuitting)
        {
            //�����G�t�F�N�g�Đ�
            if(Eff_Explosion)
            {
                Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
            }
            if (audioclip)
            {
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }
        }
    }
}