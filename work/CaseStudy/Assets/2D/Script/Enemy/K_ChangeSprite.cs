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

    private Animator animator;

    private void Start()
    {
        string strEnemySprites = EnemyObjNormal.name;
        EnemySpriteNormal = this.transform.Find(strEnemySprites).gameObject;
        if(EnemyObjRolling)
        {
            EnemySpriteRolling = Instantiate(EnemyObjRolling, gameObject.transform.position, Quaternion.identity);
            EnemySpriteRolling.SetActive(false);
            EnemySpriteRolling.transform.parent = this.transform;
        }
        EnemyBall = GetComponent<S_EnemyBall>();

        animator = transform.GetChild(2).GetComponent<Animator>();
    }

    private void Update()
    {
        bool IsRolling = EnemyBall.GetisBall();
        bool isHitStop = EnemyBall.GetisHitStop();
        bool isMoving = gameObject.GetComponent<Rigidbody2D>().velocity.x >= 1.0f;

        // ヒットストップ中はvelocityが0になるので強制的に
        if (isHitStop)
        {
            isMoving = true;
        }
        if (IsRolling)
        {
            // 動いているなら
            if (isMoving)
            {
                EnemySpriteNormal.SetActive(false);
                EnemySpriteRolling.SetActive(true);
                Vector3 pos = this.transform.position;
                pos.y += 0.3f;
                EnemySpriteRolling.transform.position = pos;
                animator.SetBool("rollover", false);

            }
            // 転がっていない
            else
            {
                EnemyBall.SetEffectActivation(false);
                EnemySpriteNormal.SetActive(true);
                EnemySpriteNormal.transform.position = new Vector3(transform.position.x, EnemySpriteNormal.transform.position.y, transform.position.z);
                EnemySpriteRolling.SetActive(false);
                animator.SetBool("rollover", true);
            }
        }
        else
        {
            EnemySpriteNormal.SetActive(true);
            EnemySpriteNormal.transform.position = new Vector3(transform.position.x,EnemySpriteNormal.transform.position.y,transform.position.z);
            EnemySpriteRolling.SetActive(false);
        }
    }
}
