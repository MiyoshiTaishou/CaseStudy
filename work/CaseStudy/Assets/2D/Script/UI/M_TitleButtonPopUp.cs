using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TitleButtonPopUp : MonoBehaviour
{
    [Header("�Z���N�g�X�N���v�g"),SerializeField]
    private GameObject m_Title;

    [Header("�\������I�u�W�F�N�g"), SerializeField]
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
