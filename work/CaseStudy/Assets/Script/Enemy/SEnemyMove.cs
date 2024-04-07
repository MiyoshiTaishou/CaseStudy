using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    [Header("Rayの長さ(現状壁と足元共通)"), SerializeField]
    float fDistance=0.0f;
    [Header("Rayの始点をどれだけ離すか(x)"), SerializeField]
    float fGup = 0.0f;
    [Header("足元のRayの方向"), SerializeField]
    float fGrounddirx = 0.2f, fGrounddiry = -0.8f;
    [Header("ピタッと止まる"), SerializeField]
    bool isStop = false;
    [Header("Rayの可視化"), SerializeField]
    bool isRayDraw = false;

    //Rayの始点
    private Vector2 Origin=Vector2.zero;

    //足元に飛ばすRayの方向
    private Vector2 GroundDirection=Vector2.zero;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<MPlayerSearch>().GetIsSearch())
        {
            return;
        }

        //この辺コードくっそ汚いので気が向いたら直しておきます
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

        //壁判定用のRayがタイルマップに接触しているか、足元のRayが何も情報を得られなければ方向を切り替える
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
            rb.velocity = vel;
        }
        //指定の秒数待つ
        yield return new WaitForSeconds(fWaitTime);
        //再開してから実行したい処理を書く
        this.enabled = true;
    }
}
