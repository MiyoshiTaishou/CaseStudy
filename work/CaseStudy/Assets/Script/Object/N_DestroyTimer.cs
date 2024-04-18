using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定時間でオブジェクトが削除される

public class N_DestroyTimer : MonoBehaviour
{

    [Header("削除されるまでの時間"), SerializeField]
    private float fDestroyTimer = 5.0f;

    [Header("時間で削除される？"), SerializeField]
    private bool isDestroy = false;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float fElapsedTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (isDestroy)
        {
            if (fElapsedTime >= fDestroyTimer)
            {
                Destroy(this.gameObject);
            }
            fElapsedTime += Time.deltaTime;
        }
    }

    public void SetBoolDestroy(bool _truefalse)
    {
        isDestroy = _truefalse;
    }
}
