using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; //instance of music manager
    private AudioSource audioSource; //reference to the audiosource
    public AudioClip backgroundMusic; //background music clip
    public AudioClip deathMusic; //death music clip

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
        if (!audioSource.isPlaying)
        {
            PlayBackgroundMusic(true, backgroundMusic);
        }
    }
}