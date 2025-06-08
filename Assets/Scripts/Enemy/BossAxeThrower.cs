using UnityEngine;

public class BossAxeThrower : MonoBehaviour
{
    public GameObject axePrefab; // prefab for axe
    public Transform player; // reference to player transform
    public float throwInterval = 3f; // time interval between throws

    private float throwTimer; // timer tracks throws

    void Update() // called once per frame
    {
        if (player == null) return; // do nothing if there is no player reference

        throwTimer += Time.deltaTime; // accumulate time since last frame
        if (throwTimer >= throwInterval) // checks if its time to throw
        {
            ThrowAxeAtPlayer();
            throwTimer = 0f;
        }
    }

    public void ThrowAxeAtPlayer() // throw an axe towards the player
    {
        Vector3 direction = (player.position - transform.position).normalized; // calculate normalised direction vector from boss to player

        GameObject axe = Instantiate(axePrefab, transform.position, Quaternion.identity); // instantiate the axe at the boss position

        // Calculate rotation to face the direction (optional - for visual consistency)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        axe.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Axe axeScript = axe.GetComponent<Axe>(); // get the axe script component and assign direction
        if (axeScript != null)
        {
            axeScript.SetDirection(direction);
        }
    }
}