using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class N_PostProcess : MonoBehaviour
{
    // カメラにエフェクトをつける

    [Header("警告エフェクト最大値"), SerializeField]
    private float MaxVignette = 0.5f;

    [Header("警告エフェクト最小値"), SerializeField]
    private float MinVignette = 0.0f;

    [Header("何秒かけて最大値まで行くか"), SerializeField]
    private float VignetteTime = 1.0f;

    private bool AddVignette = true;

    private Volume postVolume;

    private Vignette vignette;

    private int SearchManagerNum = 0;

    private bool isInit = false;

    public int GetSearchNum()
    {
        return SearchManagerNum;
    }

    public void SetSearchNum(int _num)
    {
        SearchManagerNum = _num;
    }

    private void Initialize()
    {
        postVolume = GetComponent<Volume>();

        // vignetteを取得
        postVolume.profile.TryGet(out vignette);

        vignette.intensity.value = 0.0f;

        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit)
        {
            Initialize();
        }

        Vignette();
    }

    public void AddSerachNum()
    {
        SearchManagerNum++;
    }

    public void SubSerachNum()
    {
        SearchManagerNum--;
    }

    private void Vignette()
    {
        if (SearchManagerNum > 0)
        {
            // 追跡状態なので画面にエフェクトをかける
            if (AddVignette)
            {
                // 徐々に画面にエフェクトをかけていく
                vignette.intensity.value += MaxVignette / VignetteTime * Time.deltaTime;

                if (vignette.intensity.value >= MaxVignette)
                {
                    AddVignette = false;
                }
            }
            else
            {
                // 徐々にエフェクトをのけていく
                vignette.intensity.value -= MaxVignette / VignetteTime * Time.deltaTime;

                if (vignette.intensity.value <= MinVignette)
                {
                    AddVignette = true;
                }
            }
        }
        else
        {
            // 非追跡状態なので初期化
            SearchManagerNum = 0;

            // 徐々にvignetteのintensityを下げていく
            if (vignette.intensity.value >= 0.0f)
            {
                vignette.intensity.value -= 0.5f * Time.deltaTime;
                if (vignette.intensity.value < 0.0f)
                {
                    vignette.intensity.value = 0.0f;

                }
            }
        }
    }
}
