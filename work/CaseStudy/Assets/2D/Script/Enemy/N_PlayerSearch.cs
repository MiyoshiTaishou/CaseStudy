using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch : MonoBehaviour
{
    private N_EnemyManager enemyManager;
    private SEnemyMove enemyMove;

    private bool isSearch = false;
    public bool GetIsSearch() { return isSearch; }

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = this.gameObject.GetComponent<SEnemyMove>();
        enemyManager = enemyMove.GetManager();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyManager == null)
        {
            enemyManager = enemyMove.GetManager();
        }

        //ã èÛë‘Ç»ÇÁí«ê’èÛë‘Çâèú
        if(GetComponent<S_EnemyBall>().GetisBall())
        {
            isSearch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("î≠å©");
                enemyManager.SetTarget(collision.gameObject);
                isSearch = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("í«ê’");
                enemyManager.SetTarget(collision.gameObject);

                isSearch = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //enemyManager.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);
        isSearch = false;
    }
}
