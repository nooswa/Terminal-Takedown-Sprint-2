using UnityEngine;

// Provides visual feedback when the player hovers the mouse over an enemy, helping to indicate that the enemy is clickable
public class EnemyHoverIndicator : MonoBehaviour
{
    private Renderer enemyRenderer;
    private Color originalColor;
    public Color hoverColor = Color.red; // Highlight color

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>(); //access to renderer
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color; //if not null saves original color
        }
    }

    void OnMouseEnter() //if mouse is on top of enemy uses hover color
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = hoverColor;
        }
    }

    void OnMouseExit() //when mouse is not on top of enemy it stops hover color
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
    }
}