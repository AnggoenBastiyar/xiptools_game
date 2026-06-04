using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 10;
    public float rotateSpeed = 50f;     // KECEPATAN PUTAR (kecilin)
    public float bobAmount = 0.05f;     // NAIK TURUN SEDIKIT (dulu 0.1f)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Putar di tempat (kecil)
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Naik turun dikit aja
        float newY = startPosition.y + Mathf.Sin(Time.time * 2f) * bobAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddScore(coinValue);
                Debug.Log("Koin diambil! Score +" + coinValue);
            }

            Destroy(gameObject);
        }
    }
}