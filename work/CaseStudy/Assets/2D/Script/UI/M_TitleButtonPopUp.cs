using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_TitleButtonPopUp : MonoBehaviour
{
    [Header("セレクトスクリプト"),SerializeField]
    private GameObject m_Title;

    [Header("表示するオブジェクト"), SerializeField]
    private GameObject[] buttons;

    private Color col;
    private float time;
    private float fAlpha;

    // Start is called before the first frame update
    void Start()
    {
        m_Title.GetComponent<M_TitleSelect>().enabled = false;
        col = GetComponent<Image>().color;
        fAlpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        fAlpha =Mathf.PingPong(time, 1.0f);
        col.a = fAlpha;
        GetComponent<Image>().color = col;
        if (Input.GetButtonDown("SympathyButton"))
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
