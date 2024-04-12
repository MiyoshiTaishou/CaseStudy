using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class SEnemyMove : MonoBehaviour
{
    [Header("何マス分移動するか"), SerializeField]
    Vector2 MoveDistance= Vector2.zero;

    [Header("移動速度"), SerializeField]
    Vector2 MoveSpeed= Vector2.zero;

    [Header("目標位置まで到達して何秒待機するか"), SerializeField]
    float fWaitTime = 0.0f;

    //反対方向を向いているか
    private bool IsReflectionX = false;

    private bool IsReflectionY = false;

    bool GetReflectionX() { return IsReflectionX; }
    bool GetReflectionY() { return IsReflectionY; }

    //初期位置
    private Vector2 defaultPos= Vector2.zero;

    private Rigidbody2D rb=null;

    //目標位置
    private Vector2 GallPos=Vector2.zero;

    //壁や崖を判断するRay
    private Ray rayWall,rayGround;
    [Header("Rayの長さ(壁用)"), SerializeField]
    float fDistance=0.5f;
    [Header("Rayの長さ(足元)"), SerializeField]
    float fGroundDistance=1.0f;
    [Header("Rayの始点をどれだけ離すか(x)"), SerializeField]
    float fGup = 0.0f;
    [Header("足元のRayの方向"), SerializeField]
    float fGrounddirx = 0.2f, fGrounddiry = -0.8f;
    [Header("ピタッと止まる"), SerializeField]
    bool isStop = false;
    [Header("Rayの可視化"), SerializeField]
    bool isRayDraw = false;
    [Header("坂道抵抗力推奨20"), SerializeField]
    float Power = 20.0f;
    [Header("制限速度"), SerializeField]
    float fLimitSpeed = 2.0f;

    //Rayの始点
    private Vector2 Origin=Vector2.zero;

    //足元に飛ばすRayの方向
    private Vector2 GroundDirection=Vector2.zero;

    //以下、坂道計算用の変数
    private Vector2 slopeOrigin1=Vector2.zero;
    private Vector2 slopeOrigin2=Vector2.zero;
    private Vector2 slopePos1= Vector2.zero;
    private Vector2 slopePos2= Vector2.zero;
    Vector2 slopeGup1=Vector2.zero;
    private Vector2 slopeGup2=Vector2.zero;

    private float frictionCoefficient = 0.5f; // 摩擦係数
    //坂道上にいるか
    private bool isSlope = false;

    //地に足ついてるか
    private bool isGround = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
        GallPos = defaultPos + MoveDistance;
        rb= GetComponent<Rigidbody2D>();
        if(!rb)
        {
            Debug.LogError("RigidBody2Dがありません");
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

        //坂道計算
        //傾きを計算するためのポジションを取得
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
            //2点間の傾きを計算
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
            //傾斜の角度が一定以上なら傾いていると判断

            //傾きに対する抵抗力を計算
            //傾斜に対する水平方向の抗力を計算
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


        //この辺コードくっそ汚いので気が向いたら直しておきます
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

        //壁判定用のRayがタイルマップに接触しているか、足元のRayが何も情報を得られなければ方向を切り替える
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
        //以下から現在位置がゴール位置を越えているかを判定し、
        //越えていれば待機したのち進行方向を反転する処理
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

        //現状使わないY軸移動
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

    //コルーチンで待機処理
    IEnumerator Gall()
    {
        //終わるまで待ってほしい処理を書く
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
        //指定の秒数待つ
        yield return new WaitForSeconds(fWaitTime);
        //再開してから実行したい処理を書く
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
            //ホロ床すり抜けなどで位置が大きく変わった場合に目標位置等の更新を行う
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
