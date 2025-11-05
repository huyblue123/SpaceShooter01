using UnityEngine;

public class BlueWhale : Whales
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool destroyWhenOffscreen = true;
    [SerializeField] private float offscreenX = -20f;

    [Header("Drop")]
    [SerializeField] private GameObject energy;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Di chuyển sang trái
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Hủy khi ra khỏi màn hình
        if (destroyWhenOffscreen && transform.position.x < offscreenX)
            Destroy(gameObject);
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg); 
        animator.SetTrigger("bluewhale_dmg");

        if (currentHp <= 0 && energy != null)
        {
            Instantiate(energy, transform.position, Quaternion.identity);
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
