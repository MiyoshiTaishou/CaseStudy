using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���̊Ǘ�
/// �|�[�Y�����̔��ʂɎg��
/// </summary>
static public class M_GameMaster
{
    /// <summary>
    /// �Q�[�����f
    /// </summary>
    private static bool isGamePlay = true;

    /// <summary>
    /// �N���A����
    /// </summary>
    private static bool isGameClear = false;

    /// <summary>
    /// ���S�J�E���g
    /// </summary>
    private static int nDethCount = 0;

    /// <summary>
    /// �G��S�ē|��
    /// </summary>
    private static bool isEnemyAllKill = false;
    
    public static bool GetGamePlay() { return isGamePlay; }
    public static void SetGamePlay(bool gamePlay) {  isGamePlay = gamePlay; }

    public static bool GetGameClear() { return isGameClear; }
    public static void SetGameClear(bool gameClear) { isGameClear = gameClear; }

    public static int GetDethCount() {  return nDethCount; }

    public static void SetDethCount(int _count) { nDethCount = _count; }

    public static bool GetEnemyAllKill() { return isEnemyAllKill; }
    public static void SetEneymAllKill(bool kill) { isEnemyAllKill = kill; }

    public static void RessetScore()
    {
        nDethCount = 0;
        isEnemyAllKill = false;
    }
}
