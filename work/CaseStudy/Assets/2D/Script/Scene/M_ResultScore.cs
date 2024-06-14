using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ResultScore : MonoBehaviour
{
    [Header("�X�R�A�摜"), SerializeField]
    private GameObject[] m_Score;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in m_Score)
        {
            item.gameObject.SetActive(false);
        }

        m_Score[1].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(M_GameMaster.GetDethCount() == 0)
        {
            m_Score[0].gameObject.SetActive(true);
        }        

        if(M_GameMaster.GetEnemyAllKill())
        {
            m_Score[2].gameObject.SetActive(true);
        }
    }
}
