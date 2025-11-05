using UnityEngine;

public class Whales : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp = 100f;
    protected float currentHp;

    [Header("Damage")]
    public float dmgEnter = 10f;

    [Header("Components")]
    public Animator animator;

    protected virtual void Start()
    {
        currentHp = maxHp;
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Hàm nhận sát thương
    public virtual void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Max(currentHp, 0);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // Hàm chết
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    // Va chạm với player
    protected virtual void OnTriggerEnter2D(Collider2D collision)
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
