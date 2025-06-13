using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UISounds : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Main Menu Button Sounds")]
    public AudioClip clickClip;
    public int sceneToLoad = 1;

    [Header("Class Selection Button Sounds")]
    public AudioClip backClickClip;
    public AudioClip softDevClickClip;
    public AudioClip cyberSecClickClip;

    [Header("SoftDevChecklist Button Sounds")]
    public AudioClip softDevBackClickClip;
    public AudioClip softDevStartClickClip;

    [Header("CyberSecChecklist Button Sounds")]
    public AudioClip cyberSecBackClickClip;
    public AudioClip cyberSecStartClickClip;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); //access to audiosource
        audioSource.playOnAwake = false; //disables on awake.
    }

    // normal sound player
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Optional inspector-configured fallback
    public void PlayConfiguredSoundAndLoadScene()
    {
        StartCoroutine(PlayAndLoad(clickClip, sceneToLoad));
    }

    public void PlaySoundAndLoadScene(AudioClip clip, int sceneIndex)
    {
        StartCoroutine(PlayAndLoad(clip, sceneIndex));
    }

    // Class Selection Buttons
    public void PlayBackButtonSound() => StartCoroutine(PlayAndLoad(backClickClip, 0)); //scene transitions from class selection back to main menu
    public void PlaySoftDevButtonSound() => StartCoroutine(PlayAndLoad(softDevClickClip, 4)); //scene transitions from class selection to 
    public void PlayCyberSecButtonSound() => StartCoroutine(PlayAndLoad(cyberSecClickClip, 2));

    // SoftDevChecklist Buttons
    public void PlaySoftDevBackButtonSound() => StartCoroutine(PlayAndLoad(softDevBackClickClip, 1));
    public void PlaySoftDevStartGameButtonSound() => StartCoroutine(PlayAndLoad(softDevStartClickClip, 5));

    // CyberSecChecklist Buttons
    public void PlayCyberSecBackButtonSound() => StartCoroutine(PlayAndLoad(cyberSecBackClickClip, 1));
    public void PlayCyberSecStartGameButtonSound() => StartCoroutine(PlayAndLoad(cyberSecStartClickClip, 3));

    private IEnumerator PlayAndLoad(AudioClip clip, int index)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        SceneManager.LoadScene(index);
    }
}


