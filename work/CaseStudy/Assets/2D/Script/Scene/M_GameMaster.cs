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
    /// �v���C���[�̎u�]��
    /// </summary>
    private static int nDethCount = 0;
    
    public static bool GetGamePlay() { return isGamePlay; }
    public static void SetGamePlay(bool gamePlay) {  isGamePlay = gamePlay; }

    public static int GetDethCount() {  return nDethCount; }

    public static void SetDethCount(int _deth) { nDethCount = _deth; }
}
