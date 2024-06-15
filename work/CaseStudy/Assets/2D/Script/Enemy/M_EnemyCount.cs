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
            // �^�O�� "EnemyTag" �̂��ׂẴI�u�W�F�N�g������
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTag");

            // �G�̐����X�V
            m_EnemyCount = enemies.Length;

            if (m_EnemyCount == 0)
            {
                M_GameMaster.SetEneymAllKill(true);
            }
        }
    }   
}
