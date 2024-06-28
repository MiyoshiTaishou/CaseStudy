using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SceneList
{
    public List<string> scenes;
}


public class M_TransitionList : MonoBehaviour
{
    private Image image;
    private Material material;

    [Header("イージング関数"), SerializeField]
    private M_Easing.Ease ease;

    [Header("フェードインアウト"), SerializeField]
    private bool isInOut = true;

    /// <summary>
    /// シーン遷移先の名前
    /// </summary>
    [SerializeField]
    public List<SceneList> sceneName = new List<SceneList>(); // シリアライズ可能にし、初期化

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

    private int sceneIndex = 0;

    public void SetSceneIndex(int _index) { sceneIndex = _index; }

    private Animator Animator;

    private bool isRe = false;

    public void SetRe(bool _isRe) {  isRe = _isRe; }

    public N_PlaySound sound;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();

        sceneIndex = M_GameMaster.GetSceneIndex();
        index = M_GameMaster.GetCurrentIndex();
    }

    // Update is called once per frame
    void Update()
    {

        if (isTransition)
        {
            if (pause)
            {
                pause.GetComponent<M_Pause>().enabled = false;
            }

            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);

            // アニメーションが終了したかどうかをチェック
            if (stateInfo.normalizedTime >= 1.0f && stateInfo.IsName("SceneEnd"))
            {
                if(isRe)
                {
                    SceneManager.LoadScene(M_GameMaster.GetAfetrScene());
                    return;
                }

                if(sceneName.Count == 1)
                {
                    sceneIndex = 0;
                }
                SceneManager.LoadScene(sceneName[sceneIndex].scenes[index]);
            }           
        }       
    }

    public void LoadScene()
    {
        if (!once)
        {
            isTransition = true;
            sound.PlaySound(N_PlaySound.SEName.Transition);

            M_GameMaster.SetGamePlay(true);

            fTime = 0.0f;

            once = true;

            Animator.SetBool("End", true);
        }
    }

    public string GetScene()
    {
        return sceneName[sceneIndex].scenes[index];
    }
}
