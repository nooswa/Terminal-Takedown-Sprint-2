using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;

    public AudioClip backgroundMusic; //music for different scenarios
    public AudioClip deathMusic;
    public AudioClip bossMusic;

    public float fadeSpeed = 1f; //speed of fade between songs

    private bool isBossMusicActive = false;
    private float pausedTime = 0f;

    private void Awake()
    {
        if (Instance == null) //ensures only 1 instance is running
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); //destroys extra instances
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //runs with scene loads
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //stops when disabled
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isBossMusicActive) // Only play background if boss music is NOT active
        {
            Debug.Log("Scene loaded: Playing background music");
            PlayBackgroundMusic(true, backgroundMusic);
        }
        else
        {
            Debug.Log("Scene loaded, but boss music is active, skipping background music");
    }
}


    public static void SetVolume(float volume) //public for volume change
    {
        if (Instance != null)
            Instance.audioSource.volume = volume;
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null) //plays bg music
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
                pausedTime = 0f;
            }
            audioSource.time = pausedTime;
            audioSource.Play();
        }
    }

    public void PauseBackgroundMusic() //pauses music and tracks pausedtime
    {
        if (audioSource.isPlaying)
        {
            pausedTime = audioSource.time;
            audioSource.Pause();
        }
    }

    public void PlayDeathMusic() //death music player upon death
    {
        if (deathMusic != null)
        {
            pausedTime = 0f;
            audioSource.Stop();
            audioSource.clip = deathMusic;
            audioSource.Play();
            isBossMusicActive = false;
        }
    }

    public void StopMusic() //stops music method
    {
        if (audioSource.isPlaying)
        {
            pausedTime = 0f;
            audioSource.Stop();
        }
    }

    public void EnsureBackgroundMusicPlaying() //keeps music playing (avoids false//acidental stops)
    {
        if (!audioSource.isPlaying && !isBossMusicActive)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
    }

    public void PlayBossMusic() //boss music
    {
        if (bossMusic != null && !isBossMusicActive)
        {
            pausedTime = audioSource.time; // Save progress
            isBossMusicActive = true;
            StartCoroutine(SwitchToBossMusic());
        }
    }

    public void StopBossMusic() //stop boss music
    {
        if (isBossMusicActive)
        {
            isBossMusicActive = false;
            StartCoroutine(SwitchBackToBackgroundMusic());
        }
    }

    private IEnumerator SwitchToBossMusic() //bossmusic switcher
    {
        float originalVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.Pause(); // Pause background
        audioSource.clip = bossMusic;
        audioSource.time = 0f;
        audioSource.Play();

        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.volume = originalVolume;
    }

    private IEnumerator SwitchBackToBackgroundMusic() //switches to bg music
    {
        float originalVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = backgroundMusic;
        audioSource.time = pausedTime; // Resume from last point
        audioSource.Play();

        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += originalVolume * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.volume = originalVolume;
    }

    public bool IsBossMusicActive() //bool to check if boss music is on.
    {
        return isBossMusicActive;
    }
}
