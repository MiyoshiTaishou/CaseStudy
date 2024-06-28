using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���U���g�V�[���ŏ��ҏ��w�i�̉摜��ς���X�N���v�g
/// </summary>
public class M_ResultImageChange : MonoBehaviour
{
    [Header("�ύX�������w�i�̉摜"), SerializeField]
    private Sprite[] imagesBG;

    [Header("�ύX���������ҏ�̉摜"), SerializeField]
    private Sprite[] imagesLetter;

    [Header("�ύX���������I�u�W�F�N�g�w�i"), SerializeField]
    private GameObject BG;

    [Header("�ύX���������I�u�W�F�N�g���ҏ�"), SerializeField]
    private GameObject Letter;

    // Start is called before the first frame update
    void Start()
    {
        BG.GetComponent<Image>().sprite = imagesBG[M_GameMaster.GetSceneIndex()];
        Letter.GetComponent<Image>().sprite = imagesLetter[M_GameMaster.GetCurrentIndex() + M_GameMaster.GetSceneIndex() * 6];
    }    
}
