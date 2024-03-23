using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private Vector2 GallPos=Vector2.zero;
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
            Debug.Log("ムーブ");
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
        
        //指定の秒数待つ
        yield return new WaitForSeconds(fWaitTime);
        //再開してから実行したい処理を書く
        this.enabled = true;
    }
}
