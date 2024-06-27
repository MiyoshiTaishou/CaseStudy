using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_PauseButtonSelect : MonoBehaviour
{
    [Header("シーンごとに表示する画像")]
    public GameObject[] sceneImages; // シーンごとのボタンのリスト

    private int currentIndex = 0; // 現在選択中のボタンのインデックス

    [Header("トランジション"), SerializeField]
    private GameObject tran;

    private bool stickMoved = false; // スティックが動いたかどうかのフラグ

    [Header("オンオフの画像を入れる"), SerializeField]
    private Sprite[] OnOff;

    bool isOnce = false;

    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        //M_GameMaster.SetGameClear(false);
        //M_GameMaster.SetGamePlay(false);

        sceneImages[0].GetComponent<Image>().sprite = OnOff[1];

        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (M_GameMaster.GetGamePlay())
        {
            Player.GetComponent<N_ProjecterSympathy>().enabled = true;
            return;
        }

        //int count = 0;

        //foreach (var scene in sceneImages)
        //{
        //    if (currentIndex != count)
        //    {
        //        scene.GetComponent<M_ImageEasing>().Resset();
        //    }
        //    count++;
        //}

        Player.GetComponent<N_ProjecterSympathy>().enabled = false;

        float verticlainput = Input.GetAxisRaw("Vertical"); // 縦方向のスティック入力を取得

        if (verticlainput > 0.5f && !stickMoved)
        {
            Debug.Log("傾けた");
            SelectImage(currentIndex - 1); // 右方向に移動したら次のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (verticlainput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // 左方向に移動したら前のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (verticlainput == 0)
        {
            stickMoved = false; // スティックが中立位置に戻ったらフラグをリセット
        }

        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // ボタンを押すボタンが押されたら選択中のボタンを押す
        }

        if (Input.GetButtonDown("Cancel"))
        {
            this.GetComponent<M_Pause>().PauseOnOff();
        }
    }

    void SelectImage(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0)
        {
            return;
        }
        else if (newIndex >= sceneImages.Length)
        {
            return;
        }

        // 現在のボタンの選択を解除
        //sceneImages[currentIndex].GetComponent<M_ImageEasing>().Resset();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2];

        // 新しいボタンを選択
        currentIndex = newIndex;

        //sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        //sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2 + 1];
    }

    void PressSelectedButton()
    {
        switch (currentIndex)
        {
            case 0:
                this.GetComponent<M_Pause>().PauseOnOff();
                break;

            case 1:
                tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
                tran.GetComponent<M_TransitionList>().SetRe(true);
                tran.GetComponent<M_TransitionList>().LoadScene();
                break;
            case 2:
                tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);                
                tran.GetComponent<M_TransitionList>().LoadScene();
                break;
            case 3:
                //オプションのやつ
                break;
        }      
    }
}
