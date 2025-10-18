using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] float amount = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(amount);
                Destroy(gameObject); 
            }
        }
    }
}
