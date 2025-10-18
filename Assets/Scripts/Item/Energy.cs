using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                gameManager.AddEnergy();
                Destroy(gameObject);
            }
        }
    }
}
