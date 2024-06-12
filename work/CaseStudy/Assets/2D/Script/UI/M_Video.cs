using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class M_Video : MonoBehaviour
{
    [SerializeField] RawImage rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;

    private void Awake()
    {   // �ŏ��͕\�����Ȃ�
        rawImage.enabled = false;
    }

    /// <summary>
    /// URL���w�肵�čĐ�����
    /// </summary>
    public void Play(string url)
    {
        // ����̐ݒ�
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        // �Đ������̊J�n
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }

    /// <summary>
    /// �r�f�I�N���b�v���w�肵�čĐ�����
    /// </summary>
    public void Play(VideoClip videoClip)
    {
        // ����̐ݒ�
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;

        // �Đ������̊J�n
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }

    /// <summary>
    /// �Đ���������������
    /// </summary>
    void OnPrepareCompleted(VideoPlayer vp)
    {
        // �C���[�W�ɓ���e�N�X�`�����Z�b�g����
        rawImage.texture = videoPlayer.texture;

        // �C���[�W�T�C�Y�𓮉�Ɠ����傫���ɂ���
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(videoPlayer.texture.width, videoPlayer.texture.height);

        // �C�x���g�n���h�����Z�b�g���čĐ�����
        videoPlayer.started += OnMovieStarted;
        videoPlayer.Play();
    }

    /// <summary>
    /// �Đ����J�n���ꂽ�Ƃ��ɌĂ΂��C�x���g
    /// </summary>
    void OnMovieStarted(VideoPlayer vp)
    {
        // �Đ����J�n���ꂽ��C���[�W��\������
        rawImage.enabled = true;
    }
}