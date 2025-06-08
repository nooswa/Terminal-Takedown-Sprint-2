using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class LoadGameSceneTests
{
    [UnityTest]
    public IEnumerator StartButton_ShouldLoad_SoftDevMasterScene()
    {
        // loads SoftDevChecklist scene index 4
        SceneManager.LoadScene(4);
        yield return null;

        // find and click the STARTGAME button
        var startBtn = GameObject.Find("STARTGAME");
        Assert.That(startBtn, Is.Not.Null, "STARTGAME not found in SoftDevChecklist scene.");
        startBtn.GetComponent<Button>().onClick.Invoke();

        // wait until the new scene becomes active
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 5);

        // checks if next scene is index 5
        Assert.That(
            SceneManager.GetActiveScene().buildIndex,
            Is.EqualTo(3),
            "SoftDev master scene did not load after clicking STARTGAME."
        );
    }
}
