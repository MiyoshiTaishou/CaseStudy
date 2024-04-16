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

    //このオブジェクトにくっついている敵スプライト(転がり時)
    private GameObject EnemySpriteRolling;

    private S_EnemyBall EnemyBall;

    private void Start()
    {
        string strEnemySprites = EnemyObjNormal.name;
        EnemySpriteNormal = this.transform.Find(strEnemySprites).gameObject;

        strEnemySprites = EnemyObjRolling.name;
        EnemyObjRolling = this.transform.Find(strEnemySprites).gameObject;
        EnemyObjRolling.SetActive(false);

        EnemyBall = GetComponent<S_EnemyBall>();
    }

    private void Update()
    {
        int StickEnemyNum = EnemyBall.GetStickCount();
        if(StickEnemyNum > 0)
        {
            EnemySpriteNormal.SetActive(false);
            EnemyObjRolling.SetActive(true);
        }
    }
}
