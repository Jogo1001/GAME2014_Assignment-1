using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    void Awake()
    {
       if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }
    else
    {
        Destroy(gameObject);
    }
    }

    public void PlayMusic(AudioClip clip, float volume = 0.6f)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayHelicopter(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.clip = clip;
        sfxSource.loop = true;
        sfxSource.volume = volume;
        sfxSource.Play();
    }
    public void PlayExplosion(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayKill(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayCoin(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }
}
