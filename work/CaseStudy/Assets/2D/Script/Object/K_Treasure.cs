using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Treasure : MonoBehaviour
{
    [Header("ステージクリア(仮)"), SerializeField]
    private GameObject StageClearPrefab;

    private GameObject StageClearObj;

    void Start()
    {
        StageClearObj = Instantiate(StageClearPrefab, transform.position, Quaternion.identity);
        StageClearObj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            StageClearObj.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // プレイヤーがぶつかってきたら
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            StageClearObj.SetActive(true);
        }
    }
}
