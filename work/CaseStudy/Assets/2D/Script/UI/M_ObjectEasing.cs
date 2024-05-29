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

    private Vector3 savePos;  // Vector2からVector3に変更
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

    [Header("開始時に行うか"), SerializeField]
    private bool isStart = true;

    void Start()
    {
        if (this.transform != null)
        {
            savePos = this.transform.position;
        }
        saveScale = this.transform.localScale;
        saveRot = this.transform.rotation.eulerAngles;
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
                fTime = 1;
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

        // Nullチェックを追加
        if (this.transform != null)
        {
            Debug.Log("Transform is not null");
            this.transform.position = savePos;
        }
        else
        {
            Debug.LogError("Transform is null!");
        }

        Debug.Log("Setting scale and rotation");
        this.transform.localScale = saveScale;
        this.transform.rotation = Quaternion.Euler(saveRot);
    }


    private void Easing()
    {
        float t = Mathf.Clamp01(fTime / duration);

        if (isReverse)
        {
            t = 1 - t;
        }

        // Nullチェックを追加
        if (this.transform != null && pos.isApply)
        {
            var func = M_Easing.GetEasingMethod(pos.ease);
            this.transform.position = savePos + pos.amount * func(t);
        }

        if (scale.isApply)
        {
            var func = M_Easing.GetEasingMethod(scale.ease);
            this.transform.localScale = saveScale + scale.amount * func(t);
        }

        if (rot.isApply)
        {
            var func = M_Easing.GetEasingMethod(rot.ease);
            Vector3 vecRot = saveRot + rot.amount * func(t);
            this.transform.rotation = Quaternion.Euler(vecRot);
        }
    }
}
