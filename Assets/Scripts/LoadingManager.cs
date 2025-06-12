using UnityEngine;
using System.Collections;

public class HideLoadingPanelAndUnfreeze : MonoBehaviour
{
    public GameObject loadingPanel; // Assign in the Inspector

    private void Start()
    {
        StartCoroutine(FreezeGameAndHidePanel());
    }

    private IEnumerator FreezeGameAndHidePanel()
    {
        // Freeze game
        Time.timeScale = 0f;

        // Wait for 20 seconds (use unscaled time so it’s not affected by timeScale)
        float elapsed = 0f;
        while (elapsed < 20f)
        {
            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }

        // Hide loading panel and unfreeze game
        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}