using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_EnemyCount : MonoBehaviour
{
    private int m_EnemyCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            

            // タグが "EnemyTag" のすべてのオブジェクトを検索
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // 敵の数を更新
            m_EnemyCount = enemies.Length;

            Debug.Log("エネミーの数を数えます" + m_EnemyCount);

            if (m_EnemyCount == 0)
            {
                Debug.Log("エネミーが0なのでスコアをtrueにします");
                M_GameMaster.SetEneymAllKill(true);
            }
        }
    }   
}
