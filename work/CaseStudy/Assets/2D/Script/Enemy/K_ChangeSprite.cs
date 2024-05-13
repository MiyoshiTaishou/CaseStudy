using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ChangeSprite : MonoBehaviour
{
    [Header("敵スプライト(通常時)"), SerializeField]
    private GameObject EnemyObjNormal;

    [Header("敵スプライト(転がり時)"), SerializeField]
    private GameObject EnemyObjRolling;

    //このオブジェクトにくっついている敵スプライト(通常時)
    private GameObject EnemySpriteNormal;

    //転がり時の敵スプライト(転がり時)
    private GameObject EnemySpriteRolling;

    private S_EnemyBall EnemyBall;

    private void Start()
    {
        string strEnemySprites = EnemyObjNormal.name;
        EnemySpriteNormal = this.transform.Find(strEnemySprites).gameObject;

        EnemySpriteRolling= Instantiate(EnemyObjRolling, transform.position, Quaternion.identity);
        EnemySpriteRolling.SetActive(false);
        EnemySpriteRolling.transform.parent = this.transform;

        EnemyBall = GetComponent<S_EnemyBall>();
    }

    private void Update()
    {
        bool IsRolling = EnemyBall.GetisPushing();
        if (IsRolling)
        {
            EnemySpriteNormal.SetActive(false);
            EnemySpriteRolling.SetActive(true);
            EnemySpriteRolling.transform.position = this.transform.position;
        }
        else
        {
            EnemySpriteNormal.SetActive(true);
            EnemySpriteNormal.transform.position = this.transform.position;
            EnemySpriteRolling.SetActive(false);
        }
    }
}
