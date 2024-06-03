using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TimeContoroller : MonoBehaviour
{
    [Header("�x������l"), SerializeField]
    private float slowTime = 0.1f;

    [Header("���������̃A�N�V������"), SerializeField]
    private string actionName = "EnemyPush";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis(actionName) > 0.5)
        {
            Time.timeScale = 1.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = slowTime;
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 1.0f;
        }
    }
}
