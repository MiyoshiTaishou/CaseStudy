using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DuctEnemyWarp : MonoBehaviour
{
    [Header("ワープ先オブジェクト"),SerializeField]
    GameObject m_WarpObj; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.transform.position = m_WarpObj.transform.position;
        }
    }
}
