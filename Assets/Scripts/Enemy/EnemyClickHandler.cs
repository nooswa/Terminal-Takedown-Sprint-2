using UnityEngine;

// Detects when an enemy is clicked and opens the terminal UI via the TerminalUIHandler
public class EnemyClickHandler : MonoBehaviour
{
    public TerminalUIHandler terminalUIHandler; 

    private void OnMouseDown()
    {
        // Open the terminal when the enemy is clicked
        if (terminalUIHandler != null)
        {
            terminalUIHandler.OpenTerminal(this.gameObject);
        }
    }
}