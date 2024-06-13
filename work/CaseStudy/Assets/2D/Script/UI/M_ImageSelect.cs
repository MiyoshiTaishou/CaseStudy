using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneImages
{
    public List<GameObject> images; // シーンごとのボタンのリスト
}

public class M_ImageSelect : MonoBehaviour
{
    [Header("シーンごとに表示する画像")]
    public List<SceneImages> sceneImages; // シーンごとのボタンのリスト
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private int sceneIndex = 0; // 現在選択中のシーンのインデックス
    private bool stickMoved = false; // スティックが動いたかどうかのフラグ

    private bool once = false;

    [Header("トランジション"), SerializeField]
    private GameObject tran;

    [Header("スライド"), SerializeField]
    private GameObject sla;

    private void Start()
    {
        // 初期化処理が必要な場合はここに記述します
    }

    void Update()
    {
        if (!once)
        {
            sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            once = true;
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

        if (Input.GetButtonDown("LButton"))
        {
            SelectScene(sceneIndex - 1); // Lボタンでシーンインデックスを減少
            sla.GetComponent<M_SelectSlide>().Sub();
        }

        if (Input.GetButtonDown("RButton"))
        {
            SelectScene(sceneIndex + 1); // Rボタンでシーンインデックスを増加
            sla.GetComponent<M_SelectSlide>().Add();
        }

        // 選択されているもの以外をリセット
        for (int i = 0; i < sceneImages.Count; i++)
        {
            for (int j = 0; j < sceneImages[i].images.Count; j++)
            {
                if (i != sceneIndex || j != currentIndex)
                {
                    Debug.Log(sceneImages[i].images[j]);
                    sceneImages[i].images[j].GetComponent<M_ImageEasing>().Resset();
                }
            }
        }
    }

    void SelectImage(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0)
        {
            newIndex = sceneImages[sceneIndex].images.Count - 1;
        }
        else if (newIndex >= sceneImages[sceneIndex].images.Count)
        {
            newIndex = 0;
        }

        // 現在のボタンの選択を解除
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // 新しいボタンを選択
        currentIndex = newIndex;
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
    }

    void SelectScene(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0)
        {
            newIndex = sceneImages.Count - 1;
        }
        else if (newIndex >= sceneImages.Count)
        {
            newIndex = 0;
        }

        // 現在のシーンのボタンの選択を解除
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // 新しいシーンを選択
        sceneIndex = newIndex;
        currentIndex = 0; // 新しいシーンでは最初のボタンを選択
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();

        tran.GetComponent<M_TransitionList>().SetSceneIndex(sceneIndex);
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
