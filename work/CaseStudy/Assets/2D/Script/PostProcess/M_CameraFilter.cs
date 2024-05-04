using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K�p�����}�e���A���̃t�B���^�[���J�����ɂ�����
/// </summary>
public class M_CameraFilter : MonoBehaviour
{
    [SerializeField] private Material filter;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, filter);
    }
}
