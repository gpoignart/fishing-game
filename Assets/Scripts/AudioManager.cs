using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Allow to call AudioManager.Instance anywhere (singleton)
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    // Musics
    [SerializeField] private AudioClip menuAndEventMusic;
    [SerializeField] private AudioClip mapMusic;
    [SerializeField] private AudioClip fishingDayMusic;
    [SerializeField] private AudioClip fishingNightMusic;
    [SerializeField] private AudioClip monsterMusic;

    // Parameters
    private float fadeDuration = 0.5f;
    private float musicVolume = 1f;

    // Internal references
    private float fishingNightMusicTime;


    // Make this class a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlayMenuAndEventMusic()
    {
        PlayMusicIfNotPlaying(menuAndEventMusic);
    }

    public void PlayMapMusic()
    {
        PlayMusicIfNotPlaying(mapMusic);
    }

    public void PlayFishingDayMusic()
    {
        PlayMusicIfNotPlaying(fishingDayMusic);
    }

    public void InitializeFishingNightMusicTime()
    {
        fishingNightMusicTime = 0f;
    }

    public void PlayFishingNightMusic()
    {
        PlayMusicIfNotPlaying(fishingNightMusic, fishingNightMusicTime);
    }

    public void SaveFishingNightMusicTime()
    {
        if (!musicSource.clip == fishingNightMusic) { return; }
        fishingNightMusicTime = musicSource.time;
    }

    public void PlayMonsterMusic()
    {
        PlayMusicIfNotPlaying(monsterMusic);
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutAndStop());
    }


    // Helping functions

    private void PlayMusicIfNotPlaying(AudioClip music, float resumeTime = 0f)
    {
        if (musicSource.clip == music && musicSource.isPlaying) { return; }
        
        StartCoroutine(FadeOutIn(music, resumeTime));
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0f)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = musicVolume;
    }

    private IEnumerator FadeOutIn(AudioClip newMusic, float resumeTime)
    {
        float startVolume = musicSource.volume;

        // Fade out
        while (musicSource.isPlaying && musicSource.volume > 0f)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();

        // Switch music
        musicSource.clip = newMusic;
        musicSource.time = resumeTime;
        musicSource.volume = 0f;
        musicSource.Play();

        // Fade in
        while (musicSource.volume < musicVolume)
        {
            musicSource.volume += Time.deltaTime / fadeDuration * musicVolume;
            yield return null;
        }

        musicSource.volume = musicVolume;
    }
}
