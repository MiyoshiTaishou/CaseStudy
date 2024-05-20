using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ScrollBack : MonoBehaviour
{
    enum SCROLLMODE
    {
        STATIC,
        DYNAMIC
    }

    // �X�N���[�����̃��[�h
    [Header("�X�N���[�����[�h"), SerializeField]
    private SCROLLMODE scrollMode = SCROLLMODE.STATIC;

    [Header("�ړ��̋���(0.0�`0.99)"), SerializeField]
    private float TranckingStrength = 0.99f;

    // BasicArea�����ƂɈړ����������܂�
    [Header("�ړ��ʂ̌��ɂȂ�l"), SerializeField]
    private float BasicArea = 1.0f;

    // ���C���[���ړ��̋����ƃ����N������
    private int layer;

    // Start is called before the first frame update
    void Start()
    {
        if(TranckingStrength <= 0.0f)
        {
            TranckingStrength = 0.0f;
        }
        else if(TranckingStrength >= 0.99f)
        {
            TranckingStrength = 0.99f;
        }

        layer = -100 + (int)(100 * TranckingStrength);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
    }

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h���͂��󂯎��
        float fHorizontalInput = Input.GetAxis("Horizontal");

        // �Ǐ]����Ȃ�
        if (scrollMode == SCROLLMODE.DYNAMIC)
        {
            Vector3 vec = new Vector3(-fHorizontalInput * TranckingStrength * BasicArea * Time.deltaTime,0.0f,0.0f);
            gameObject.transform.Translate(vec, Space.Self);
        }
    }
}