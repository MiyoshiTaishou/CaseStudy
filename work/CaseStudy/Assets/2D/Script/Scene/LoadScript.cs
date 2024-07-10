using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{
    [Header("画面遷移の時間"), SerializeField]
    private float waitTime = 2.0f;

    /// <summary>
    /// クリアUI
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
        Debug.Log("ロード待ちです");

        // 指定秒数待機
        yield return new WaitForSeconds(waitTime);

        Debug.Log("ロード完了");

        // シーンをロード
        ClearUI.GetComponent<M_TransitionList>().SetIndex(0);
        ClearUI.GetComponent<M_TransitionList>().LoadScene();
    }
}
