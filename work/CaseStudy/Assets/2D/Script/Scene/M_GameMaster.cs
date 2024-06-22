using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの管理
/// ポーズ中等の判別に使う
/// </summary>
static public class M_GameMaster
{
    /// <summary>
    /// ゲーム中断
    /// </summary>
    private static bool isGamePlay = true;

    public static bool GetGamePlay() { return isGamePlay; }
    public static void SetGamePlay(bool gamePlay) { isGamePlay = gamePlay; }

    /// <summary>
    /// クリア判定
    /// </summary>
    private static bool isGameClear = false;

    public static bool GetGameClear() { return isGameClear; }
    public static void SetGameClear(bool gameClear) { isGameClear = gameClear; }

    /// <summary>
    /// 死亡カウント
    /// </summary>
    private static int nDethCount = 0;

    public static int GetDethCount() { return nDethCount; }

    public static void SetDethCount(int _count) { nDethCount = _count; }

    /// <summary>
    /// 敵を全て倒す
    /// </summary>
    private static bool isEnemyAllKill = false;

    public static bool GetEnemyAllKill() { return isEnemyAllKill; }
    public static void SetEneymAllKill(bool kill) { isEnemyAllKill = kill; }

    /// <summary>
    /// 前のシーン
    /// </summary>
    private static string afterScene;        
  
    public static string GetAfetrScene() { return afterScene; }
    public static void SetAferScene(string scene) { afterScene = scene; }

    /// <summary>
    /// セレクトシーンの座標保存
    /// </summary>
    private static Vector2 selectPos = Vector2.zero;

    public static Vector2 GetSelectPos() { return selectPos; }

    public static void SetSelectPos(Vector2 _pos) { selectPos = _pos; }

    private static int currentIndex = 0; // 現在選択中のボタンのインデックス
    private static int sceneIndex = 0; // 現在選択中のシーンのインデックス
    private static int slideIndex = 4; // スライドのインデックス

    public static int GetCurrentIndex() { return currentIndex; }
    public static int GetSceneIndex() { return sceneIndex; }
    public static int GetSlideIndex() { return slideIndex; }

    public static void SetCurrentIndex(int _index) { currentIndex = _index; }
    public static void SetSceneIndex(int _index) { sceneIndex = _index; }
    public static void SetSlideIndex(int _index) { slideIndex = _index; }

    public static void RessetScore()
    {
        nDethCount = 0;
        isEnemyAllKill = false;
    }
}
