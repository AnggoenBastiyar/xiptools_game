using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    private int currentHealth;
    public int damage = 1;
    public int scoreValue = 50;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private bool movingRight = true;
    private SpriteRenderer sprite;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        Debug.Log("Enemy siap! HP: " + currentHealth);
    }

    void Update()
    {
        if (isDead) return;
        Patrol();
    }

    void Patrol()
    {
        // Jalan terus ke kanan atau kiri
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
    }

    // BALIK ARAH pas kena BOUNDARY
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            movingRight = !movingRight;
            Flip();
            Debug.Log("Enemy balik arah!");
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Enemy kena! Sisa HP: " + currentHealth);

        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemy MATI! Score +" + scoreValue);

        // Kasih score
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.AddScore(scoreValue);
            player.EnemyKilled(); // Panggil fungsi cek menang
        }

        // Matikan collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.3f);
    }

    System.Collections.IEnumerator FlashWhite()
    {
        if (sprite != null)
        {
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.red;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}