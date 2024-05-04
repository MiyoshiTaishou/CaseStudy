using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �w�莞�ԂŃI�u�W�F�N�g���폜�����

public class N_DestroyTimer : MonoBehaviour
{

    [Header("�폜�����܂ł̎���"), SerializeField]
    private float fDestroyTimer = 5.0f;

    [Header("���Ԃō폜�����H"), SerializeField]
    private bool isDestroy = false;

    /// <summary>
    /// �o�ߎ���
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
