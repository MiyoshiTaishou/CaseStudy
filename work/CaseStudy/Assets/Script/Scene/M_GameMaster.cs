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
    
    public static bool GetGamePlay() { return isGamePlay; }
    public static void SetGamePlay(bool gamePlay) {  isGamePlay = gamePlay; }
}
