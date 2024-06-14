using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ResultScore : MonoBehaviour
{
    [Header("ƒXƒRƒA‰æ‘œ"), SerializeField]
    private GameObject[] m_Score;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in m_Score)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(M_GameMaster.GetDethCount() == 0)
        {
            m_Score[0].gameObject.SetActive(true);
        }
    }
}
