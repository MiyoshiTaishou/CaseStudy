using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI : MonoBehaviour
{
    [Header("表示したいオブジェクトを入れる"),SerializeField]
    private GameObject hitPopUI;

    /// <summary>
    /// クリアUI
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

            ClearUI.GetComponent<M_Transition>().LoadScene();
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
}
