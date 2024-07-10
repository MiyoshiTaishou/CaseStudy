using UnityEngine;

[System.Serializable]
public class SoundEffectClip
{
    public AudioClip clip;
}

public class M_SEPlay : MonoBehaviour
{
    public SoundEffectClip[] soundEffects;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySoundEffect(int index)
    {
        if (index < 0 || index >= soundEffects.Length)
        {
            Debug.LogWarning("Index out of range.");
            return;
        }

        AudioClip clipToPlay = soundEffects[index].clip;
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning("Sound effect at index " + index + " is not assigned.");
        }
    }
}
