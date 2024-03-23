using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SEnemyMove : MonoBehaviour
{
    [Header("���}�X���ړ����邩"), SerializeField]
    Vector2 MoveDistance= Vector2.zero;

    [Header("�ړ����x"), SerializeField]
    Vector2 MoveSpeed= Vector2.zero;

    [Header("�ڕW�ʒu�܂œ��B���ĉ��b�ҋ@���邩"), SerializeField]
    float fWaitTime = 0.0f;

    //���Ε����������Ă��邩
    private bool IsReflectionX = false;

    private bool IsReflectionY = false;

    bool GetReflectionX() { return IsReflectionX; }
    bool GetReflectionY() { return IsReflectionY; }

    //�����ʒu
    private Vector2 defaultPos= Vector2.zero;

    private Rigidbody2D rb=null;

    private Vector2 GallPos=Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
        GallPos = defaultPos + MoveDistance;
        rb= GetComponent<Rigidbody2D>();
        if(!rb)
        {
            Debug.LogError("RigidBody2D������܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<MPlayerSearch>().GetIsSearch())
        {
            return;
        }     

        rb.AddForce(MoveSpeed);

        //�ȉ����猻�݈ʒu���S�[���ʒu���z���Ă��邩�𔻒肵�A
        //�z���Ă���Αҋ@�����̂��i�s�����𔽓]���鏈��
        if(transform.position.x>GallPos.x&&
            !IsReflectionX) 
        {            
            IsReflectionX = true;
            GallPos.x = defaultPos.x - MoveDistance.x;
            MoveSpeed.x = -MoveSpeed.x;
            StartCoroutine(Gall());
        }
        else if(transform.position.x<GallPos.x&&
            IsReflectionX) 
        {
            Debug.Log("���[�u");
            IsReflectionX = false;
            GallPos.x = defaultPos.x + MoveDistance.x;
            MoveSpeed.x = -MoveSpeed.x;
            StartCoroutine(Gall());
        }

        //����g��Ȃ�Y���ړ�
        //if(transform.position.y>GallPos.y&&
        //    !IsReflectionY) 
        //{
        //    IsReflectionY = true;
        //    GallPos.y = defaultPos.y - MoveDistance.y;
        //    MoveSpeed.y = -MoveSpeed.y;
        //    StartCoroutine(Gall());
        //}
        //else if(transform.position.y<GallPos.y&&
        //    IsReflectionY) 
        //{
        //    IsReflectionY = false;
        //    GallPos.y = defaultPos.y + MoveDistance.y;
        //    MoveSpeed.y = -MoveSpeed.y;
        //    StartCoroutine(Gall());
        //}
    }

    //�R���[�`���őҋ@����
    IEnumerator Gall()
    {
        //�I���܂ő҂��Ăق�������������
        this.enabled = false;
        
        //�w��̕b���҂�
        yield return new WaitForSeconds(fWaitTime);
        //�ĊJ���Ă�����s����������������
        this.enabled = true;
    }
}
