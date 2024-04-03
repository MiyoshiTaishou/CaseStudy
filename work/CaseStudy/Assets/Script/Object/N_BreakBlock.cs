using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_BreakBlock : MonoBehaviour
{
    // オブジェクトを破壊できるようになる数
    [Header("壊せるようになる敵の数"), SerializeField]
    private int iBreakNum = 3;

    [Header("テスト用　敵塊の数"), SerializeField]
    private int iTestNum = 1;

    /// <summary>
    /// 自身のオブジェクトを破壊するオブジェクトが持つタグ名
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "Enemy";

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 指定したタグをもったオブジェクトがぶつかってきたら
        if(collision.collider.tag == sBreakTag)
        {
            // 指定値以上の塊がぶつかってきたら
            if(iTestNum >= iBreakNum)
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
