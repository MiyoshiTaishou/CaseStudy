using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定時間でオブジェクトが削除される

public class N_DestroyTimer : MonoBehaviour
{

    [Header("削除されるまでの時間"), SerializeField]
    private float fDestroyTimer = 5.0f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float fElapsedTime = 0.0f; 

    // Update is called once per frame
    void Update()
    {
        if(fElapsedTime >= fDestroyTimer)
        {
            Destroy(this.gameObject);
        }
        fElapsedTime += Time.deltaTime;
    }
}
