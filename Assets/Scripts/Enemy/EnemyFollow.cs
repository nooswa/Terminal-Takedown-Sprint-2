using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player; 
    public float speed = 2f;
    private Rigidbody2D rb; //rigidbody based movment
    private Animator animator; //for anmations
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rigid grabbers
        animator = GetComponent<Animator>(); //anim grabbers
    }
    void FixedUpdate()
    {
        if (player == null) return; //cchecks for player null
        Vector2 direction = (player.position - transform.position).normalized; 
        rb.linearVelocity = direction * speed;
        animator.SetFloat(horizontal, direction.x); //animations based on movement
        animator.SetFloat(vertical, direction.y);
    }
}