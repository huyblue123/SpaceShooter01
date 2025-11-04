using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float xMin, xMax, yMin, yMax;
    private Animator anim;
    public Joystick joyStick; 

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    public float spacing = 0.3f;

    [Header("HP Settings")]
    [SerializeField] private float maxHp = 50f;
    [SerializeField] private float currentHp;
    [SerializeField] private Image hpBar;

    [Header("Energy Settings")]
    [SerializeField] private float maxEne = 50f;
    [SerializeField] private float currentEne;
    [SerializeField] private float eneRegenate = 5f; // tốc độ hồi năng lượng
    [SerializeField] private float eneCost = 10f;    // năng lượng tiêu hao khi bắn hoặc dash
    [SerializeField] private Image eneBar;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashTime = 0.2f; // thời gian dash
    private bool isDashing;

    [SerializeField] public GameObject gameOverObj;
    
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Giới hạn camera
        Camera cam = Camera.main;
        Vector3 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        xMin = screenBottomLeft.x + 0.5f;
        xMax = screenTopRight.x - 0.5f;
        yMin = screenBottomLeft.y + 0.5f;
        yMax = screenTopRight.y - 0.5f;

        // Khởi tạo máu và năng lượng
        currentHp = maxHp;
        currentEne = maxEne;
        updateHPBar();
        updateEneBar();

        gameOverObj.SetActive(false);
    }

    void Update()
    {
        // Lấy input từ bàn phím
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Lấy input từ joystick
        float joyX = joyStick.Horizontal;
        float joyY = joyStick.Vertical;

        // Kết hợp cả hai (ưu tiên cái nào lớn hơn)
        float finalX = Mathf.Abs(inputX) > Mathf.Abs(joyX) ? inputX : joyX;
        float finalY = Mathf.Abs(inputY) > Mathf.Abs(joyY) ? inputY : joyY;

        moveInput = new Vector2(finalX, finalY).normalized;

        // --- Animation ---
        anim.SetBool("isUp", moveInput.y > 0.2f);
        anim.SetBool("isDown", moveInput.y < -0.2f);
        anim.SetBool("isDash", isDashing);

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if (Input.GetKeyDown(KeyCode.F))
            ShootParallel();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            StartCoroutine(Dash());

        if (currentEne < maxEne)
        {
            currentEne += eneRegenate * Time.deltaTime;
            currentEne = Mathf.Clamp(currentEne, 0, maxEne);
            updateEneBar();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }

        // Giới hạn trong màn hình
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        transform.position = pos;
    }

    // --- BẮN ĐẠN THƯỜNG ---
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetSpeed(bulletSpeed);
        audioManager.PlayShootSound(); 
    }

    // --- BẮN CHÙM ---
    public void ShootParallel()
    {
        if (currentEne < eneCost)
            return;

        currentEne -= eneCost;
        updateEneBar();

        int bulletCount = 3;
        Vector2 fireDir = firePoint.right;
        Vector2 perp = new Vector2(-fireDir.y, fireDir.x);
        float startOffset = -(bulletCount - 1) * spacing / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 offset = perp * (startOffset + i * spacing);
            Vector3 spawnPos = firePoint.position + (Vector3)offset;

            GameObject b = Instantiate(bulletPrefab, spawnPos, firePoint.rotation);
            Rigidbody2D rbBullet = b.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
                rbBullet.linearVelocity = fireDir * bulletSpeed;
            audioManager.PlayEnemyHit();
        }
    }

    // --- DASH ---
    public IEnumerator Dash()
    {
        if (currentEne < eneCost)
            yield break; // không đủ năng lượng thì bỏ qua

        // Trừ năng lượng khi dash
        currentEne -= eneCost;
        updateEneBar();

        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            rb.linearVelocity = moveInput * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }
    public void DashTrigger()
    {
        StartCoroutine(Dash());
    }

    // --- HP ---
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);
        anim.SetTrigger("TakeDmg");
        updateHPBar();
        if (currentHp <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Min(currentHp, maxHp);
        updateHPBar();
    }

    public void Die()
    {
        gameOverObj.SetActive(true);
        Destroy(gameObject);
    }

    // --- THANH MÁU & NĂNG LƯỢNG ---
    private void updateHPBar()
    {
        if (hpBar != null)
            hpBar.fillAmount = currentHp / maxHp;
    }

    private void updateEneBar()
    {
        if (eneBar != null)
            eneBar.fillAmount = currentEne / maxEne;
    }
}
