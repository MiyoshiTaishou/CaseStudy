using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_SetEnemyCount : MonoBehaviour
{
    private GameObject EnemyObj;
    private S_EnemyBall EnemyBall;
    private K_DisplaySuuji DisplaySuuji;

    // Start is called before the first frame update
    void Start()
    {
        //親取得
        EnemyObj = transform.parent.gameObject;
        EnemyBall = EnemyObj.GetComponent<S_EnemyBall>();
        DisplaySuuji = this.GetComponent<K_DisplaySuuji>();
        if(!EnemyBall||!DisplaySuuji)
        {
            Debug.Log("このオブジェクト怪しいぞ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplaySuuji.SetNum(EnemyBall.GetStickCount());
    }
}
