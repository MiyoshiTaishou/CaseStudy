using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public AudioClip clip;
    public int probability; // �Đ��m���i%�j
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

        // �m���̑��a���v�Z
        int totalProbability = 0;
        foreach (SoundEffect se in soundEffects)
        {
            totalProbability += se.probability;
        }

        // �����_���Ȑ��l�𐶐�
        int randomValue = Random.Range(0, totalProbability);

        // �����_���Ȑ��l�Ɋ�Â��ăT�E���h�G�t�F�N�g��I��
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
