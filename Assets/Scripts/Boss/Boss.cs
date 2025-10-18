using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float enterDmg = 10f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float speedCircleBullet = 10f;

    Transform player;
    float shootTimer;

    [SerializeField] private float floatAmplitude = 0.5f; // Biên độ dao động (cao thấp)
    [SerializeField] private float floatSpeed = 2f;       // Tốc độ dao động

    private float startY;

    [SerializeField] private float currentHp; 
    [SerializeField] private float maxHp = 100f;

    [SerializeField] public GameObject winScreen;
    private void Start()
    {
        winScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startY = transform.position.y;
        currentHp = maxHp;
    }
    private void Update()
    {
        if (player == null) return;

        // --- Di chuyển lên xuống ---
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);


        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                UseRandomSkill();
                shootTimer = shootInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(enterDmg);
            }
        }
    }

    private void ShootNormal()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = (player.position - firePoint.position).normalized;

        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
    }
    private void ShootCircle()
    {
        const int bulletCount = 15;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // Tính hướng của từng viên đạn theo góc
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector2 bulletDirection = new Vector2(bulletDirX, bulletDirY).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.SetDirection(bulletDirection);
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bulletDirection * speedCircleBullet;
            }

            angle += angleStep;
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);
        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
        winScreen.SetActive(true);
    }
    private void UseRandomSkill()
    {
        int randomSkill = Random.Range(0, 2); // 0 = bắn thường, 1 = bắn vòng tròn
        switch (randomSkill)
        {
            case 0:
                ShootNormal();
                break;
            case 1:
                ShootCircle();
                break;
        }
    }
}
