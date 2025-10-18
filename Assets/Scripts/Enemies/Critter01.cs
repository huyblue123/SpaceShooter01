using UnityEngine;
using UnityEngine.UI; 

public class Critter01 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private float moveSpeed;
    private Vector3 targetPosition;

    private float moveTimer;
    private float moveInterval;

    private Quaternion targetRotation;


    [SerializeField] private float dmgEnter = 2f;
    [SerializeField] private GameObject burnEffect;
   // [SerializeField] private AudioManager audioManager;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        moveSpeed = Random.Range(0.5f, 3f);
        GenerateRandomPosition();
        moveInterval = Random.Range(0.5f, 3f);
        moveTimer = moveInterval;
        //currentHp = maxHp;
    }

    void Update()
    {
        if (moveTimer > 0) { 
            moveTimer -= Time.deltaTime;
        }
        else
        {
            GenerateRandomPosition();
            moveTimer = moveInterval; 
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 relativePos = targetPosition - transform.position;
        if (relativePos != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1080 * Time.deltaTime);
        }
    }
    private void GenerateRandomPosition()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector2(randomX, randomY);
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
        if (collision.CompareTag("Bullet"))
        {
            if (burnEffect != null)
            {
                //audioManager.PlayEnemyHit();
                GameObject effect = Instantiate(burnEffect, transform.position, Quaternion.identity);
                effect.transform.parent = null;
                Destroy(effect, 0.5f);
            }
            Destroy(gameObject);
        }
    }
}
