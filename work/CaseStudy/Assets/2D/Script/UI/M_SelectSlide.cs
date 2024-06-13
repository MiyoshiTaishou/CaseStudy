using UnityEngine;

public class M_SelectSlide : MonoBehaviour
{
    [Header("ˆÚ“®—Ê"), SerializeField]
    private float xMove = 2000;

    [Header("ˆÚ“®—Ê‚Ì”{—¦"), SerializeField]
    private float moveMultiplier = 1.0f; // ˆÚ“®—Ê‚Ì”{—¦‚ð’²®‚µ‚Ü‚·

    [Header("ˆÚ“®‚É‚©‚¯‚éŽžŠÔ"), SerializeField]
    private float fMoveTime = 1.0f; // ˆÚ“®‚É‚©‚¯‚éŽžŠÔ‚ð’²®‚µ‚Ü‚·

    [Header("ƒC[ƒWƒ“ƒOŠÖ”"), SerializeField]
    M_Easing.Ease ease;

    private bool isEasing = false;
    private float fTime;

    private bool isAdd = true;

    private Vector3 Startpos;

    private void Start()
    {
        Startpos = this.transform.position;
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
            }
            Easing();
        }
    }

    public void EasingOnOff()
    {
        isEasing = !isEasing;
        fTime = 0;
        Startpos = this.transform.position;
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
            transform.position = Startpos + Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
        else
        {
            transform.position = Startpos - Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
    }
}
