using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyManagerManager : MonoBehaviour
{
    [Header("�X�e�[�W���̑S�Ă̓G�}�l�[�W���["), SerializeField]
    private N_EnemyManager[] ManagerList;

    [Header("�e�}�l�[�W���[�̗L��/����"), SerializeField]
    private bool[] managerStatus;

    void OnValidate()
    {
        if (ManagerList != null)
        {
            if (managerStatus == null || managerStatus.Length != ManagerList.Length)
            {
                managerStatus = new bool[ManagerList.Length];
            }
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < ManagerList.Length; i++) 
        {
            Debug.Log("�������ƕ�����");
            N_EnemyManager manager = ManagerList[i];
            manager.IsReflectionX=managerStatus[i];
        }
        Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
