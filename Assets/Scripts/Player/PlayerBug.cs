using UnityEngine;
public class PlayerBug : MonoBehaviour
{
    public float speed = 8f; // Speed at which bug moves
    public float rotationSpeed = 360f; // Speed of spinning rotation
    public float homingStrength = 2f; // How aggressively it homes towards target
    private Transform target; // The enemy to home towards
    private Vector3 direction; // Current movement direction
    private TerminalUIHandler terminalHandler; // Reference to terminal handler

    public void SetTarget(Transform enemyTarget)
    {
        target = enemyTarget;
        if (target != null)
        {
            // Initial direction towards target
            direction = (target.position - transform.position).normalized;
        }
    }

    public void SetTerminalHandler(TerminalUIHandler handler)
    {
        terminalHandler = handler;
    }

    void Update()
    {
        // Spin the bug continuously
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Move towards target if it exists
        if (target != null)
        {
            // Calculate direction to target
            Vector3 targetDirection = (target.position - transform.position).normalized;
            // Smoothly adjust current direction towards target (homing behavior)
            direction = Vector3.Slerp(direction, targetDirection, homingStrength * Time.deltaTime).normalized;
            // Move the bug
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // If no target, move in current direction
            transform.position += direction * speed * Time.deltaTime;
        }

        // Destroy if too far from origin (safety cleanup)
        if (Vector3.Distance(transform.position, Vector3.zero) > 50f)
        {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if bug hits an enemy
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // Use the stored reference first, then try to find it if not set
            if (terminalHandler == null)
            {
                terminalHandler = FindObjectOfType<TerminalUIHandler>();
            }

            if (terminalHandler != null)
            {
                terminalHandler.ExplodeRobot(other.gameObject);
            }
            else
            {
                // Alternative: try to find it by name or other method
                GameObject terminalGO = GameObject.Find("TerminalUIHandler");
                if (terminalGO != null)
                {
                    TerminalUIHandler altHandler = terminalGO.GetComponent<TerminalUIHandler>();
                    if (altHandler != null)
                    {
                        altHandler.ExplodeRobot(other.gameObject);
                    }
                }
            }

            // Destroy the bug after hitting enemy
            Destroy(gameObject);
        }
    }
}