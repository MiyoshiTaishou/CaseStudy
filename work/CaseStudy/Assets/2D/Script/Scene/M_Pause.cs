using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_Pause : MonoBehaviour
{
    public bool isPaused = false;

    /// <summary>
    /// �|�[�Y�������邩�ǂ���
    /// </summary>
    private bool isStart = false;

    [Header("�|�[�Y���ɏo�����̂�S�Ă����ɓ����"), SerializeField]
    private List<GameObject> m_PauseList = new List<GameObject>();
   
    // Update is called once per frame
    void Update()
    {
        //�N���A���͉����Ȃ��悤�ɂ���
        if(M_GameMaster.GetGameClear())
        {            
            return;
        }

        Debug.Log("�|�[�Y�X�N���v�g" + M_GameMaster.GetGamePlay());
        foreach (var item in m_PauseList)
        {
            if (item.GetComponent<M_ImageEasing>())
            {
                isStart = item.GetComponent<M_ImageEasing>().GetEasing();
            }
            else
            {
                isStart = item.GetComponent<M_ObjectEasing>().GetEasing();
            }

            if(isStart)
            {
                break;
            }
        }        

        if (Input.GetButtonDown("Pause") && !isStart && !m_PauseList[0].GetComponent<M_ImageEasing>().GetEasing())
        {
            //M_GameMaster.SetGamePlay(isPaused);           

            foreach (var item in m_PauseList)
            {
                if(item.GetComponent<M_ImageEasing>())
                {
                    if (isPaused)
                    {
                        item.GetComponent<M_ImageEasing>().SetReverse(true);
                    }
                    else
                    {
                        item.GetComponent<M_ImageEasing>().SetReverse(false);
                    }

                    item.GetComponent<M_ImageEasing>().EasingOnOff();
                }              
                else
                {
                    if (isPaused)
                    {
                        item.GetComponent<M_ObjectEasing>().SetReverse(true);
                    }
                    else
                    {
                        item.GetComponent<M_ObjectEasing>().SetReverse(false);
                    }

                    item.GetComponent<M_ObjectEasing>().EasingOnOff();
                }
            }

            isPaused = !isPaused;

            M_GameMaster.SetGamePlay(!isPaused);
        }
    }

    public bool PauseOnOff()
    {
        if(m_PauseList[0].GetComponent<M_ImageEasing>().GetEasing())
        {
            return false;
        }

        foreach (var item in m_PauseList)
        {
            if (item.GetComponent<M_ImageEasing>())
            {
                if (isPaused)
                {
                    item.GetComponent<M_ImageEasing>().SetReverse(true);
                }
                else
                {
                    item.GetComponent<M_ImageEasing>().SetReverse(false);
                }

                item.GetComponent<M_ImageEasing>().EasingOnOff();
            }
            else
            {
                if (isPaused)
                {
                    item.GetComponent<M_ObjectEasing>().SetReverse(true);
                }
                else
                {
                    item.GetComponent<M_ObjectEasing>().SetReverse(false);
                }

                item.GetComponent<M_ObjectEasing>().EasingOnOff();
            }
        }

        isPaused = !isPaused;

        M_GameMaster.SetGamePlay(!isPaused);

        return true;
    }
}
