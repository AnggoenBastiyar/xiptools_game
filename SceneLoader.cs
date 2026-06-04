using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Fungsi untuk pindah scene berdasarkan nama
    public void LoadScene(string sceneName)
    {
        Debug.Log("LoadScene dipanggil! Nama scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Fungsi untuk keluar game (buat nanti)
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}