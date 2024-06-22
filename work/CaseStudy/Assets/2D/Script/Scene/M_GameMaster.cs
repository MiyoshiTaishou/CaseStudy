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
    public static void SetGamePlay(bool gamePlay) { isGamePlay = gamePlay; }

    /// <summary>
    /// �N���A����
    /// </summary>
    private static bool isGameClear = false;

    public static bool GetGameClear() { return isGameClear; }
    public static void SetGameClear(bool gameClear) { isGameClear = gameClear; }

    /// <summary>
    /// ���S�J�E���g
    /// </summary>
    private static int nDethCount = 0;

    public static int GetDethCount() { return nDethCount; }

    public static void SetDethCount(int _count) { nDethCount = _count; }

    /// <summary>
    /// �G��S�ē|��
    /// </summary>
    private static bool isEnemyAllKill = false;

    public static bool GetEnemyAllKill() { return isEnemyAllKill; }
    public static void SetEneymAllKill(bool kill) { isEnemyAllKill = kill; }

    /// <summary>
    /// �O�̃V�[��
    /// </summary>
    private static string afterScene;        
  
    public static string GetAfetrScene() { return afterScene; }
    public static void SetAferScene(string scene) { afterScene = scene; }

    /// <summary>
    /// �Z���N�g�V�[���̍��W�ۑ�
    /// </summary>
    private static Vector2 selectPos = Vector2.zero;

    public static Vector2 GetSelectPos() { return selectPos; }

    public static void SetSelectPos(Vector2 _pos) { selectPos = _pos; }

    private static int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private static int sceneIndex = 0; // ���ݑI�𒆂̃V�[���̃C���f�b�N�X
    private static int slideIndex = 4; // �X���C�h�̃C���f�b�N�X

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
