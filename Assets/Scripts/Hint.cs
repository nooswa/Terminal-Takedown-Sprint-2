using System;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public GameObject HintPack;

    private bool HintActive = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (HintActive == false)
            {
                HintActive = true;
                HintPack.SetActive(true);
            }
            else
            {
                HintActive = false;
                HintPack.SetActive(false);
            }
        }
    }
}
