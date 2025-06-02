using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    private Rigidbody2D rb;

    private Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        _animator.SetFloat(_horizontal, direction.x);
        _animator.SetFloat(_vertical, direction.y);
    }
}