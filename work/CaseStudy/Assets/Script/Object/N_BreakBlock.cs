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

    [Header("破壊時のエフェクト"), SerializeField]
    private GameObject Effect;

    [Header("AudioClip"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    private Transform trans_Block;

    // Start is called before the first frame update
    void Start()
    {
        trans_Block = gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 最初だけスクリプト取得
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 指定したタグをもったオブジェクトがぶつかってきたら
        if(collision.collider.tag == sBreakTag)
        {
            enemyBall = collision.gameObject.GetComponent<S_EnemyBall>();

            // ぶつかってきた敵塊の数を取得
            int checkNum = (int)enemyBall.GetStickCount();

            // 指定値以上の塊がぶつかってきたら
            if (checkNum >= iBreakNum)
            {
                Vector3 pos = trans_Block.position;

                Instantiate(Effect, pos, Quaternion.identity);

                // オブジェクト削除と同時に効果音を鳴らす処理
                AudioSource.PlayClipAtPoint(audioclip, pos);

                Destroy(gameObject);
            }
        }
    }
}
