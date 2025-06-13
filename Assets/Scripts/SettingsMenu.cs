using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer; 

    public void SetVolume (float volume)//called when volume is edited
    {
        audioMixer.SetFloat("volume", volume); //updates volume upon slider change
    }
}
