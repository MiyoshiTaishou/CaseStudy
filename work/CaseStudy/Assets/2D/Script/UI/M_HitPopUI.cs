using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI : MonoBehaviour
{
    [Header("�\���������I�u�W�F�N�g������"),SerializeField]
    private GameObject hitPopUI;

    [Header("��ʑJ�ڂ̎���"), SerializeField]
    private float waitTime = 2.0f;

    [Header("���ꂽ������邩"), SerializeField]
    private bool IsDisappear = true;

    /// <summary>
    /// �N���AUI
    /// </summary>
    GameObject ClearUI;

    private void Start()
    {
        hitPopUI.SetActive(false);

        ClearUI = GameObject.Find("SceneEffect_Panel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitPopUI.GetComponent<M_ObjectEasing>().SetReverse(false);
            hitPopUI.SetActive(true);
            hitPopUI.GetComponent<M_ObjectEasing>().EasingOnOff();

            M_GameMaster.SetGamePlay(false);
            M_GameMaster.SetGameClear(true);
            hitPopUI.GetComponent<LoadScript>().LoadScene();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player") && IsDisappear)
        //{
        //    hitPopUI.GetComponent <M_ObjectEasing>().SetReverse(true);
        //    hitPopUI.GetComponent<M_ObjectEasing>().EasingOnOff();
        //    hitPopUI.GetComponent<LoadScript>().LoadScene();
        //}
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
