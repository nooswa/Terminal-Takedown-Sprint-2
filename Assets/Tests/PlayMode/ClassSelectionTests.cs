using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ClassSelectionTests
{
    [UnityTest]
    public IEnumerator ClassSelection_ShouldContain_CyberSecAndSoftDevButtons()
    {
        // loads the class selection scene index 0
        SceneManager.LoadScene(1);
        yield return null;

        //looks for buttonsa
        var cyberButton = GameObject.Find("CyberSec");
        var softButton  = GameObject.Find("SoftDev");

        //checks if they are there
        if (cyberButton == null || softButton == null)
        {
            string missing = cyberButton == null && softButton == null
                ? "CyberSecButton and SoftDevButton"
                : cyberButton == null
                    ? "CyberSecButton"
                    : "SoftDevButton";
            Assert.Fail($"{missing} not found in class selection.");
        }
    }
}

