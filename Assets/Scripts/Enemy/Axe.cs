using UnityEngine;

public class Axe : MonoBehaviour
{
    public float speed = 4f; // speed at which axe moves
    public float rotationSpeed = 360f; // rotation speed for spinning effect
    public int damage = 20; // damage dealt by axe
    private Vector3 direction; // direction axe will travel in

    public void SetDirection(Vector3 dir) // method sets direction of axe
    {
        direction = dir.normalized; // normalise ensures consistent speed
    }

    void Update() // called once per frame
    {
        // Move axe in set direction at specific speed
        transform.position += direction * speed * Time.deltaTime;

        // Rotate axe for spinning effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) // called when axe collides with another collider
    {
        if (other.CompareTag("Player")) // checks if axe hits player
        {
            HealthManager health = other.GetComponent<HealthManager>(); // tries to get health manager component
            if (health != null) // if player has a healthmanager, apply damage
            {
                health.TakeDamage(damage); // take damage
            }
            Destroy(gameObject); // destroys axe after player is hit
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground")) // destroy axe if it hits walls/ground
        {
            Destroy(gameObject);
        }
    }
}