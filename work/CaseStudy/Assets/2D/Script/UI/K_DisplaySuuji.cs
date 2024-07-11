using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_DisplaySuuji : MonoBehaviour
{
    [Header("�����摜"), SerializeField]
    private Sprite[] Tex;

    [Header("�\������������"), SerializeField]
    private int Num = 0;
    public void SetNum(int _Num) { Num = _Num; }

    [Header("�T�C�Y"), SerializeField]
    private float Size = 1;

    public void SetSize(int _Size) { Size = _Size; }

    //�q�I�u�W�F�N�g��
    private int ChildCount;

    //�����T�C�Y
    private Vector3 InitSize;

    // Start is called before the first frame update
    void Start()
    {
        //�������ۑ�
        ChildCount = 0;
        InitSize = this.gameObject.transform.localScale;
        //�ő�܂Ŏq�I�u�W�F�N�g�𐶐�����
        int num = 2147483647;
        for(int i=0; num!=0; i++)
        {
            //�����̎q�I�u�W�F�N�g�𐶐�
            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            obj.transform.position = new Vector3(this.transform.position.x - i, this.transform.position.y, this.transform.position.z) ;
            obj.layer = 5;
            //���O�ύX
            obj.name = i.ToString();
            //�X�v���C�g�����_���[�ǉ�
            obj.AddComponent<SpriteRenderer>();
            //�����̐�������U��
            obj.GetComponent<SpriteRenderer>().sprite = Tex[num % 10];
            obj.GetComponent<SpriteRenderer>().sortingOrder = 20;
            //�q�I�u�W�F�N�g�̐�+1
            ChildCount++;
            //�ő包�����f����
            if (num/10 == 0)
            {//�ő包��������
                //�����ɉ����ăT�C�Y�ς���
                if(i==0)
                {//�ꌅ��������
                    this.gameObject.transform.localScale = InitSize * Size;
                }
                else
                {//�񌅈ȏゾ������
                    this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
                }
                break;
            }
            else
            {//���̌��̌v�Z��
                num = num / 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�������X�V����
        int ActiveNumCount = 0;
        int num = Num;
        for (int i = 0; i< ChildCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            obj.GetComponent<SpriteRenderer>().sprite = Tex[num % 10];
            if(num<=0)
            {
                obj.SetActive(false);
            }
            else
            {
                ActiveNumCount++;
                obj.SetActive(true);
            }
            num = num / 10;
        }


        //�����ɉ����ăT�C�Y�ς���
        if(ActiveNumCount <= 1)
        {
            this.gameObject.transform.localScale = InitSize * Size;
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
        }

        //�ʒu����
        float Mediannum = 0;
        if(ActiveNumCount % 2==0)
        {
            Mediannum = ActiveNumCount / 2 + 0.5f;
            for (int i = 1; i <= ActiveNumCount; i++)
            {
                GameObject obj = transform.GetChild(i-1).gameObject;
                obj.transform.position = new Vector3(this.transform.position.x + (Mediannum - i) * Size * 0.65f, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            Mediannum = ActiveNumCount / 2 + 1;
            for (int i = 1; i <= ActiveNumCount; i++)
            {
                GameObject obj = transform.GetChild(i-1).gameObject;
                obj.transform.position = new Vector3(this.transform.position.x + (Mediannum - i) * Size * 0.65f, this.transform.position.y, this.transform.position.z);
            }
        }
    }
}
