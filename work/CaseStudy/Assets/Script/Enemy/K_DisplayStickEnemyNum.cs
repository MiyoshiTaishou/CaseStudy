using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_DisplayStickEnemyNum : MonoBehaviour
{
    [Header("UIManager"), SerializeField]
    GameObject UIMAnager;


    private K_UIEnemyStikCount UIScript;

    // Start is called before the first frame update
    void Start()
    {
        UIScript = UIMAnager.GetComponent<K_UIEnemyStikCount>();
    }

    // Update is called once per frame
    void Update()
    {
        int StickEnemyNum = GetComponent<S_EnemyBall>().GetStickCount();

        if(StickEnemyNum!=0)
        {
             UIScript.SetEnemyNum(StickEnemyNum);
        }
    }
}
