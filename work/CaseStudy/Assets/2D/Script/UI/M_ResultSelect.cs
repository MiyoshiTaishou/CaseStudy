using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_ResultSelect : MonoBehaviour
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
  
    // Start is called before the first frame update
    void Start()
    {       
        M_GameMaster.SetGameClear(false);
        M_GameMaster.SetGamePlay(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce)
        {
            sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();

            isOnce = true;
        }

        int count = 0;

        foreach (var scene in sceneImages)
        {
            if(currentIndex != count)
            {
                scene.GetComponent<M_ImageEasing>().Resset();
            }
            count++;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal"); // 横方向のスティック入力を取得
     
        if (horizontalInput > 0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // 右方向に移動したら次のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (horizontalInput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex - 1); // 左方向に移動したら前のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (horizontalInput == 0)
        {
            stickMoved = false; // スティックが中立位置に戻ったらフラグをリセット
        }

        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // ボタンを押すボタンが押されたら選択中のボタンを押す
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
        sceneImages[currentIndex].GetComponent<M_ImageEasing>().Resset();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2];

        // 新しいボタンを選択
        currentIndex = newIndex;

        sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2 + 1];
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);

        if (currentIndex == 0)
        {
            tran.GetComponent<M_TransitionList>().SetRe(true);
        }

        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
