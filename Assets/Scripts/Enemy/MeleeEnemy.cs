using UnityEngine;

public class MeleeEnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 5f;
    public float damageDelay = 1f; // Time between damage instances
    private float lastDamageTime = 0f;

    [Header("Visual Effects")]
    public ParticleSystem attackParticleSystem; // Drag your particle system prefab here
    public Vector3 particleOffset = Vector3.zero; // Offset from enemy position for particle spawn

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the enemy touched the player
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Continue dealing damage while touching the player
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }

    private void DealDamage(Collider2D playerCollider)
    {
        // Only deal damage if enough time has passed since last damage
        if (Time.time >= lastDamageTime + damageDelay)
        {
            HealthManager healthManager = playerCollider.GetComponent<HealthManager>();
            if (healthManager != null && !healthManager.IsDead())
            {
                healthManager.TakeDamage(damageAmount);
                lastDamageTime = Time.time;

                // Play particle effect
                PlayAttackParticles(playerCollider.transform.position);

                // Optional: Add debug message
                Debug.Log($"Enemy dealt {damageAmount} damage to player!");
            }
        }
    }

    private void PlayAttackParticles(Vector3 playerPosition)
    {
        if (attackParticleSystem != null)
        {
            // Position particles between enemy and player
            Vector3 midpoint = (transform.position + playerPosition) / 2f + particleOffset;
            attackParticleSystem.transform.position = midpoint;

            // Calculate direction from enemy to player
            Vector3 direction = (playerPosition - transform.position).normalized;

            // Calculate rotation to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            attackParticleSystem.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Play the particle effect
            attackParticleSystem.Play();
        }
        else
        {
            Debug.LogWarning("No particle system assigned to MeleeEnemyDamage on " + gameObject.name);
        }
    }
}