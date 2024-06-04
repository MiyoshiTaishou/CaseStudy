using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerTrail : MonoBehaviour
{
    [Header("プレイヤーオブジェクト"),SerializeField]
    private GameObject player;

    [Header("残像オブジェクト"), SerializeField]
    private GameObject trailPrefab;

    [Header("残像の間隔"), SerializeField]
    private int trailInterval = 3;

    [Header("残像の長さ"), SerializeField]
    private int trailLength = 5; // 残像の長さ

    [Header("残像のフェードアウト時間"), SerializeField]
    private float fadeTime = 0.5f; // 残像のフェードアウト時間

    [Header("残像の表示位置"),SerializeField]
    private Vector3 trailOffset = Vector3.zero;

    private int intervalCounter;
    private GameObject[] trails;

    void Start()
    {
        trails = new GameObject[trailLength];
        intervalCounter = trailInterval;
    }

    void Update()
    {
        if(!M_GameMaster.GetGamePlay())
        {
            return;
        }

        // プレイヤーがダッシュしているかどうかを確認
        if (player.GetComponent<M_PlayerMove>().GetIsDash())
        {
            intervalCounter--;
            // 残像の間隔ごとに残像を生成
            if (intervalCounter <= 0)
            {
                intervalCounter = trailInterval;
                GenerateTrail();
            }
        }
    }

    void GenerateTrail()
    {
        // プレイヤーの現在の回転を取得
        Quaternion playerRotation = player.transform.rotation;

        // 新しい残像を生成
        GameObject newTrail = Instantiate(trailPrefab, player.transform.position + trailOffset, playerRotation);
        // 残像をリストに追加
        for (int i = 0; i < trails.Length - 1; i++)
        {
            trails[i] = trails[i + 1];
        }
        trails[trails.Length - 1] = newTrail;
        // 残像のフェードアウトを開始
        StartCoroutine(FadeOutTrail(newTrail));
    }

    IEnumerator FadeOutTrail(GameObject trail)
    {
        SpriteRenderer trailRenderer = trail.GetComponent<SpriteRenderer>();
        Color color = trailRenderer.color;
        float startTime = Time.time;
        while (Time.time < startTime + fadeTime)
        {
            float t = (Time.time - startTime) / fadeTime;
            color.a = Mathf.Lerp(1f, 0f, t);
            trailRenderer.color = color;
            yield return null;
        }
        Destroy(trail);
    }
}
