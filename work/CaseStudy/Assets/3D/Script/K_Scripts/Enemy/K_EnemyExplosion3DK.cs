using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyExplosion3DK : MonoBehaviour
{
    [Header("敵爆散のエフェクト"), SerializeField]
    private GameObject Eff_Scatter;

    [Header("敵爆発音"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall3DK enemyBall;

    //敵爆散用
    private ParticleSystem particle;
    private ParticleSystem.Burst burst;

    void Start()
    {
        //パーティクル情報取得
        particle = Eff_Scatter.GetComponent<ParticleSystem>();

        //S_EnemyBallスクリプト取得
        enemyBall = this.GetComponent<S_EnemyBall3DK>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;

        //相手にK_BreakObjectスクリプトが付いていたら
        if (HitObj.GetComponent<K_BreakObject>())
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if(HitObj.GetComponent<K_BreakObject>() .GetBreakNum()<= checkNum)
            {
                // 敵爆散エフェクト
                burst.count = enemyBall.GetStickCount();
                particle.emission.SetBurst(0, burst);
                Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                // オブジェクト削除と同時に効果音を鳴らす処理
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }

                Destroy(HitObj);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;
        K_BreakObject Break = HitObj.GetComponent<K_BreakObject>();

        //相手にK_BreakObjectスクリプトが付いていたら
        if (Break)
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if (Break.GetBreakNum() <= checkNum)
            {
                // 敵爆散エフェクト
                burst.count = enemyBall.GetStickCount();
                particle.emission.SetBurst(0, burst);
                Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                // オブジェクト削除と同時に効果音を鳴らす処理
                if(audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }

                Destroy(HitObj);
                Destroy(gameObject);
            }
        }
    }
}