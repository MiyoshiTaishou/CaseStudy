using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch : MonoBehaviour
{
    private N_EnemyManager enemyManager;
    private SEnemyMove enemyMove;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = this.gameObject.GetComponent<SEnemyMove>();
        enemyManager = enemyMove.GetManager();

        //Debug.Log(enemyManager);
        //Debug.Log(enemyMove);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Decoy"))
        {
            Debug.Log("î≠å©");
            enemyManager.SetTarget(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
        {
            Debug.Log("í«ê’");
            enemyManager.SetTarget(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //enemyManager.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);
    }
}
