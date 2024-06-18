using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class N_PostProcess : MonoBehaviour
{
    // �J�����ɃG�t�F�N�g������

    [Header("�x���G�t�F�N�g�ő�l"), SerializeField]
    private float MaxVignette = 0.5f;

    [Header("�x���G�t�F�N�g�ŏ��l"), SerializeField]
    private float MinVignette = 0.0f;

    [Header("���b�����čő�l�܂ōs����"), SerializeField]
    private float VignetteTime = 1.0f;

    private bool AddVignette = true;

    private Volume postVolume;

    private Vignette vignette;

    private int SearchManagerNum = 0;

    private bool isInit = false;

    public int GetSearchNum()
    {
        return SearchManagerNum;
    }

    public void SetSearchNum(int _num)
    {
        SearchManagerNum = _num;
    }

    private void Initialize()
    {
        postVolume = GetComponent<Volume>();

        // vignette���擾
        postVolume.profile.TryGet(out vignette);

        vignette.intensity.value = 0.0f;

        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit)
        {
            Initialize();
        }

        Vignette();
    }

    public void AddSerachNum()
    {
        SearchManagerNum++;
    }

    public void SubSerachNum()
    {
        SearchManagerNum--;
    }

    private void Vignette()
    {
        if (SearchManagerNum > 0)
        {
            // �ǐՏ�ԂȂ̂ŉ�ʂɃG�t�F�N�g��������
            if (AddVignette)
            {
                // ���X�ɉ�ʂɃG�t�F�N�g�������Ă���
                vignette.intensity.value += MaxVignette / VignetteTime * Time.deltaTime;

                if (vignette.intensity.value >= MaxVignette)
                {
                    AddVignette = false;
                }
            }
            else
            {
                // ���X�ɃG�t�F�N�g���̂��Ă���
                vignette.intensity.value -= MaxVignette / VignetteTime * Time.deltaTime;

                if (vignette.intensity.value <= MinVignette)
                {
                    AddVignette = true;
                }
            }
        }
        else
        {
            // ��ǐՏ�ԂȂ̂ŏ�����
            SearchManagerNum = 0;

            // ���X��vignette��intensity�������Ă���
            if (vignette.intensity.value >= 0.0f)
            {
                vignette.intensity.value -= 0.5f * Time.deltaTime;
                if (vignette.intensity.value < 0.0f)
                {
                    vignette.intensity.value = 0.0f;

                }
            }
        }
    }
}
