using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 適用したマテリアルのフィルターがカメラにかかる
/// </summary>
public class M_CameraFilter : MonoBehaviour
{
    [SerializeField] private Material filter;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, filter);
    }
}
