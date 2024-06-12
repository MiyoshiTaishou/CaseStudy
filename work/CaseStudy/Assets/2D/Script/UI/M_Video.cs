using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class M_Video : MonoBehaviour
{
    [SerializeField] RawImage rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;

    private void Awake()
    {   // 最初は表示しない
        rawImage.enabled = false;
    }

    /// <summary>
    /// URLを指定して再生する
    /// </summary>
    public void Play(string url)
    {
        // 動画の設定
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        // 再生準備の開始
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }

    /// <summary>
    /// ビデオクリップを指定して再生する
    /// </summary>
    public void Play(VideoClip videoClip)
    {
        // 動画の設定
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;

        // 再生準備の開始
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }

    /// <summary>
    /// 再生準備が完了した
    /// </summary>
    void OnPrepareCompleted(VideoPlayer vp)
    {
        // イメージに動画テクスチャをセットする
        rawImage.texture = videoPlayer.texture;

        // イメージサイズを動画と同じ大きさにする
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(videoPlayer.texture.width, videoPlayer.texture.height);

        // イベントハンドラをセットして再生する
        videoPlayer.started += OnMovieStarted;
        videoPlayer.Play();
    }

    /// <summary>
    /// 再生が開始されたときに呼ばれるイベント
    /// </summary>
    void OnMovieStarted(VideoPlayer vp)
    {
        // 再生が開始されたらイメージを表示する
        rawImage.enabled = true;
    }
}