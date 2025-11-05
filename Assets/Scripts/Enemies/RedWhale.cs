using UnityEngine;

public class RedWhale : Whales
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private Rigidbody2D rb;

    [Header("Drop")]
    [SerializeField] private GameObject healthItem;

    private Transform player;
    private Vector2 moveDirection;

    protected override void Start()
    {
        base.Start(); // init HP và animator từ class cha
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu player trong tầm detect → di chuyển về phía player
        if (distance <= detectRange && distance > stopDistance)
        {
            moveDirection = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg); 
        animator.SetTrigger("dmg");
        if (currentHp <= 0 && healthItem != null)
        {
            Instantiate(healthItem, transform.position, Quaternion.identity);
        }
    }

    public override void Die()
    {
        base.Die();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(dmgEnter); 
            }
        }
    }
}
