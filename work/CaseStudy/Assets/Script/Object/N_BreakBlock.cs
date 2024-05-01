using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//耐久値表示用(2024/04/25　木村追加)

public class N_BreakBlock : MonoBehaviour
{
    // オブジェクトを破壊できるようになる数
    [Header("壊せるようになる敵の数"), SerializeField]
    private int iBreakNum = 3;

    /// <summary>
    /// 自身のオブジェクトを破壊するオブジェクトが持つタグ名
    /// </summary>
    [Header("BreakObjectTag"), SerializeField]
    private const string sBreakTag = "EnemyBall";

    [Header("破壊時のエフェクト"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("敵爆散のエフェクト"), SerializeField]
    private GameObject Eff_Scatter;

    [Header("AudioClip"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    private Transform trans_Block;

    //耐久値表示用(2024/04/25　木村追加)
    private TextMeshProUGUI TextEndurance;

    // Start is called before the first frame update
    void Start()
    {
        trans_Block = gameObject.transform;

        //耐久値表示用(2024/04/25　木村追加)
        GameObject cd = transform.GetChild(0).gameObject; ;
        GameObject gcd = cd.GetComponent<Transform>().transform.GetChild(0).gameObject;
        TextEndurance = gcd.GetComponent<TextMeshProUGUI>();
        TextEndurance.text = iBreakNum.ToString();
        Debug.Log(TextEndurance.text);
    }

    //耐久値表示用(2024/04/25　木村追加)
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z); // 適切なオフセットを持たせる
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

                // ブロック爆発
                Instantiate(Eff_Explosion, pos, Quaternion.identity);

                // 敵爆散
                pos = collision.transform.position;
                Instantiate(Eff_Scatter, pos, Quaternion.identity);

                // オブジェクト削除と同時に効果音を鳴らす処理
                AudioSource.PlayClipAtPoint(audioclip, pos);

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
            if (checkNum >= iBreakNum)
            {
                Vector3 pos = trans_Block.position;

                // ブロック爆発
                Instantiate(Eff_Explosion, pos, Quaternion.identity);

                // 敵爆散
                pos = collision.transform.position;
                Instantiate(Eff_Scatter, pos, Quaternion.identity);

                // オブジェクト削除と同時に効果音を鳴らす処理
                AudioSource.PlayClipAtPoint(audioclip, pos);

                Destroy(gameObject);
                Destroy(collision.gameObject);

            }
        }
    }
}
