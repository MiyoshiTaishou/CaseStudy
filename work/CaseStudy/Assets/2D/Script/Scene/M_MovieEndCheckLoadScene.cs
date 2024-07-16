using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class M_MovieEndCheckLoadScene : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer component not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SympathyButton") && isEnd)
        {
            SceneManager.LoadScene("M_TestTitleScene");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {        
        isEnd = true;
    }
}
