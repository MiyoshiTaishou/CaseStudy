using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class M_PlayMovie : MonoBehaviour
{
    [SerializeField] M_Video movieImage = null;
    [SerializeField] VideoClip videoClip = null;

    public void Start()
    {   // ������Đ�����
        movieImage.Play(videoClip);
    }
}
