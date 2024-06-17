using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class M_AlphaScale : MonoBehaviour
{

    [Header("透過時間"), SerializeField]
    private float fAlphaTime = 1.0f; // 透過にかける時間を調整します

    [Header("イージング関数"), SerializeField]
    M_Easing.Ease ease;

    private float fTime = 0.0f;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {        
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

        image.color = new Color(image.color.r, image.color.g, image.color.b, easedValue);
    }
}
