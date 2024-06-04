using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J������h�炷����
/// </summary>
public class M_CameraShake : MonoBehaviour
{
    [Header("�h�炷����"), SerializeField]
    private float m_Power = 1.0f;

    [Header("�h�炷����"), SerializeField]
    private float m_TimeLimit = 1.0f;
   
    /// <summary>
    /// ���̃X�N���v�g�ŌĂԂƗh���
    /// </summary>
    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    /// <summary>
    /// �h��鏈��
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShake()
    {
        //�h���O�̃J�����̍��W
        Vector3 initPos = transform.position;

        //�o�ߎ��Ԍv��
        float countTime = 0.0f;

        //�h��鎞�Ԃ̊ԏ�������
        while (countTime < m_TimeLimit)
        {
            //�J�����̈ʒu�������_���Ō��߂�
            float camX = initPos.x + Random.Range(-m_Power, m_Power);
            float camY = initPos.y + Random.Range(-m_Power, m_Power);
            transform.position = new Vector3(camX, camY, initPos.z);

            countTime += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;

    }
}
