using Unity.VisualScripting;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool destroyWhenOffscreen = true;
    [SerializeField] float offscreenX = -20f;

    [SerializeField] float maxHp = 5;
    [SerializeField] float currentHp;
    [SerializeField] float dmgEnter = 5f;

    //[SerializeField] private AudioManager audioManager;
    private void Start()
    {
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
