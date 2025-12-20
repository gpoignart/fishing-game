using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Allow to call AudioManager.Instance anywhere (singleton)
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // Musics
    [SerializeField] private AudioClip menuAndEventMusic;
    [SerializeField] private AudioClip mapMusic;
    [SerializeField] private AudioClip fishingDayMusic;
    [SerializeField] private AudioClip fishingNightMusic;
    [SerializeField] private AudioClip monsterMusic;

    // Sound effects
    [SerializeField] private AudioClip catchingFish;
    [SerializeField] private AudioClip catchingRareFish;
    [SerializeField] private AudioClip fishSwamAway;
    [SerializeField] private AudioClip monsterRanAway;
    [SerializeField] private AudioClip monsterScreamLeft;
    [SerializeField] private AudioClip monsterScreamRight;
    [SerializeField] private AudioClip monsterGotPlayer;
    [SerializeField] private AudioClip makeRecipe;
    [SerializeField] private AudioClip endDay;
    [SerializeField] private AudioClip endNight;

    // Parameters
    private float musicFadeDuration = 0.5f;
    private float musicVolume = 1f;

    // Internal references
    private float fishingNightMusicTime;
    private Coroutine fadeCoroutine;


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

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }


    // Music functions

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


    // SFX functions

    public void PlayMonsterScreamLeftSFX()
    {
        PlaySFX(monsterScreamLeft);
    }

    public void PlayMonsterScreamRightSFX()
    {
        PlaySFX(monsterScreamRight);
    }

    public void PlayMonsterRanAwaySFX()
    {
        PlaySFX(monsterRanAway);
    }

    public void PlayerMonsterGotPlayerSFX()
    {
        PlaySFX(monsterGotPlayer);
    }

    public void PlayCatchingFishSFX()
    {
        PlaySFX(catchingFish);
    }

    public void PlayCatchingRareFishSFX()
    {
        PlaySFX(catchingRareFish);
    }

    public void PlayMakeRecipeSFX()
    {
        PlaySFX(makeRecipe);
    }

    public void PlayFishSwamAwaySFX()
    {
        PlaySFX(fishSwamAway);
    }

    public void PlayEndDaySFX()
    {
        PlaySFX(endDay);
    }

    public void PlayEndNightSFX()
    {
        PlaySFX(endNight);
    }


    // Helping functions

    private void PlayMusicIfNotPlaying(AudioClip music, float resumeTime = 0f)
    {
        if (musicSource.clip == music && musicSource.isPlaying) { return; }
        
        // To avoid collision, only one fadeCoroutine
        if (fadeCoroutine != null) { StopCoroutine(fadeCoroutine); }

        fadeCoroutine = StartCoroutine(FadeOutIn(music, resumeTime));
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0f)
        {
            musicSource.volume -= startVolume * Time.deltaTime / musicFadeDuration;
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
            musicSource.volume -= startVolume * Time.deltaTime / musicFadeDuration;
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
            musicSource.volume += Time.deltaTime / musicFadeDuration * musicVolume;
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }
}
