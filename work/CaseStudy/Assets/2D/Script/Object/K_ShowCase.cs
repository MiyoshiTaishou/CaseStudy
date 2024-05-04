using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ShowCase : MonoBehaviour
{
    [Header("ケース耐久値"), SerializeField]
    private uint nHardness = 1;

    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private GameObject CaseObj;
    private BoxCollider2D CaseCollider;
    private GameObject TreasureObj;
    private BoxCollider2D TreasureCollider;
    private bool IsBreaked;
    private S_EnemyBall enemyBall;

    void Start()
    {
        CaseObj = transform.Find("Case").gameObject;
        CaseCollider = CaseObj.GetComponent<BoxCollider2D>();
        TreasureObj = transform.Find("Treasure").gameObject;
        TreasureCollider = TreasureObj.GetComponent<BoxCollider2D>();
        IsBreaked = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ケースが壊されていなかったら
        if(IsBreaked==false)
        {
            // 指定したタグをもったオブジェクトがぶつかってきたら
            if (collision.collider.tag == sBreakTag)
            {
                enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

                // ぶつかってきた敵塊の数を取得
                int checkNum = (int)enemyBall.GetStickCount();

                // 指定値以上の塊がぶつかってきたら
                if (checkNum >= nHardness)
                {
                    Destroy(CaseObj);
                    Destroy(collision.gameObject);
                    IsBreaked = true;
                }
            }
        }
        else
        {
            // 指定したタグをもったオブジェクトがぶつかってきたら
            if (collision.collider.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //ケースが壊されていなかったら
        if (IsBreaked == false)
        {
            // 指定したタグをもったオブジェクトがぶつかってきたら
            if (collision.collider.tag == sBreakTag)
            {
                enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

                // ぶつかってきた敵塊の数を取得
                int checkNum = (int)enemyBall.GetStickCount();

                // 指定値以上の塊がぶつかってきたら
                if (checkNum >= nHardness)
                {
                    Destroy(CaseObj);
                    Destroy(collision.gameObject);
                    IsBreaked = true;
                }
            }
        }
        else
        {
            // 指定したタグをもったオブジェクトがぶつかってきたら
            if (collision.collider.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}