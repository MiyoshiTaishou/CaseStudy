using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_TransitionList : MonoBehaviour
{
    private Image image;
    private Material material;

    [Header("イージング関数"),SerializeField]
    private M_Easing.Ease ease;

    [Header("フェードインアウト"), SerializeField]
    private bool isInOut = true;

    /// <summary>
    /// シーン遷移先の名前
    /// </summary>
    public string[] sceneName;

    /// <summary>
    /// シーン遷移開始
    /// </summary>
    private bool isTransition;

    /// <summary>
    /// 時間計測
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// 現在のトランジションの進行度
    /// </summary>
    private float val = 1.0f;

    /// <summary>
    /// 一度だけ実行する
    /// </summary>
    private bool once = false;

    [Header("Animation Duration")]
    [SerializeField] private float duration = 1.0f;

    private GameObject pause;

    private int index = 0;

    public void SetIndex(int _index) { index = _index; }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        material = image.material;

        if(isInOut)
        {
            val = -1.0f;
        }
        else
        {
            val = 1.0f;
        }

        this.material.SetFloat("_Val", val);

        pause = GameObject.Find("PauseCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if(isTransition)
        {
            if(pause)
            {
                pause.GetComponent<M_Pause>().enabled = false;
            }

            fTime += Time.deltaTime;

            //関数登録
            var func = M_Easing.GetEasingMethod(ease);

            float t = Mathf.Clamp01(fTime / duration);

            // 値を1から-1に減少させる
            val = 1.0f - func(t) * 2f;

            this.material.SetFloat("_Val", val);

            if (val <= -0.9f)
            {
                SceneManager.LoadScene(sceneName[index]);
            }
                     
        }
       

        if (isInOut)
        {
            fTime += Time.deltaTime;

            //関数登録
            var func = M_Easing.GetEasingMethod(ease);

            float t = Mathf.Clamp01(fTime / duration);

            val = func(t) * 2f;

            this.material.SetFloat("_Val", val);  
            
            if(val >= 1.0f)
            {
                isInOut = false;
            }
        }
    }    

    public void LoadScene()
    {
        if(!once)
        {
            isTransition = true;
            M_GameMaster.SetGamePlay(true);

            fTime = 0.0f;

            once = true;
        }       
    }
}
