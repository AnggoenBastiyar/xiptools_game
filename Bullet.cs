using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet kena: " + other.gameObject.name + " | Tag: " + other.tag);

        // Kena musuh
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Musuh terkena damage!");
            }
            else
            {
                Debug.LogError("Enemy script TIDAK DITEMUKAN di object: " + other.name);
            }
            Destroy(gameObject);
        }

        // Kena ground
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}