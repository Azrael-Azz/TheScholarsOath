using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip enemySound;
    public AudioClip deathSound;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}