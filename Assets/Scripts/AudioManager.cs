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
    [SerializeField] private AudioClip monsterGotPlayerTransition;
    [SerializeField] private AudioClip endDayTransition;
    [SerializeField] private AudioClip endNightTransition;
    [SerializeField] private AudioClip makeRecipe;
    [SerializeField] private AudioClip turnRecipeBookPage;

    // Parameters
    private float musicFadeDuration = 0.3f;
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
    
    private void PlayMusic(AudioClip newMusic, float resumeTime = 0f)
    {
        // To avoid collision if we call a new music when the olds ones are not entirely fade in/fade out
        if (fadeCoroutine != null) 
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
            
            // Stop the music not entirely fade in/fade out to fade in the new music directly
            musicSource.Stop();
            musicSource.volume = musicVolume;
        }

        // We don't change the music if it is already playing
        if (musicSource.clip == newMusic && musicSource.isPlaying) { return; }

        fadeCoroutine = StartCoroutine(FadeOutIn(newMusic, resumeTime));
    }

    public void StopMusic()
    {
        // To avoid collision if we stop a music when a new one could be soon fading in
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    public void PlayMenuAndEventMusic()
    {
        PlayMusic(menuAndEventMusic);
    }

    public void PlayMapMusic()
    {
        PlayMusic(mapMusic);
    }

    public void PlayFishingDayMusic()
    {
        PlayMusic(fishingDayMusic);
    }

    public void InitializeFishingNightMusicTime()
    {
        fishingNightMusicTime = 0f;
    }

    public void PlayFishingNightMusic()
    {
        PlayMusic(fishingNightMusic, fishingNightMusicTime);
    }

    public void SaveFishingNightMusicTime()
    {
        if (!musicSource.clip == fishingNightMusic) { return; }
        fishingNightMusicTime = musicSource.time;
    }

    public void PlayMonsterMusic()
    {
        PlayMusic(monsterMusic);
    }


    // SFX functions

    private void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }

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

    public void PlayerMonsterGotPlayerTransitionSFX()
    {
        PlaySFX(monsterGotPlayerTransition);
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

    public void PlayTurnRecipeBookPageSFX()
    {
        PlaySFX(turnRecipeBookPage);
    }

    public void PlayFishSwamAwaySFX()
    {
        PlaySFX(fishSwamAway);
    }

    public void PlayEndDayTransitionSFX()
    {
        PlaySFX(endDayTransition);
    }

    public void PlayEndNightTransitionSFX()
    {
        PlaySFX(endNightTransition);
    }


    // Helping functions

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
        fadeCoroutine = null;
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
        fadeCoroutine = null;
    }
}
