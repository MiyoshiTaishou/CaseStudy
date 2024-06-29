using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TitleButtonPopUp : MonoBehaviour
{
    [Header("セレクトスクリプト"),SerializeField]
    private GameObject m_Title;

    [Header("表示するオブジェクト"), SerializeField]
    private GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        m_Title.GetComponent<M_TitleSelect>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("SympathyButton"))
        {
            foreach(var button in buttons)
            {
                button.SetActive(true);
            }

            m_Title.GetComponent<M_TitleSelect>().enabled = true;

            this.gameObject.SetActive(false);
        }
    }
}
