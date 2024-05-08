using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_SwitchBGM : MonoBehaviour
{
    [Header("�ʏ�Đ������"),SerializeField]
    private AudioSource bgmSource1; // �I�[�f�B�I�\�[�X1

    [Header("�������Đ������"),SerializeField]
    private AudioSource bgmSource2; // �I�[�f�B�I�\�[�X2

    [Header("�Ȃ̃~�b�N�X"), SerializeField,Range(0,1)]
    private float fRate = 0.0f; // �t�F�[�h����                                 

    [Header("�Ȃ̐؂�ւ����x"), SerializeField]
    private float fSpeed = 1.0f; // �t�F�[�h����
                                 // 
    [Header("����"), SerializeField,]
    private float fVolume = 0.1f;

    /// <summary>
    /// ���Ԍv��
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// BGM1���Đ������ǂ���
    /// </summary>
    private bool isPlayingBGM1 = true;

    private void Start()
    {
        bgmSource1.Play();
        fRate = 1.0f;
    }

    private void Update()
    {
        //if(bgmSource1.volume <= 0.0f)
        //{
        //    bgmSource1.Stop();
        //}       

        //if (bgmSource2.volume <= 0.0f)
        //{
        //    bgmSource2.Stop();
        //}       

        //BGM�̃~�b�N�X
        bgmSource1.volume = 1 * fVolume;
        bgmSource2.volume = fRate * fVolume;
    }

    /// <summary>
    /// ����true�ً͋},false�͒ʏ�
    /// </summary>
    /// <param name="_bgm"></param>
    public void ChangeBGM(bool _bgm)
    {
        if(_bgm)
        {
            //fRate = 1.0f;
            if(!bgmSource2.isPlaying)
            {
                bgmSource2.Play();
                bgmSource1.Stop();
            }           
        }
        else
        {
            if(!bgmSource1.isPlaying)
            {
                //fRate = 0.0f;
                bgmSource1.Play();
                bgmSource2.Stop();
            }           
        }        
    }

    /// <summary>
    /// �Ȃ�؂�ւ��郁�\�b�h
    /// </summary>
    public void SwitchBGM()
    {
        StartCoroutine(Crossfade());
    }

    // �N���X�t�F�[�h����
    IEnumerator Crossfade()
    {
        // �t�F�[�h�A�E�g
        if (isPlayingBGM1)
        {
            float currentTime = 0.0f;
            while (currentTime < fSpeed)
            {
                currentTime += Time.deltaTime;
                fRate = Mathf.Lerp(0, 1, currentTime / fSpeed);
                yield return null;
            }
        }
        else // �t�F�[�h�C��
        {
            float currentTime = 0.0f;           
            while (currentTime < fSpeed)
            {
                currentTime += Time.deltaTime;
                fRate = Mathf.Lerp(1, 0, currentTime / fSpeed);
                yield return null;
            }
        }

        // �t���O�𔽓]������
        isPlayingBGM1 = !isPlayingBGM1;
    }
}