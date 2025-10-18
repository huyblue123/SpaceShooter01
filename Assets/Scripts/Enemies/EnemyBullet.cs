using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float damage = 5f; 
    Vector2 direction;
    [SerializeField] private GameObject dmgEffect;
    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject effect = Instantiate(dmgEffect, transform.position, Quaternion.identity);
            effect.transform.parent = null;
            Destroy(effect, 0.5f);
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
