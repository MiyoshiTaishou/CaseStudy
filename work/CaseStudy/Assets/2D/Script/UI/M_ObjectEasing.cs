using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ObjectEasing : MonoBehaviour
{
    private float fTime;
    private bool isEasing = false;
    public bool GetEasing() { return isEasing; }

    private bool isReverse = false;

    public void SetReverse(bool reverse) { isReverse = reverse; }

    private Vector2 savePos;
    private Vector3 saveScale;
    private Vector3 saveRot;

    [System.Serializable]
    struct ApplyEasing
    {
        public bool isApply;
        public M_Easing.Ease ease;
        public Vector2 amount;  // Vector3からVector2に変更
    }

    [Header("Animation Duration")]
    [SerializeField] private float duration = 1.0f;

    [Header("Apply easing to position, scale, rotation")]
    [SerializeField] private ApplyEasing pos;
    [SerializeField] private ApplyEasing rot;
    [SerializeField] private ApplyEasing scale;

    [Header("開始時に行うか"), SerializeField]
    private bool isStart = true;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            savePos = rectTransform.anchoredPosition;
        }
        saveScale = rectTransform.localScale;
        saveRot = rectTransform.localRotation.eulerAngles;
        isEasing = isStart;
        fTime = 0.0f;
    }

    void Update()
    {
        if (isEasing)
        {
            fTime += Time.deltaTime;
            if (fTime > duration)
            {
                fTime = duration;
                isEasing = false;
            }
            Easing();
        }
    }

    public void EasingOnOff()
    {
        Debug.Log("EasingOnOff called");
        isEasing = !isEasing;
        fTime = 0;

        if (rectTransform != null)
        {
            Debug.Log("RectTransform is not null");
            rectTransform.anchoredPosition = savePos;
        }
        else
        {
            Debug.LogError("RectTransform is null!");
        }

        Debug.Log("Setting scale and rotation");
        rectTransform.localScale = saveScale;
        rectTransform.localRotation = Quaternion.Euler(saveRot);
    }

    private void Easing()
    {
        float t = Mathf.Clamp01(fTime / duration);

        if (isReverse)
        {
            t = 1 - t;
        }

        if (rectTransform != null && pos.isApply)
        {
            var func = M_Easing.GetEasingMethod(pos.ease);
            rectTransform.anchoredPosition = savePos + pos.amount * func(t);
        }

        if (scale.isApply)
        {
            var func = M_Easing.GetEasingMethod(scale.ease);
            rectTransform.localScale = saveScale + new Vector3(scale.amount.x, scale.amount.y, 0) * func(t);
        }

        if (rot.isApply)
        {
            var func = M_Easing.GetEasingMethod(rot.ease);
            Vector3 vecRot = saveRot + new Vector3(0, 0, rot.amount.x) * func(t);  // 2DではZ回転のみ適用
            rectTransform.localRotation = Quaternion.Euler(vecRot);
        }
    }
}
