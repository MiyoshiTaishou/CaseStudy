using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_BottomFloorController : MonoBehaviour
{
    public int iStageEnemyNum;
    private int iEnemyCount;

    void Start()
    {
        iEnemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(iEnemyCount==iStageEnemyNum)
        {
            Debug.Log("敵全員落ちた");
        }
    }

    // 当たり判定が発生した時に呼び出される関数
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 接触してきたオブジェクトが"Enemy"タグを持つ場合
        if (other.CompareTag("Enemy"))
        {
            // カウントを増やす
            iEnemyCount++;
        }
    }

    // 当たり判定が終了した時に呼び出される関数
    private void OnTriggerExit2D(Collider2D other)
    {
        // 接触していたオブジェクトが"Enemy"タグを持つ場合
        if (other.CompareTag("Enemy"))
        {
            // カウントを減らす
            iEnemyCount--;
        }
    }
}
