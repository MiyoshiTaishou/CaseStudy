using UnityEngine;

public class M_SelectSlide : MonoBehaviour
{
    [Header("�ړ���"), SerializeField]
    private float xMove = 2000;

    [Header("�ړ��ʂ̔{��"), SerializeField]
    private float moveMultiplier = 1.0f; // �ړ��ʂ̔{���𒲐����܂�

    [Header("�ړ��ɂ����鎞��"), SerializeField]
    private float fMoveTime = 1.0f; // �ړ��ɂ����鎞�Ԃ𒲐����܂�

    [Header("�C�[�W���O�֐�"), SerializeField]
    M_Easing.Ease ease;

    private bool isEasing = false;
    private float fTime;

    private bool isAdd = true;

    private Vector3 Startpos;

    /// <summary>
    /// �X���C�h����
    /// </summary>
    private bool isSlide = false;

    public bool GetIsSlide() { return isSlide; }

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
                isSlide = false;
            }
            Easing();
        }
    }

    public void EasingOnOff()
    {
        isEasing = !isEasing;
        fTime = 0;
        Startpos = this.transform.position;

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
            transform.position = Startpos + Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
        else
        {
            transform.position = Startpos - Vector3.Lerp(Vector3.zero, new Vector3(actualMove, 0, 0), easedValue);
        }
    }
}
