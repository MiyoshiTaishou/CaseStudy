using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// se再生

public class N_PlaySound : MonoBehaviour
{
    public enum SEName
    {
        CrowCry,
        Decide,
        CursorMove,
        OpenLetter,
        Transition,
        Cansel,
        Stamp,
        Dash,
        StageChange,
    };

    // se情報
    [System.Serializable]
    struct SEInfomation
    {
        [Header("名前"), SerializeField]
        public SEName seName;

        [Header("再生する時間"), SerializeField]
        public float SEPlayTime;

        [Header("クリップ"), SerializeField]
        public AudioClip audioClip;
    }

    [Header("SE情報"), SerializeField]
    SEInfomation[] SEInfo;

    private AudioSource audioSource;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SEName _name)
    {
        if (isPlaying)
        {
            return;
        }
        StartCoroutine(Play(_name));
    }

    public bool GetIsPlaying()
    {
        return isPlaying;/*audioSource.isPlaying;*/
    }

    IEnumerator Play(SEName _name)
    {
        int num = 0;

        // 指定されたSEの情報を取得
        foreach(var info in SEInfo)
        {
            // 指定通りの物があれば
            if(info.seName == _name)
            {
                break;
            }
            num++;
        }


        // SE再生
        audioSource.PlayOneShot(SEInfo[num].audioClip);
        isPlaying = true;

        yield return new WaitForSeconds(SEInfo[num].SEPlayTime);

        audioSource.Stop();
        isPlaying = false;
        //Debug.Log("停止");
    }
}
