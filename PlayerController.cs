using UnityEngine;
using TMPro;
using UnityEngine.UI; // Tambahin di atas

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Health & Score")]
    public int maxHealth = 3;
    public int currentHealth;
    public int score = 0;

    [Header("UI References")]
    public Text healthText;
    public TextMeshProUGUI scoreText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded;
    private bool facingRight = true;
    private int totalEnemies;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateScoreUI();

        if (animator == null)
            animator = GetComponent<Animator>();

        // Hitung total musuh
        totalEnemies = FindObjectsOfType<Enemy>().Length;
        Debug.Log("Total musuh di level: " + totalEnemies);

        Debug.Log("Player siap! Nyawa: " + currentHealth + " | Score: " + score);
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    void Move(float moveInput)
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Shoot()
    {
        Debug.Log("SHOOT! Fire1 pressed");

        if (bulletPrefab == null)
        {
            Debug.LogError("BULLET PREFAB KOSONG! Drag prefab Bullet ke Player!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FIRE POINT KOSONG! Pastikan ada child FirePoint di Player!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb == null)
        {
            Debug.LogError("PREFAB BULLET TIDAK PUNYA RIGIDBODY2D!");
            return;
        }

        float direction = facingRight ? 1 : -1;
        bulletRb.linearVelocity = new Vector2(bulletSpeed * direction, 0);

        Debug.Log("Peluru ditembakkan! Arah: " + direction);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        StartCoroutine(FlashRed());

        Debug.Log("KENA! Nyawa: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("GAME OVER!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        if (animator != null)
            animator.SetTrigger("Die");
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log("Score +" + points + " | Total: " + score);
    }

    public void EnemyKilled()
    {
        totalEnemies--;
        Debug.Log("Musuh tersisa: " + totalEnemies);

        if (totalEnemies <= 0)
        {
            Win();
        }
    }
    void Win()
    {
        Debug.Log("KAMU MENANG!");
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0; // Pause game
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentScene == "Level1")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
        else if (currentScene == "Level2")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
        else if (currentScene == "Level3")
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Kembali setelah tamat
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        Time.timeScale = 1; // Unpause game
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        Debug.Log("RETRY! Load ulang: " + currentScene);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            string hearts = "";
            for (int i = 0; i < maxHealth; i++)
            {
                if (i < currentHealth)
                    hearts += "❤️ ";
                else
                    hearts += "🖤 ";
            }
            healthText.text = hearts;
            Debug.Log("HEALTH UI DIUPDATE: " + hearts + " | Nyawa: " + currentHealth);
        }
        else
        {
            Debug.LogError("HEALTH TEXT BELUM DI-DRAG! Drag Txt_Health ke Player!");
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sprite.color = Color.white;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            TakeDamage(1);

        if (other.CompareTag("Spike"))
            TakeDamage(1);

        if (other.CompareTag("Coin"))
        {
            AddScore(10);
            Destroy(other.gameObject);
        }
    }
}