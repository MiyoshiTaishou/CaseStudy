using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyExplosion : MonoBehaviour
{
    [Header("敵爆散のエフェクト"), SerializeField]
    private GameObject Eff_Scatter;

    [Header("敵爆発音"), SerializeField]
    private AudioClip audioclip;

    private S_EnemyBall enemyBall;

    [Header("ヒットストップ"), SerializeField]
    private float fHitStop = 10;

    //敵爆散用
    private ParticleSystem particle;
    private ParticleSystem.Burst burst;


    void Start()
    {
        //パーティクル情報取得
        particle = Eff_Scatter.GetComponent<ParticleSystem>();

        //S_EnemyBallスクリプト取得
        enemyBall = this.GetComponent<S_EnemyBall>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;

        //相手にK_BreakObjectスクリプトが付いていたら
        if (HitObj.GetComponent<K_BreakObject>())
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if(HitObj.GetComponent<K_BreakObject>() .GetBreakNum()<= checkNum && GetComponent<S_EnemyBall>().GetisPushing())
            {
                StartCoroutine(HitStop(HitObj));
                Debug.Log("破壊");
                //// 敵爆散エフェクト
                //burst.count = enemyBall.GetStickCount();
                //particle.emission.SetBurst(0, burst);
                //Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                //// オブジェクト削除と同時に効果音を鳴らす処理
                //if (audioclip)
                //{
                //    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                //}

                //Destroy(HitObj);
                //Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //オブジェクト取得
        GameObject HitObj = collision.transform.gameObject;
        K_BreakObject Break = HitObj.GetComponent<K_BreakObject>();

        //相手にK_BreakObjectスクリプトが付いていたら
        if (Break)
        {
            //敵塊数取得、オブジェクトの耐久値と比較
            int checkNum = enemyBall.GetStickCount();
            if (Break.GetBreakNum() <= checkNum && GetComponent<S_EnemyBall>().GetisPushing())
            {
                StartCoroutine(HitStop(HitObj));
                // 敵爆散エフェクト
                //burst.count = enemyBall.GetStickCount();
                //particle.emission.SetBurst(0, burst);
                //Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

                //// オブジェクト削除と同時に効果音を鳴らす処理
                //if (audioclip)
                //{
                //    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                //}

                //Destroy(HitObj);
                //Destroy(gameObject);
            }
        }
    }
    IEnumerator HitStop(GameObject obj)
    {

        Debug.Log("aaaa");
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(fHitStop / 60);
        Debug.Log("止まる時");
        // 敵爆散エフェクト
        burst.count = enemyBall.GetStickCount();
        particle.emission.SetBurst(0, burst);
        Instantiate(Eff_Scatter, transform.position, Quaternion.identity);

        // オブジェクト削除と同時に効果音を鳴らす処理
        if (audioclip)
        {
            AudioSource.PlayClipAtPoint(audioclip, transform.position);
        }

        Destroy(obj);
        Destroy(gameObject);
    }
}