using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class M_PlayMovie : MonoBehaviour
{
    [SerializeField] M_Video movieImage = null;
    [SerializeField] VideoClip videoClip = null;

    public void Start()
    {   // 動画を再生する
        movieImage.Play(videoClip);
    }
}
