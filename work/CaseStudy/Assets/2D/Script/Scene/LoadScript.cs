using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{
    [Header("��ʑJ�ڂ̎���"), SerializeField]
    private float waitTime = 2.0f;

    /// <summary>
    /// �N���AUI
    /// </summary>
    GameObject ClearUI;

    // Start is called before the first frame update
    void Start()
    {
        ClearUI = GameObject.Find("SceneEffect_Panel");
    }   

    public void LoadScene()
    {
        StartCoroutine(WaitAndLoadScene());
        GetComponent<M_RandomSEPlay>().PlayRandomSoundEffect();
    }

    private IEnumerator WaitAndLoadScene()
    {
        Debug.Log("���[�h�҂��ł�");

        // �w��b���ҋ@
        yield return new WaitForSeconds(waitTime);

        Debug.Log("���[�h����");

        // �V�[�������[�h
        ClearUI.GetComponent<M_TransitionList>().SetIndex(0);
        ClearUI.GetComponent<M_TransitionList>().LoadScene();
    }
}
