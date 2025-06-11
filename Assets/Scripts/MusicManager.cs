using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; //instance of music manager
    private AudioSource audioSource; //reference to the audiosource
    public AudioClip backgroundMusic; //background music clip
    public AudioClip deathMusic; //death music clip
    public AudioClip bossMusic; //boss music clip

    public float fadeSpeed = 1f; //speed of music transitions

    private bool isBossMusicActive = false;
    private AudioClip previousMusic; //to remember what was playing before boss

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            transform.SetParent(null); //move to root so DontDestroyOnLoad works
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //only one scene (master scene) at index 0, always play background music when scene loads
        PlayBackgroundMusic(true, backgroundMusic);
        isBossMusicActive = false; //reset boss music state on scene load
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
        if (audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }

    public void PlayDeathMusic()
    {
        if (deathMusic != null)
        {
            audioSource.Stop();
            audioSource.clip = deathMusic;
            audioSource.Play();
            isBossMusicActive = false; //death music overrides boss music
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void EnsureBackgroundMusicPlaying()
    {
        if (!audioSource.isPlaying && !isBossMusicActive)
        {
            PlayBackgroundMusic(true, backgroundMusic);
        }
    }

    // New boss music methods
    public void PlayBossMusic()
    {
        if (bossMusic != null && !isBossMusicActive)
        {
            previousMusic = audioSource.clip; // Remember what was playing
            isBossMusicActive = true;
            StartCoroutine(FadeToMusic(bossMusic));
        }
    }

    public void StopBossMusic()
    {
        if (isBossMusicActive)
        {
            isBossMusicActive = false;
            // Return to previous music or default to background music
            AudioClip musicToPlay = previousMusic != null ? previousMusic : backgroundMusic;
            StartCoroutine(FadeToMusic(musicToPlay));
        }
    }

    private IEnumerator FadeToMusic(AudioClip newMusic)
    {
        if (newMusic == null) yield break;

        float originalVolume = audioSource.volume;

        // Fade out current music
        while (audioSource.volume > 0)
        {
            audioSource.volume -= originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Switch to new music
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();

        // Fade in new music
        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.volume = originalVolume; // Ensure exact volume
    }

    // Public method to check if boss music is playing
    public bool IsBossMusicActive()
    {
        return isBossMusicActive;
    }
}