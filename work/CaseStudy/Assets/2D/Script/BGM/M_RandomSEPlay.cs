using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public AudioClip clip;
    public int probability; // 再生確率（%）
}

public class M_RandomSEPlay : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayRandomSoundEffect()
    {
        if (soundEffects.Length == 0)
        {
            Debug.LogWarning("No sound effects assigned.");
            return;
        }

        // 確率の総和を計算
        int totalProbability = 0;
        foreach (SoundEffect se in soundEffects)
        {
            totalProbability += se.probability;
        }

        // ランダムな数値を生成
        int randomValue = Random.Range(0, totalProbability);

        // ランダムな数値に基づいてサウンドエフェクトを選択
        int cumulativeProbability = 0;
        foreach (SoundEffect se in soundEffects)
        {
            cumulativeProbability += se.probability;
            if (randomValue < cumulativeProbability)
            {
                audioSource.PlayOneShot(se.clip);
                return;
            }
        }
    }
}
