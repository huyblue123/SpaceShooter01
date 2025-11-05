using UnityEngine;

public class PurpleWhale : Whales
{
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 4f;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequency = 2f;
    private Vector3 startPos;
    private Rigidbody2D rb;

    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float detectRange = 10f;
    private float shootTimer;
    private Transform player;

    protected override void Start()
    {
        base.Start(); // init HP và animator
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        // Di chuyển ngang sang trái
        float y = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x - horizontalSpeed * Time.deltaTime, y, transform.position.z);

        if (player == null) return;

        // Kiểm tra tầm bắn
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootInterval;
            }
        }
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        animator.SetTrigger("dmg");
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

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
    }
}
