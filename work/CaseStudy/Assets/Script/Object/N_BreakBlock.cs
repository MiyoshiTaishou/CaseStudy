using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_BreakBlock : MonoBehaviour
{
    // オブジェクトを破壊できるようになる数
    [Header("壊せるようになる敵の数"), SerializeField]
    private int iBreakNum = 3;

    /// <summary>
    /// 自身のオブジェクトを破壊するオブジェクトが持つタグ名
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private Animator animator;

    private S_EnemyBall enemyBall;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 最初だけスクリプト取得
        enemyBall = collision.GetComponent<S_EnemyBall>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 指定したタグをもったオブジェクトがぶつかってきたら
        if(collision.collider.tag == sBreakTag)
        {
            // ぶつかってきた敵塊の数を取得
            int checkNum = (int)enemyBall.GetStickCount();

            // 指定値以上の塊がぶつかってきたら
            if (checkNum >= iBreakNum)
            {
                animator.SetBool("break", true);
            }
        }
    }

    void SetDestroy()
    {
        animator.SetBool("destroy", true);

        // 自身を削除
        Destroy(this.gameObject);
    }
}
