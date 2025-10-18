using UnityEngine;

public class BlueWhale : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 5f;
    [SerializeField] bool destroyWhenOffscreen = true;
    [SerializeField] float offscreenX = -20f;

    [SerializeField] float maxHp = 5;
    [SerializeField] float currentHp;
    [SerializeField] float dmgEnter = 5f;

    [SerializeField] private GameObject energy;
    //[SerializeField] private AudioManager audioManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (destroyWhenOffscreen && transform.position.x < offscreenX)
            Destroy(gameObject);
    }
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);
        //audioManager.PlayEnemyHit();
        if (currentHp <= 0)
        {
            Die();
            Instantiate(energy, transform.position, Quaternion.identity);
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
}
