using UnityEngine;
public class PlayerBugShooter : MonoBehaviour
{
    public GameObject bugPrefab; // Drag your bug PNG prefab here

    public void ShootBugAtEnemy(Transform enemyTarget, TerminalUIHandler terminalHandler = null)
    {
        if (bugPrefab == null || enemyTarget == null)
        {
            return;
        }

        // Create the bug at player position
        GameObject bug = Instantiate(bugPrefab, transform.position, Quaternion.identity);

        // Get the PlayerBug component and set target
        PlayerBug bugScript = bug.GetComponent<PlayerBug>();
        if (bugScript != null)
        {
            bugScript.SetTarget(enemyTarget);

            // Set terminal handler reference if provided
            if (terminalHandler != null)
            {
                bugScript.SetTerminalHandler(terminalHandler);
            }
        }
    }

    public void ShootBugAtNearestEnemy()
    {
        // Find the nearest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        // Check regular enemies
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // Check boss enemies
        foreach (GameObject boss in bosses)
        {
            float distance = Vector3.Distance(transform.position, boss.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = boss;
            }
        }

        // Shoot at the nearest enemy if found
        if (nearestEnemy != null)
        {
            ShootBugAtEnemy(nearestEnemy.transform);
        }
    }
}