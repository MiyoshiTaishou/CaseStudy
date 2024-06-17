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

    [Header("シーンごとに表示する招待状画像")]
    public List<SceneImages> ChallengeImages; // シーンごとのボタンのリスト

    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private int sceneIndex = 0; // 現在選択中のシーンのインデックス
    private int slideIndex = 0; // スライドのインデックス
    private bool stickMoved = false; // スティックが動いたかどうかのフラグ
    private bool once = false;

    [Header("トランジション"), SerializeField]
    private GameObject tran;

    [Header("スライド"), SerializeField]
    private GameObject sla;

    // 元の位置を保存するための辞書
    private Dictionary<GameObject, int> originalSiblingIndices = new Dictionary<GameObject, int>();

    //招待状開いているか？
    private bool isChallenge = false;

    private void Start()
    {
        slideIndex = sceneImages.Count - 1;

        // 元の位置を保存
        foreach (var scene in sceneImages)
        {
            foreach (var image in scene.images)
            {
                originalSiblingIndices[image] = image.transform.GetSiblingIndex();
            }
        }
    }

    void Update()
    {
       if(isChallenge)
        {
            ChallengeUpdate();
        }
       else
        {
            SelectUpdate();
        }
    }

    void ChallengeUpdate()
    {
        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // ボタンを押すボタンが押されたら選択中のボタンを押す
        }

        if(Input.GetButtonDown("Cancel"))
        {
            Debug.Log("挑戦状閉じる");
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().SetReverse(true);
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();           
            isChallenge = false;
        }
    }

    void SelectUpdate()
    {
        if (!once)
        {
            sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            MoveToFront(sceneImages[sceneIndex].images[currentIndex]);
            once = true;
        }

        if (sla.GetComponent<M_SelectSlide>().GetIsSlide())
        {
            return;
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
           isChallenge = true;

            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().SetReverse(false);
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        }

        if (Input.GetButtonDown("RButton"))
        {
            if (slideIndex > 0)
            {
                sla.GetComponent<M_SelectSlide>().Sub();
                SelectScene(sceneIndex + 1); // Lボタンでシーンインデックスを減少
                slideIndex--;
            }
        }

        if (Input.GetButtonDown("LButton"))
        {
            if (slideIndex < sceneImages.Count - 1)
            {
                sla.GetComponent<M_SelectSlide>().Add();
                SelectScene(sceneIndex - 1); // Rボタンでシーンインデックスを増加
                slideIndex++;
            }
        }

        // 選択されているもの以外をリセット
        for (int i = 0; i < sceneImages.Count; i++)
        {
            for (int j = 0; j < sceneImages[i].images.Count; j++)
            {
                if (i != sceneIndex || j != currentIndex)
                {
                    sceneImages[i].images[j].GetComponent<M_ImageEasing>().Resset();
                    sceneImages[i].images[j].GetComponent<M_OutLine>().OutLineOff();
                    ResetPosition(sceneImages[i].images[j]); // 元の位置に戻す                    
                }
            }
        }

        Debug.Log(sceneIndex);
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
        ResetPosition(sceneImages[sceneIndex].images[currentIndex]); // 元の位置に戻す

        // 新しいボタンを選択
        currentIndex = newIndex;
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        MoveToFront(sceneImages[sceneIndex].images[currentIndex]);

        Debug.Log(sceneImages[sceneIndex].images[currentIndex].transform.GetSiblingIndex());
    }

    void SelectScene(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0 || newIndex >= sceneImages.Count)
        {
            return;
        }

        // 現在のシーンのボタンの選択を解除
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();
        ResetPosition(sceneImages[sceneIndex].images[currentIndex]); // 元の位置に戻す

        // 新しいシーンを選択
        sceneIndex = newIndex;
        currentIndex = 0; // 新しいシーンでは最初のボタンを選択
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        MoveToFront(sceneImages[sceneIndex].images[currentIndex]);

        tran.GetComponent<M_TransitionList>().SetSceneIndex(sceneIndex);
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }

    // オブジェクトを最前列に移動させるメソッド
    void MoveToFront(GameObject obj)
    {
        int count = obj.transform.parent.childCount; // 親の子オブジェクトの数を取得
        Debug.Log($"Moving {obj.name} to front. Parent has {count} children.");
        obj.transform.SetAsLastSibling(); // 最前列に移動
        Debug.Log($"{obj.name} new sibling index: {obj.transform.GetSiblingIndex()}");
    }

    // オブジェクトを元の位置に戻すメソッド
    void ResetPosition(GameObject obj)
    {
        if (originalSiblingIndices.ContainsKey(obj))
        {
            obj.transform.SetSiblingIndex(originalSiblingIndices[obj]);
            Debug.Log($"{obj.name} reset to original index: {originalSiblingIndices[obj]}");
        }
    }
}
