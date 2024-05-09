using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
public class M_ClearUI : MonoBehaviour
{
    /// <summary>
    /// �N���A���ǂ���
    /// </summary>
    [SerializeField]
    private bool isClear = false;

    /// <summary>
    /// �O�̃t���[���ŃC�[�W���O�����ǂ���
    /// </summary>
    private bool isEasingNow = false;

    /// <summary>
    /// �����邩�ǂ���
    /// </summary>
    private bool isStart = false;

    [Header("�o�����̂�S�Ă����ɓ����"), SerializeField]
    private List<GameObject> m_ClearList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        foreach (var item in m_ClearList)
        {
            if (item.GetComponent<M_ObjectEasing>())
            {
                isStart = item.GetComponent<M_ObjectEasing>().GetEasing();
            }
            else
            {
                isStart = item.GetComponent<M_ObjectEasing>().GetEasing();
            }

            if (isStart)
            {
                break;
            }
        }

        if(isClear)
        {
            foreach (var item in m_ClearList)
            {
                item.GetComponent<M_ObjectEasing>().SetReverse(false);
                item.GetComponent<M_ObjectEasing>().EasingOnOff();
            }

            M_GameMaster.SetGamePlay(isClear);
            isClear = false;
        }

        if (Input.GetButtonDown("Pause") && !isClear)
        {           
            foreach (var item in m_ClearList)
            {
                item.GetComponent<M_ObjectEasing>().SetReverse(true);                   
                item.GetComponent<M_ObjectEasing>().EasingOnOff();                              
            }            
        }
    }
}
