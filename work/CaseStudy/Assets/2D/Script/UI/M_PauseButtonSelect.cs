#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M_PauseButtonSelect : MonoBehaviour
{
    public Button[] buttons; // UIボタンの配列
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private bool stickMoved = false; // スティックが動いたかどうかのフラグ
    private EventSystem eventSystem; // EventSystemの参照

    private void Start()
    {
        // EventSystemの参照を取得
        eventSystem = EventSystem.current;

        // 新しいボタンを選択       
        buttons[currentIndex].Select();
        buttons[currentIndex].image.color = Color.green; // 新しいボタンの色を変更
    }

    void Update()
    {
        if (M_GameMaster.GetGamePlay())
        {
            // ポーズ画面が表示されていない場合はEventSystemを無効化
            if (eventSystem)
            {
                eventSystem.enabled = false;
            }
            return;
        }
        else
        {
            // ポーズ画面が表示されている場合はEventSystemを有効化
            if (eventSystem)
            {
                eventSystem.enabled = true;
            }
        }

        Debug.Log("ポーズボタンセレクト" + M_GameMaster.GetGamePlay());
        if (!M_GameMaster.GetGamePlay())
        {
            float verticalInput = Input.GetAxisRaw("Horizontal"); // 横方向のスティック入力を取得

            if (verticalInput > 0.5f && !stickMoved)
            {
                SelectButton(currentIndex + 1); // 右方向に移動したら前のボタンを選択
                stickMoved = true; // スティックが動いたフラグを立てる
            }
            else if (verticalInput < -0.5f && !stickMoved)
            {
                SelectButton(currentIndex - 1); // 左方向に移動したら次のボタンを選択
                stickMoved = true; // スティックが動いたフラグを立てる
            }
            else if (verticalInput == 0)
            {
                stickMoved = false; // スティックが中立位置に戻ったらフラグをリセット
            }

            if (Input.GetButtonDown("SympathyButton"))
            {
                Debug.Log("ボタン押したよ");
                PressSelectedButton(); // ボタンを押すボタンが押されたら選択中のボタンを押す
            }
        }
    }

    void SelectButton(int newIndex)
    {
        // インデックスが範囲外の場合はインデックスをループさせる
        if (newIndex < 0)
        {
            newIndex = buttons.Length - 1;
        }
        else if (newIndex >= buttons.Length)
        {
            newIndex = 0;
        }

        // 現在のボタンの選択を解除
        buttons[currentIndex].image.color = Color.white; // 現在のボタンの色を元に戻す

        // 新しいボタンを選択
        currentIndex = newIndex;
        buttons[currentIndex].Select();
        buttons[currentIndex].image.color = Color.green; // 新しいボタンの色を変更
    }

    void PressSelectedButton()
    {
        buttons[currentIndex].onClick.Invoke(); // 選択中のボタンを押す
    }
}
