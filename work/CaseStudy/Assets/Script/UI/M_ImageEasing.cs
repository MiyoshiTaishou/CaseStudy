using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static M_Easing;

public class M_ImageEasing : MonoBehaviour
{
    /// <summary>
    /// UI�摜
    /// </summary>
    private Image image;

    /// <summary>
    /// ���Ԍv���p
    /// </summary>
    private float fTime;

    /// <summary>
    /// �C�[�W���O����true
    /// </summary>
    private bool isEasing = false;

    /// <summary>
    /// �����l�ۑ�
    /// </summary>
    private Vector3 savePos;
    private Vector3 saveScale;
    private Vector3 saveRot;

    [Header("�A�j���[�V��������"), SerializeField]
    private float duration = 1.0f; // �A�j���[�V�����̎���  

    //�\���̕\���e�X�g�N���X
    [System.Serializable]
    struct ApplyEasing
    {
        [Header("�C�[�W���O�����邩�ǂ���")]
        public bool isApply;

        [Header("�ǂ̃C�[�W���O��K�p���邩")]
        public M_Easing.Ease ease;

        [Header("�㏸��")]
        public Vector3 amount;
    }

    [Header("pos,scale,rot���ꂼ��ɃC�[�W���O��K�p���邩")]
    [SerializeField] private ApplyEasing pos;
    [SerializeField] private ApplyEasing rot;
    [SerializeField] private ApplyEasing scale;   

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        //�����l�ۑ�
        savePos = image.transform.position;
        saveScale = image.transform.localScale;
        saveRot.x = image.transform.rotation.x;
        saveRot.y = image.transform.rotation.y;
        saveRot.z = image.transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {       
        //�C�[�W���O���͎��Ԃ�i�߂�
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
    /// �C�[�W���O���J�n����
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
        // �w�肵�����ԓ��ł̐i�s�x���v�Z
        float t = Mathf.Clamp01(fTime / duration);       

        //���W�̃C�[�W���O
        if (pos.isApply)
        {
            //�C�[�W���O�֐����Ă�
            var func = M_Easing.GetEasingMethod(pos.ease);
            image.transform.position = Vector3.Lerp(savePos, savePos + pos.amount, func(t));
        }

        //�X�P�[���̃C�[�W���O
        if (scale.isApply)
        {
            //�C�[�W���O�֐����Ă�
            var func = M_Easing.GetEasingMethod(scale.ease);
            image.transform.localScale = Vector3.Lerp(saveScale, saveScale + scale.amount, func(t));
        }

        //��]�̃C�[�W���O
        if(rot.isApply)
        {
            //�C�[�W���O�֐����Ă�
            var func = M_Easing.GetEasingMethod(rot.ease);
            Vector3 vecRot = Vector3.Lerp(saveRot, saveRot + rot.amount, func(t));
            image.transform.rotation = Quaternion.Euler(vecRot);

        }
    }
}
