using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_ImageEasing : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;
    private float fTime;
    private bool isEasing = false;

    public bool GetEasing() { return isEasing; }

    private bool isReverse = false;

    private bool isResset = false;

    public void SetReverse(bool reverse) { isReverse = reverse; }

    private Vector2 savePos;
    private Vector3 saveScale;
    private Vector3 saveRot;

    [Header("ループさせるか"),SerializeField]
    private bool isLoop = false;

    [System.Serializable]
    struct ApplyEasing
    {
        public bool isApply;
        public M_Easing.Ease ease;
        public Vector3 amount;
    }

    [Header("Animation Duration")]
    [SerializeField] private float duration = 1.0f;

    [Header("Apply easing to position, scale, rotation")]
    [SerializeField] private ApplyEasing pos;
    [SerializeField] private ApplyEasing rot;
    [SerializeField] private ApplyEasing scale;

    [Header("開始時に行うか"), SerializeField]
    private bool isStart = true;

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = image.GetComponent<RectTransform>();
        savePos = rectTransform.anchoredPosition;
        saveScale = image.transform.localScale;
        saveRot = image.transform.rotation.eulerAngles;
        isEasing = isStart;
    }

    void Update()
    {
        if(isResset)
        {
            Resset();
            return;
        }

        if (isEasing && !isLoop)
        {
            fTime += Time.deltaTime;
            if (fTime > duration)
            {
                fTime = duration;
                isEasing = false;
            }
            Easing();
        }

        if(isEasing && isLoop)
        {
            if(isReverse)
            {
                fTime -= Time.deltaTime;
            }
            else
            {
                fTime += Time.deltaTime;
            }

            if(fTime > duration)
            {
                fTime = duration;
                isReverse = true;
            }
            else if(fTime < 0.0f)
            {
                fTime = 0.0f;
                isReverse = false;
            }

            Easing();
        }
    }

    public void EasingOnOff()
    {
        isEasing = !isEasing;
        fTime = 0;
        rectTransform.anchoredPosition = savePos;
        image.transform.localScale = saveScale;
        image.transform.rotation = Quaternion.Euler(saveRot);
    }

    private void Easing()
    {
        float t = Mathf.Clamp01(fTime / duration);

        if (isReverse && !isLoop)
        {
            t = 1 - t;
        }

        if (pos.isApply)
        {
            var func = M_Easing.GetEasingMethod(pos.ease);
            rectTransform.anchoredPosition = savePos + (Vector2)(pos.amount * func(t));
        }

        if (scale.isApply)
        {
            var func = M_Easing.GetEasingMethod(scale.ease);
            image.transform.localScale = saveScale + scale.amount * func(t);
        }

        if (rot.isApply)
        {
            var func = M_Easing.GetEasingMethod(rot.ease);
            Vector3 vecRot = saveRot + rot.amount * func(t);
            image.transform.rotation = Quaternion.Euler(vecRot);
        }
    }

    public void Resset()
    {
        rectTransform.anchoredPosition = savePos;
        image.transform.localScale = saveScale;
        image.transform.rotation = Quaternion.Euler(saveRot);
        isEasing = false;
    }

    public bool GetIsEasing()
    {
        return isEasing;
    }
}
