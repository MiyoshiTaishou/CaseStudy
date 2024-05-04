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
            Debug.Log("�G�S��������");
        }
    }

    // �����蔻�肪�����������ɌĂяo�����֐�
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �ڐG���Ă����I�u�W�F�N�g��"Enemy"�^�O�����ꍇ
        if (other.CompareTag("Enemy"))
        {
            // �J�E���g�𑝂₷
            iEnemyCount++;
        }
    }

    // �����蔻�肪�I���������ɌĂяo�����֐�
    private void OnTriggerExit2D(Collider2D other)
    {
        // �ڐG���Ă����I�u�W�F�N�g��"Enemy"�^�O�����ꍇ
        if (other.CompareTag("Enemy"))
        {
            // �J�E���g�����炷
            iEnemyCount--;
        }
    }
}
