using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI : MonoBehaviour
{
    [Header("表示したいオブジェクトを入れる"),SerializeField]
    private GameObject hitPopUI;

    [Header("画面遷移の時間"), SerializeField]
    private float waitTime = 2.0f;

    [Header("離れたら消えるか"), SerializeField]
    private bool IsDisappear = true;

    /// <summary>
    /// クリアUI
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
        Debug.Log("ロード待ちです");

        // 指定秒数待機
        yield return new WaitForSeconds(waitTime);

        Debug.Log("ロード完了");

        // シーンをロード
        ClearUI.GetComponent<M_TransitionList>().SetIndex(0);
        ClearUI.GetComponent<M_TransitionList>().LoadScene();
    }
}
