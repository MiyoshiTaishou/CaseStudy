using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Treasure : MonoBehaviour
{
    [Header("リンゴ取得SE(NULLでもいいよ)"), SerializeField]
    private AudioClip audioclip;

    private GameObject CaseObj;

    void Start()
    {
        //ケースが存在したらコライダー無効化
        CaseObj = GameObject.Find("Case");
        if(CaseObj)
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            Debug.Log("ケース発見");
        }
        else
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
            Debug.Log("ケースない");
        }
    }

    private void Update()
    {
        if (!CaseObj)
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
            Debug.Log("宝判定有効化");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            //音鳴らす
            if (audioclip)
            {
                Debug.Log("ちりんちりん");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("ぶつかってきた");
            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            //音鳴らす
            if (audioclip)
            {
                Debug.Log("ちりんちりん");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("ぶつかってきた");

            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            //音鳴らす
            if (audioclip)
            {
                Debug.Log("ちりんちりん");
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }

            Debug.Log("ぶつかってきた");

            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            //音鳴らす
            if (audioclip)
            {
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }
            M_GameMaster.SetGameClear(true);
            Destroy(gameObject);
        }
    }
}
