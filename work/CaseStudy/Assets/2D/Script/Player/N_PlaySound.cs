using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// se�Đ�

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

    // se���
    [System.Serializable]
    struct SEInfomation
    {
        [Header("���O"), SerializeField]
        public SEName seName;

        [Header("�Đ����鎞��"), SerializeField]
        public float SEPlayTime;

        [Header("�N���b�v"), SerializeField]
        public AudioClip audioClip;
    }

    [Header("SE���"), SerializeField]
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

        // �w�肳�ꂽSE�̏����擾
        foreach(var info in SEInfo)
        {
            // �w��ʂ�̕��������
            if(info.seName == _name)
            {
                break;
            }
            num++;
        }


        // SE�Đ�
        audioSource.PlayOneShot(SEInfo[num].audioClip);
        isPlaying = true;

        yield return new WaitForSeconds(SEInfo[num].SEPlayTime);

        audioSource.Stop();
        isPlaying = false;
        //Debug.Log("��~");
    }
}
