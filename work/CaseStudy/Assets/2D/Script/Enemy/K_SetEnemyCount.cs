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
        //�e�擾
        EnemyObj = transform.parent.gameObject;
        EnemyBall = EnemyObj.GetComponent<S_EnemyBall>();
        DisplaySuuji = this.GetComponent<K_DisplaySuuji>();
        if(!EnemyBall||!DisplaySuuji)
        {
            Debug.Log("���̃I�u�W�F�N�g��������");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplaySuuji.SetNum(EnemyBall.GetStickCount());
    }
}
