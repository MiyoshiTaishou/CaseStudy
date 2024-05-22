using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Enem_RollingEffect : MonoBehaviour
{
    private S_EnemyBall enemyball;
    void Start()
    {
        this.gameObject.SetActive(false);
        GameObject oya = transform.parent.gameObject;
        enemyball = oya.GetComponent<S_EnemyBall>();
        Debug.Log(oya.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyball.GetStickCount()>1)
        {
            this.gameObject.SetActive(true);
        }
    }
}
