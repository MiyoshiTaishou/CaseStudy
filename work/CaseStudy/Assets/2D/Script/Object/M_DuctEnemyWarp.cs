using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DuctEnemyWarp : MonoBehaviour
{
    [Header("���[�v��I�u�W�F�N�g"),SerializeField]
    GameObject m_WarpObj; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.transform.position = m_WarpObj.transform.position;
        }
    }
}
