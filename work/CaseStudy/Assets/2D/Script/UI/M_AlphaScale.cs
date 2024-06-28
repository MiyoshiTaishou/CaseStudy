using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class M_AlphaScale : MonoBehaviour
{
    [Header("待機時間"),SerializeField] 
    float m_Scale = 1f;

    [Header("透過時間"), SerializeField]
    private float fAlphaTime = 1.0f; // 透過にかける時間を調整します

    [Header("イージング関数"), SerializeField]
    M_Easing.Ease ease;

    private float fTime = 0.0f;

    private Image image;

    private bool isStart = false;
    private bool isSound = false;

    [Header("ハンコの音鳴らす用"), SerializeField]
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

        //Debug.Log(easedValue + "透過度");

        image.color = new Color(image.color.r, image.color.g, image.color.b, easedValue);
    }

    private IEnumerator Wait()
    {
        Debug.Log("待機中");
        // 指定秒数待機
        yield return new WaitForSeconds(m_Scale);

        if (!isSound)
        {
            isSound = true;
            sound.PlaySound(N_PlaySound.SEName.Stamp);
        }
        Debug.Log("待機終了");
        isStart = true;
    }
}
