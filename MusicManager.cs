using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern: biar cuma ada 1 music manager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Music tetap jalan pas pindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurin kalau udah ada
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("Music OFF");
        }
        else
        {
            audioSource.Play();
            Debug.Log("Music ON");
        }
    }

    public bool IsMusicPlaying()
    {
        return audioSource.isPlaying;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}