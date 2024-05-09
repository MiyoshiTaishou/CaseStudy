using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_SceneDebug : MonoBehaviour
{
    [Header("ロードしたいシーン名"), SerializeField]
    private List<string> m_SceneText = new List<string>();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(m_SceneText[0]);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(m_SceneText[1]);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(m_SceneText[2]);
        }
    }
}
