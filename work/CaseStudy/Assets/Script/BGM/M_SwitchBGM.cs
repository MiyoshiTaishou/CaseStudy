using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_SwitchBGM : MonoBehaviour
{
    [Header("通常再生する曲"),SerializeField]
    private AudioSource bgmSource1; // オーディオソース1

    [Header("発見時再生する曲"),SerializeField]
    private AudioSource bgmSource2; // オーディオソース2

    [Header("曲のミックス"), SerializeField,Range(0,1)]
    private float fRate = 0.0f; // フェード時間                                 

    [Header("曲の切り替え速度"), SerializeField]
    private float fSpeed = 1.0f; // フェード時間
                                 // 
    [Header("音量"), SerializeField,]
    private float fVolume = 0.1f;

    /// <summary>
    /// 時間計測
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// BGM1を再生中かどうか
    /// </summary>
    private bool isPlayingBGM1 = true;

    private void Start()
    {
        bgmSource1.Play();                
    }

    private void Update()
    {
        //if(bgmSource1.volume <= 0.0f)
        //{
        //    bgmSource1.Stop();
        //}       

        //if (bgmSource2.volume <= 0.0f)
        //{
        //    bgmSource2.Stop();
        //}       

        //BGMのミックス
        bgmSource1.volume = (1 - fRate) * fVolume;
        bgmSource2.volume = fRate * fVolume;
    }

    public void ChangeBGM()
    {
        if(isPlayingBGM1)
        {
            fRate = 1.0f;
            bgmSource2.Play();
            bgmSource1.Stop();
        }
        else
        {
            fRate = 0.0f;
            bgmSource1.Play();
            bgmSource2.Stop();
        }

        isPlayingBGM1 = !isPlayingBGM1;
    }

    /// <summary>
    /// 曲を切り替えるメソッド
    /// </summary>
    public void SwitchBGM()
    {
        StartCoroutine(Crossfade());
    }

    // クロスフェード処理
    IEnumerator Crossfade()
    {
        // フェードアウト
        if (isPlayingBGM1)
        {
            float currentTime = 0.0f;
            while (currentTime < fSpeed)
            {
                currentTime += Time.deltaTime;
                fRate = Mathf.Lerp(0, 1, currentTime / fSpeed);
                yield return null;
            }
        }
        else // フェードイン
        {
            float currentTime = 0.0f;           
            while (currentTime < fSpeed)
            {
                currentTime += Time.deltaTime;
                fRate = Mathf.Lerp(1, 0, currentTime / fSpeed);
                yield return null;
            }
        }

        // フラグを反転させる
        isPlayingBGM1 = !isPlayingBGM1;
    }
}
