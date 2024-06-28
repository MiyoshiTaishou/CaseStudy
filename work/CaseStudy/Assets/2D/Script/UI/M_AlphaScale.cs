using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class M_AlphaScale : MonoBehaviour
{
    [Header("�ҋ@����"),SerializeField] 
    float m_Scale = 1f;

    [Header("���ߎ���"), SerializeField]
    private float fAlphaTime = 1.0f; // ���߂ɂ����鎞�Ԃ𒲐����܂�

    [Header("�C�[�W���O�֐�"), SerializeField]
    M_Easing.Ease ease;

    private float fTime = 0.0f;

    private Image image;

    private bool isStart = false;
    private bool isSound = false;

    [Header("�n���R�̉��炷�p"), SerializeField]
    private N_PlaySound sound;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

        if(!isStart)
        {
            StartCoroutine(Wait());
            return;
        }

        //Debug.Log(fTime);
        if (fTime > fAlphaTime)
        {
            fTime = fAlphaTime;           
        }
        else
        {
            fTime += Time.deltaTime;
        }
        Easing();
    }

    private void Easing()
    {
        float t = fTime / fAlphaTime;
        float easedValue = M_Easing.GetEasingMethod(ease)(t);

        //Debug.Log(easedValue + "���ߓx");

        image.color = new Color(image.color.r, image.color.g, image.color.b, easedValue);
    }

    private IEnumerator Wait()
    {
        Debug.Log("�ҋ@��");
        // �w��b���ҋ@
        yield return new WaitForSeconds(m_Scale);

        if (!isSound)
        {
            isSound = true;
            sound.PlaySound(N_PlaySound.SEName.Stamp);
        }
        Debug.Log("�ҋ@�I��");
        isStart = true;
    }
}
