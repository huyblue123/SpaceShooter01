using UnityEngine;

public class PurpleWhale : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 4f;
    [SerializeField] float amplitude = 1f;    // biên độ dao động theo Y
    [SerializeField] float frequency = 2f;    // tốc độ dao động (số sóng)

    [SerializeField] float maxHp = 5;
    [SerializeField] float currentHp;
    [SerializeField] float dmgEnter = 5f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootInterval = 2f; // khoảng cách giữa các lần bắn
    [SerializeField] Transform firePoint; // vị trí bắn (gắn empty object trước miệng súng)
    [SerializeField] float detectRange = 10f; // tầm nhìn enemy

    Transform player;
    float shootTimer;

    private Rigidbody2D rb;

    Vector3 startPos;

    //[SerializeField] private AudioManager audioManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    void Update()
    {
        // di chuyển ngang
        float x = transform.position.x + Vector3.left.x * horizontalSpeed * Time.deltaTime;

        // dao động Y theo sin
        float y = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(transform.position.x - horizontalSpeed * Time.deltaTime, y, transform.position.z);

        if (player == null) return;

        // Kiểm tra nếu player nằm trong tầm bắn
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
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);
       // audioManager.PlayEnemyHit();
        if (currentHp <= 0)
        {
            Die();
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
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(dmgEnter);
            }
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = (player.position - firePoint.position).normalized;

        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
    }
}
