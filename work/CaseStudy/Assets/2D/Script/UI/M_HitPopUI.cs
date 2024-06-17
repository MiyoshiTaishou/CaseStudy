using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI : MonoBehaviour
{
    [Header("�\���������I�u�W�F�N�g������"),SerializeField]
    private GameObject hitPopUI;

    [Header("��ʑJ�ڂ̎���"), SerializeField]
    private float waitTime = 2.0f;

    /// <summary>
    /// �N���AUI
    /// </summary>
    GameObject ClearUI;

    private void Start()
    {
        hitPopUI.SetActive(false);

        ClearUI = GameObject.Find("TransitionClear");
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

            StartCoroutine(WaitAndLoadScene());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitPopUI.GetComponent <M_ObjectEasing>().SetReverse(true);
            hitPopUI.GetComponent<M_ObjectEasing>().EasingOnOff();
        }
    }

    private IEnumerator WaitAndLoadScene()
    {
        // �w��b���ҋ@
        yield return new WaitForSeconds(waitTime);

        // �V�[�������[�h
        ClearUI.GetComponent<M_Transition>().LoadScene();
    }
}
