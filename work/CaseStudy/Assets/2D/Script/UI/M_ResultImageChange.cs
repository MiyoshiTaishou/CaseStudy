using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトシーンで招待状や背景の画像を変えるスクリプト
/// </summary>
public class M_ResultImageChange : MonoBehaviour
{
    [Header("変更したい背景の画像"), SerializeField]
    private Sprite[] imagesBG;

    [Header("変更したい招待状の画像"), SerializeField]
    private Sprite[] imagesLetter;

    [Header("変更させたいオブジェクト背景"), SerializeField]
    private GameObject BG;

    [Header("変更させたいオブジェクト招待状"), SerializeField]
    private GameObject Letter;

    // Start is called before the first frame update
    void Start()
    {
        BG.GetComponent<Image>().sprite = imagesBG[M_GameMaster.GetSceneIndex()];
        Letter.GetComponent<Image>().sprite = imagesLetter[M_GameMaster.GetCurrentIndex() + M_GameMaster.GetSceneIndex() * 6];
    }    
}
