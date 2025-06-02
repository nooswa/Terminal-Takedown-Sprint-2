using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; //speed at which bullet moves
    private Vector3 direction; //direction bullet will travel in

    public void SetDirection(Vector3 dir) //method sets direction of bullet
    {
        direction = dir.normalized; //normalise ensures consistent speed
    }

    void Update() //called once per frame
    {
        transform.position += direction * speed * Time.deltaTime; //moves bullet in set direction at specific speed
    }

    void OnTriggerEnter2D(Collider2D other) //called when bullet collides with another collider
    {
        if (other.CompareTag("Player")) //checks if bullet hits player
        {
            HealthManager health = other.GetComponent<HealthManager>(); //tries to get health manager component
            if (health != null) //if player has a healthmanager, apply damage
            {
                health.TakeDamage(10); //take 10 damage
            }

            Destroy(gameObject); //destroys bullet after player is hit
        }
    }
}
