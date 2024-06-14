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

    /// <summary>
    /// 死亡カウント
    /// </summary>
    private static int nDethCount = 0;
    
    public static bool GetGamePlay() { return isGamePlay; }
    public static void SetGamePlay(bool gamePlay) {  isGamePlay = gamePlay; }

    public static int GetDethCount() {  return nDethCount; }

    public static void SetDethCount(int _count) { nDethCount = _count; }
}
