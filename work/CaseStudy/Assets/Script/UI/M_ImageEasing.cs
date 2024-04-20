using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static M_Easing;

public class M_ImageEasing : MonoBehaviour
{
    /// <summary>
    /// UI画像
    /// </summary>
    private Image image;

    /// <summary>
    /// 時間計測用
    /// </summary>
    private float fTime;

    /// <summary>
    /// イージング中はtrue
    /// </summary>
    private bool isEasing = false;

    /// <summary>
    /// 初期値保存
    /// </summary>
    private Vector3 savePos;
    private Vector3 saveScale;
    private Vector3 saveRot;

    [Header("アニメーション時間"), SerializeField]
    private float duration = 1.0f; // アニメーションの時間  

    //構造体表示テストクラス
    [System.Serializable]
    struct ApplyEasing
    {
        [Header("イージングをするかどうか")]
        public bool isApply;

        [Header("どのイージングを適用するか")]
        public M_Easing.Ease ease;

        [Header("上昇量")]
        public Vector3 amount;
    }

    [Header("pos,scale,rotそれぞれにイージングを適用するか")]
    [SerializeField] private ApplyEasing pos;
    [SerializeField] private ApplyEasing rot;
    [SerializeField] private ApplyEasing scale;   

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        //初期値保存
        savePos = image.transform.position;
        saveScale = image.transform.localScale;
        saveRot.x = image.transform.rotation.x;
        saveRot.y = image.transform.rotation.y;
        saveRot.z = image.transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {       
        //イージング中は時間を進める
        if (isEasing)
        {
            fTime += Time.deltaTime;
        }
        else
        {
            fTime = 0;
        }

        if(fTime > duration)
        {
            fTime = 0;
        }

        Easing();
    }

    /// <summary>
    /// イージングを開始する
    /// </summary>
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
        // 指定した時間内での進行度を計算
        float t = Mathf.Clamp01(fTime / duration);       

        //座標のイージング
        if (pos.isApply)
        {
            //イージング関数を呼ぶ
            var func = M_Easing.GetEasingMethod(pos.ease);
            image.transform.position = Vector3.Lerp(savePos, savePos + pos.amount, func(t));
        }

        //スケールのイージング
        if (scale.isApply)
        {
            //イージング関数を呼ぶ
            var func = M_Easing.GetEasingMethod(scale.ease);
            image.transform.localScale = Vector3.Lerp(saveScale, saveScale + scale.amount, func(t));
        }

        //回転のイージング
        if(rot.isApply)
        {
            //イージング関数を呼ぶ
            var func = M_Easing.GetEasingMethod(rot.ease);
            Vector3 vecRot = Vector3.Lerp(saveRot, saveRot + rot.amount, func(t));
            image.transform.rotation = Quaternion.Euler(vecRot);

        }
    }
}
