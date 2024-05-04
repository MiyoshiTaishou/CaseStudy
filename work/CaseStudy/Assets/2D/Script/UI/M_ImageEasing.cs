using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_ImageEasing : MonoBehaviour
{
    private Image image;
    private float fTime;
    private bool isEasing = false;

    public bool GetEasing() { return isEasing; }

    private bool isReverse = false;

    public void SetReverse(bool reverse) { isReverse = reverse; }

    private Vector3 savePos;
    private Vector3 saveScale;
    private Vector3 saveRot;

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

    [Header("äJénéûÇ…çsÇ§Ç©"), SerializeField]
    private bool isStart = true;

    void Start()
    {
        image = GetComponent<Image>();
        savePos = image.transform.position;
        saveScale = image.transform.localScale;
        saveRot = image.transform.rotation.eulerAngles;
        isEasing = isStart;
    }

    void Update()
    {
        if (isEasing)
        {
            fTime += Time.deltaTime;
            if (fTime > duration)
            {
                fTime = 1;
                isEasing = false;
            }
            Easing();
        }
    }

    public void EasingOnOff()
    {
        isEasing = !isEasing;
        fTime = 0;
        image.transform.position = savePos;
        image.transform.localScale = saveScale;
        image.transform.rotation = Quaternion.Euler(saveRot);
    }

    private void Easing()
    {
        float t = Mathf.Clamp01(fTime / duration);

        if(isReverse)
        {
            t = 1 - t;
        }

        if (pos.isApply)
        {
            var func = M_Easing.GetEasingMethod(pos.ease);
            image.transform.position = savePos + pos.amount * func(t);
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
}
