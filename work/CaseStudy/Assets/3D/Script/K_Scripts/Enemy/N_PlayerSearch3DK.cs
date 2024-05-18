using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch3DK : MonoBehaviour
{
    private N_EnemyManager3DK enemyManager;
    private SEnemyMove3DK enemyMove;

    private bool isSearch = false;
    public bool GetIsSearch() { return isSearch; }

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = this.gameObject.GetComponent<SEnemyMove3DK>();
        enemyManager = enemyMove.GetManager();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyManager == null)
        {
            enemyManager = enemyMove.GetManager();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("����");
                enemyManager.SetTarget(collision.gameObject);

                isSearch = true;
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("�ǐ�");
                enemyManager.SetTarget(collision.gameObject);

                isSearch = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //enemyManager.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);

        isSearch = false;
    }
}