using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI : MonoBehaviour
{
    [Header("表示したいオブジェクトを入れる"),SerializeField]
    private GameObject hitPopUI;

    [Header("画面遷移の時間"), SerializeField]
    private float waitTime = 2.0f;

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
        // 指定秒数待機
        yield return new WaitForSeconds(waitTime);

        // シーンをロード
        ClearUI.GetComponent<M_Transition>().LoadScene();
    }
}
