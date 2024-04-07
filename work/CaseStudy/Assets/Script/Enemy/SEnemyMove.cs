using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

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

    //�ڕW�ʒu
    private Vector2 GallPos=Vector2.zero;

    //�ǂ�R�𔻒f����Ray
    private Ray rayWall,rayGround;
    [Header("Ray�̒���(����ǂƑ�������)"), SerializeField]
    float fDistance=0.0f;
    [Header("Ray�̎n�_���ǂꂾ��������(x)"), SerializeField]
    float fGup = 0.0f;
    [Header("������Ray�̕���"), SerializeField]
    float fGrounddirx = 0.2f, fGrounddiry = -0.8f;
    [Header("�s�^�b�Ǝ~�܂�"), SerializeField]
    bool isStop = false;
    [Header("Ray�̉���"), SerializeField]
    bool isRayDraw = false;

    //Ray�̎n�_
    private Vector2 Origin=Vector2.zero;

    //�����ɔ�΂�Ray�̕���
    private Vector2 GroundDirection=Vector2.zero;
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

        //���̕ӃR�[�h�����������̂ŋC���������璼���Ă����܂�
        int coef=IsReflectionX? -1 : 1;
        Origin = transform.position;
        Origin.x += fGup * coef;
        GroundDirection.x=fGrounddirx*coef;
        GroundDirection.y=fGrounddiry;

        RaycastHit2D hitWall = Physics2D.Raycast(Origin, Vector2.right*coef, fDistance);
        RaycastHit2D hitGround = Physics2D.Raycast(Origin, GroundDirection, fDistance);
        if (isRayDraw)
        {
            Debug.DrawRay(Origin, Vector2.right * coef, Color.red, 0.1f, false);
            Debug.DrawRay(Origin, GroundDirection, Color.blue, 0.1f, false);
        }

        //�ǔ���p��Ray���^�C���}�b�v�ɐڐG���Ă��邩�A������Ray���������𓾂��Ȃ���Ε�����؂�ւ���
        if (hitWall.collider!=null&&hitWall.collider.CompareTag("TileMap")||hitGround.collider==null)
        {
            IsReflectionX= !IsReflectionX;
            if(IsReflectionX) 
            {
                GallPos.x = defaultPos.x - MoveDistance.x;
                MoveSpeed.x = -MoveSpeed.x;
                StartCoroutine(Gall());
            }
            else if(!IsReflectionX) 
            {
                GallPos.x = defaultPos.x + MoveDistance.x;
                MoveSpeed.x = -MoveSpeed.x;
                StartCoroutine(Gall());
            }
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
        if (isStop)
        {
            Vector2 vel = rb.velocity;
            vel.x = 0;
            rb.velocity = vel;
        }
        //�w��̕b���҂�
        yield return new WaitForSeconds(fWaitTime);
        //�ĊJ���Ă�����s����������������
        this.enabled = true;
    }
}
