using UnityEngine;
using UnityEngine.UI;

public class M_ImageSelect : MonoBehaviour
{
    public GameObject[] images; // UIボタンの配列
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private bool stickMoved = false; // スティックが動いたかどうかのフラグ

    private bool once = false;

    [Header("トランジション"), SerializeField]
    private GameObject tran;

    private void Start()
    {        
        //images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
    }

    void Update()
    {
        if(!once)
        {
            images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            once = true;
        }

        float verticalInput = Input.GetAxisRaw("Horizontal"); // 横方向のスティック入力を取得

        if (verticalInput > 0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // 右方向に移動したら前のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (verticalInput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex - 1); // 左方向に移動したら次のボタンを選択
            stickMoved = true; // スティックが動いたフラグを立てる
        }
        else if (verticalInput == 0)
        {
            stickMoved = false; // スティックが中立位置に戻ったらフラグをリセット
        }

        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // ボタンを押すボタンが押されたら選択中のボタンを押す
        }

        //選択されているもの以外
        int count = 0;
        foreach (var image in images)
        {
            if(count != currentIndex)
            {
                image.GetComponent<M_ImageEasing>().Resset();
            }
            count++;
        }
    }

    void SelectImage(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0)
        {
            newIndex = images.Length - 1;
        }
        else if (newIndex >= images.Length)
        {
            newIndex = 0;
        }

        // 現在のボタンの選択を解除
        images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // 新しいボタンを選択
        currentIndex = newIndex;
        images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();       
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
