using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    [SerializeField] float damage = 1f;

    [SerializeField] GameObject bulletEffect;
    //[SerializeField] private AudioManager audioManager;
 
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Enemy"))
            {
                BlueWhale blueWhale = collision.GetComponent<BlueWhale>();
                PurpleWhale purpleWhale = collision.GetComponent<PurpleWhale>();
                RedWhale redWhale = collision.GetComponent<RedWhale>();
                Stone stone = collision.GetComponent<Stone>();
                Boss boss = collision.GetComponent<Boss>();

                GameObject effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
                effect.transform.parent = null;

                Destroy(effect, 0.5f);

                if (blueWhale != null)
                    blueWhale.TakeDamage(damage);
                    //audioManager.PlayEnemyHit();

                if (purpleWhale != null)
                    purpleWhale.TakeDamage(damage);
                    //audioManager.PlayEnemyHit();

                if (stone != null)
                    stone.TakeDamage(damage);
                    //audioManager.PlayEnemyHit();
                
                if (redWhale != null)
                    redWhale.TakeDamage(damage);
                    //audioManager.PlayEnemyHit();

                if (boss != null)
                    boss.TakeDamage(damage);
                    //audioManager.PlayEnemyHit();
                Destroy(gameObject);
            }
        }
    }
}
