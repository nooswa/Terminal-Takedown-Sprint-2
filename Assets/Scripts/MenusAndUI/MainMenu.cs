using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame(){ //opens scene when playgame method called.
        SceneManager.LoadSceneAsync(1);
   }
}
