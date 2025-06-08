using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MainMenuTests
{
    [UnityTest]
    public IEnumerator MainMenu_ShouldContain_PlayButton()
    {
        // loads the MainMenu scene to make sure it’s scene index 0 in build settings
        SceneManager.LoadScene(0);
        yield return null; // wait one frame

        // attempts to find the Play button by name
        var playButton = GameObject.Find("Play");

        // checks the acceptance criteria
        Assert.That(playButton, Is.Not.Null, "Play Button is not found in Main Menu.");
    }
}
