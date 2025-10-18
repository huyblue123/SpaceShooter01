using UnityEngine;

public class RedWhale : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;       // tốc độ di chuyển
    [SerializeField] float detectRange = 10f;    // khoảng cách phát hiện player
    [SerializeField] float stopDistance = 0.5f;  // dừng lại nếu chạm player

    [SerializeField] float damage = 1f;
    [SerializeField] public Rigidbody2D rb;

    Transform player;
    Vector2 moveDirection;

    [SerializeField] float maxHp = 5;
    [SerializeField] float currentHp;

    [SerializeField] GameObject heatlhItem;

    //[SerializeField] private AudioManager audioManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHp = maxHp;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu trong tầm phát hiện → bắt đầu lao tới
        if (distance <= detectRange && distance > stopDistance)
        {
            moveDirection = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);
       // audioManager.PlayEnemyHit();
        if (currentHp <= 0)
        {
            Die();
            Instantiate(heatlhItem, transform.position, Quaternion.identity);
        }
    }
    public void Die()
    {

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}
