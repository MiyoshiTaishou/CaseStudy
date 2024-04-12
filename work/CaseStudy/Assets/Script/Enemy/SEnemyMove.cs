using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
    [Header("Ray�̒���(�Ǘp)"), SerializeField]
    float fDistance=0.5f;
    [Header("Ray�̒���(����)"), SerializeField]
    float fGroundDistance=1.0f;
    [Header("Ray�̎n�_���ǂꂾ��������(x)"), SerializeField]
    float fGup = 0.0f;
    [Header("������Ray�̕���"), SerializeField]
    float fGrounddirx = 0.2f, fGrounddiry = -0.8f;
    [Header("�s�^�b�Ǝ~�܂�"), SerializeField]
    bool isStop = false;
    [Header("Ray�̉���"), SerializeField]
    bool isRayDraw = false;
    [Header("�⓹��R�͐���20"), SerializeField]
    float Power = 20.0f;
    [Header("�������x"), SerializeField]
    float fLimitSpeed = 2.0f;

    //Ray�̎n�_
    private Vector2 Origin=Vector2.zero;

    //�����ɔ�΂�Ray�̕���
    private Vector2 GroundDirection=Vector2.zero;

    //�ȉ��A�⓹�v�Z�p�̕ϐ�
    private Vector2 slopeOrigin1=Vector2.zero;
    private Vector2 slopeOrigin2=Vector2.zero;
    private Vector2 slopePos1= Vector2.zero;
    private Vector2 slopePos2= Vector2.zero;
    Vector2 slopeGup1=Vector2.zero;
    private Vector2 slopeGup2=Vector2.zero;

    private float frictionCoefficient = 0.5f; // ���C�W��
    //�⓹��ɂ��邩
    private bool isSlope = false;

    //�n�ɑ����Ă邩
    private bool isGround = false;

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
        slopeGup1.x = 0.2f;
        slopeGup2 = slopeGup1;
        slopeGup2.x *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<MPlayerSearch>().GetIsSearch())
        {
            return;
        }

        //�⓹�v�Z
        //�X�����v�Z���邽�߂̃|�W�V�������擾
        slopeOrigin1 = transform.position;
        slopeOrigin1.x += slopeGup1.x;
        slopeOrigin1.y += slopeGup1.y;
        slopeOrigin2 = transform.position;
        slopeOrigin2.x += slopeGup2.x;
        slopeOrigin2.y += slopeGup2.y;

        RaycastHit2D hitSlope1 = Physics2D.Raycast(slopeOrigin1, Vector2.down, 2.0f);
        RaycastHit2D hitSlope2 = Physics2D.Raycast(slopeOrigin2, Vector2.down, 2.0f);
        if(hitSlope1.collider!=null && hitSlope2.collider!=null&&
            hitSlope1.collider.CompareTag("TileMap")&& hitSlope2.collider.CompareTag("TileMap"))
        {
            //2�_�Ԃ̌X�����v�Z
            Vector2 point1 = hitSlope1.point;
            Vector2 point2 = hitSlope2.point;
            float slopeAngle= Mathf.Atan2(point2.y-point1.y, point2.x-point1.x) * Mathf.Rad2Deg;
            int isRight = IsReflectionX ? -1 : 1;

            if (slopeAngle < 170 && slopeAngle > 10)
            {
                Vector2 vel=rb.velocity;
                vel.x = fLimitSpeed*isRight;
                rb.velocity = vel;
                isSlope = true;
            }
            else
            {
                rb.drag = 1;
                isSlope= false;
            }
            //�X�΂̊p�x�����ȏ�Ȃ�X���Ă���Ɣ��f

            //�X���ɑ΂����R�͂��v�Z
            //�X�΂ɑ΂��鐅�������̍R�͂��v�Z
            float horizontalResistance = rb.mass * Mathf.Abs(rb.gravityScale) * frictionCoefficient * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
            Vector2 vecAngle = new Vector2(Mathf.Cos(slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeAngle * Mathf.Deg2Rad));

            Vector2 resistanceForce = vecAngle * horizontalResistance;
            if(IsReflectionX) 
            {
              rb.AddForce(resistanceForce * Power, ForceMode2D.Force);
            }
            else 
            {
              rb.AddForce(resistanceForce * (Power/2), ForceMode2D.Force);
            }
        }


        //���̕ӃR�[�h�����������̂ŋC���������璼���Ă����܂�
        int coef=IsReflectionX? -1 : 1;
        Origin = transform.position;
        Origin.x += fGup * coef;
        GroundDirection.x=fGrounddirx*coef;
        GroundDirection.y=fGrounddiry;

        RaycastHit2D hitWall = Physics2D.Raycast(Origin, Vector2.right*coef, fDistance);
        RaycastHit2D hitGround = Physics2D.Raycast(Origin, GroundDirection, fGroundDistance);
        if (isRayDraw)
        {
            Debug.DrawRay(Origin, Vector2.right * coef*fDistance, Color.red, 0.1f, false);
            Debug.DrawRay(Origin, GroundDirection*fGroundDistance, Color.blue, 0.1f, false);
            Debug.DrawRay(slopeOrigin1, Vector2.down*2.0f,Color.blue,1.0f,false);
            Debug.DrawRay(slopeOrigin2, Vector2.down*2.0f,Color.green,1.0f,false);
        }

        //�ǔ���p��Ray���^�C���}�b�v�ɐڐG���Ă��邩�A������Ray���������𓾂��Ȃ���Ε�����؂�ւ���
        if (GroundCheck()&&
            hitWall.collider!=null&&hitWall.collider.CompareTag("TileMap")||
            hitWall.collider!=null&&hitWall.collider.CompareTag("Hologram")||
            hitGround.collider==null)
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
        if (Mathf.Abs(rb.velocity.x) > fLimitSpeed)
        {
            Vector2 vel = rb.velocity;
            vel.x = fLimitSpeed*coef;
            rb.velocity = vel;
        }
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
            vel.y = 0;
            rb.velocity = vel;
            if (isSlope)
            {
                rb.isKinematic = true;
            }
        }
        //�w��̕b���҂�
        yield return new WaitForSeconds(fWaitTime);
        //�ĊJ���Ă�����s����������������
        this.enabled = true;
        if(isSlope)
        {
            rb.isKinematic = false;
        }
    }

    private bool GroundCheck()
    {
        Vector2 origin = transform.position;
        origin.y -= 0.7f;
        RaycastHit2D hit=Physics2D.Raycast(origin, Vector2.down,0.2f);
        Debug.DrawRay(origin,Vector2.down*0.2f, Color.yellow);
        if(hit.collider != null && hit.collider.CompareTag("TileMap")) 
        {
            //�z�������蔲���Ȃǂňʒu���傫���ς�����ꍇ�ɖڕW�ʒu���̍X�V���s��
            if(!isGround) 
            {
                if (IsReflectionX)
                {
                    GallPos.x = defaultPos.x - MoveDistance.x;
                    StartCoroutine(Gall());
                }
                else if (!IsReflectionX)
                {
                    GallPos.x = defaultPos.x + MoveDistance.x;
                    StartCoroutine(Gall());
                }
                defaultPos = transform.position;
            }
          isGround= true;
        }
        else
        {
            isGround= false;
        }
        return isGround;
    }
}
