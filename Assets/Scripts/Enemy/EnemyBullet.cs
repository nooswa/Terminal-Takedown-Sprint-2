using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject bulletPrefab; //prefab for bullet
    public Transform player; //reference to player transform
    public float shootInterval = 2f; //time interval between shots

    private float shootTimer; //timer tracks shots

    void Update() //called once per frame
    {
        if (player == null) return; //do nothing if there is no player reference

        shootTimer += Time.deltaTime; //accumulate time since last frame

        if (shootTimer >= shootInterval) //checks if its time to shoot
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer() //fire a bullet towards the player
    {
        Vector3 direction = (player.position - transform.position).normalized; //calculate normalised direction vector from enemy to player
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); //instantiate the bullet at the enemys position with no rotation

        bullet.transform.right = -direction; //rotates bullet so right side points in opposite direction

        Bullet bulletScript = bullet.GetComponent<Bullet>(); //get the bullet script component and assign direction
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
}
