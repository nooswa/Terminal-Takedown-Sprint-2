using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        animator.SetFloat(horizontal, direction.x);
        animator.SetFloat(vertical, direction.y);
    }
}