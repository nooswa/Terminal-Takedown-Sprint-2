using UnityEngine;

// Detects when an enemy is clicked and opens the terminal UI via the TerminalUIHandler
public class EnemyClickHandler : MonoBehaviour
{
    [SerializeField] private TerminalUIHandler terminalUI;  // drag this in Inspector

    private void OnMouseDown()
    {
        if (terminalUI != null)
        {
            terminalUI.OpenTerminal(gameObject);
        }
        else
        {
            Debug.LogError("TerminalUIHandler reference not assigned in Inspector!");
        }
    }
}
