using UnityEngine;

// Detects when an enemy is clicked and opens the terminal UI via the TerminalUIHandler
public class EnemyClickHandler : MonoBehaviour
{
    public TerminalUIHandler terminalUIHandler;
    public GameObject shield; // drag the player's shield GameObject here

    private void OnMouseDown()
    {
        // if the shield is active, ignore clicks
        if (shield != null && shield.activeInHierarchy)
            return;

        // Open the terminal when the enemy is clicked
        if (terminalUIHandler != null)
        {
            terminalUIHandler.OpenTerminal(this.gameObject);
        }
    }
}