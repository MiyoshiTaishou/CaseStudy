using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ShowCase : MonoBehaviour
{
    [Header("ケース耐久値"), SerializeField]
    private uint nHardness = 1;

    [Header("BreakObjectTag"), SerializeField]
    private string sBreakTag = "EnemyBall";

    private S_EnemyBall enemyBall;


    private void OnCollisionEnter2D(Collision2D collision)
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
                 Destroy(gameObject);
                 Destroy(collision.gameObject);
             }
         }
    }

    private void OnCollisionStay2D(Collision2D collision)
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
                 Destroy(gameObject);
                 Destroy(collision.gameObject);
             }
         }
    }
}