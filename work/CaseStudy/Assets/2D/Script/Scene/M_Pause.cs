using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_Pause : MonoBehaviour
{
    public bool isPaused = false;

    /// <summary>
    /// ポーズを押せるかどうか
    /// </summary>
    private bool isStart = false;

    [Header("ポーズ中に出すものを全てここに入れる"), SerializeField]
    private List<GameObject> m_PauseList = new List<GameObject>();

    private GameObject sound;
    private GameObject player;

    private void Start()
    {
        sound = GameObject.Find("Sound");
        player = GameObject.Find("PlayerRespawn");
    }

    // Update is called once per frame
    void Update()
    {
        //クリア時は押せないようにする
        if(M_GameMaster.GetGameClear() || player.GetComponent<S_Respawn>().GetRespawn())
        {            
            return;
        }

        //Debug.Log("ポーズスクリプト" + M_GameMaster.GetGamePlay());
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

            sound.GetComponent<N_PlaySound>().PlaySound(N_PlaySound.SEName.OpenLetter);
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

        sound.GetComponent<N_PlaySound>().PlaySound(N_PlaySound.SEName.OpenLetter);

        return true;
    }
}
