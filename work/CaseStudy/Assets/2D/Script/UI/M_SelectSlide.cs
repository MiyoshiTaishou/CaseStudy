using UnityEngine;

public class M_SelectSlide : MonoBehaviour
{
    [Header("移動量"), SerializeField]
    private float xMove = 2000;

    [Header("移動量の倍率"), SerializeField]
    private float moveMultiplier = 1.0f; // 移動量の倍率を調整します

    [Header("移動にかける時間"), SerializeField]
    private float fMoveTime = 1.0f; // 移動にかける時間を調整します

    [Header("イージング関数"), SerializeField]
    M_Easing.Ease ease;

    private bool isEasing = false;
    private float fTime;

    private bool isAdd = true;

    private Vector3 Startpos;

    /// <summary>
    /// スライド中か
    /// </summary>
    private bool isSlide = false;

    private RectTransform rectTransform;

    public bool GetIsSlide() { return isSlide; }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Startpos = rectTransform.anchoredPosition;     
    }

    private void Update()
    {
        if (isEasing)
        {
            fTime += Time.deltaTime;
            if (fTime > fMoveTime)
            {
                fTime = fMoveTime;
                isEasing = false;
                isSlide = false;
            }
            Easing();
        }
    }

    public void EasingOnOff()
    {
        isEasing = !isEasing;
        fTime = 0;
        Startpos = rectTransform.anchoredPosition;

        isSlide = true;
    }

    public void Add()
    {
        EasingOnOff();
        isAdd = true;
    }

    public void Sub()
    {
        EasingOnOff();
        isAdd = false;
    }

    private void Easing()
    {
        float t = fTime / fMoveTime;
        float easedValue = M_Easing.GetEasingMethod(ease)(t);

        float actualMove = xMove * moveMultiplier;

        if (isAdd)
        {
            rectTransform.anchoredPosition = Startpos + Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
        else
        {
            rectTransform.anchoredPosition = Startpos - Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
    }
}
