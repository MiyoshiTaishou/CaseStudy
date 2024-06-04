using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを揺らす処理
/// </summary>
public class M_CameraShake : MonoBehaviour
{
    [Header("揺らす強さ"), SerializeField]
    private float m_Power = 1.0f;

    [Header("揺らす時間"), SerializeField]
    private float m_TimeLimit = 1.0f;
   
    /// <summary>
    /// 他のスクリプトで呼ぶと揺れる
    /// </summary>
    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    /// <summary>
    /// 揺れる処理
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShake()
    {
        //揺れる前のカメラの座標
        Vector3 initPos = transform.position;

        //経過時間計測
        float countTime = 0.0f;

        //揺れる時間の間処理する
        while (countTime < m_TimeLimit)
        {
            //カメラの位置をランダムで決める
            float camX = initPos.x + Random.Range(-m_Power, m_Power);
            float camY = initPos.y + Random.Range(-m_Power, m_Power);
            transform.position = new Vector3(camX, camY, initPos.z);

            countTime += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;

    }
}
