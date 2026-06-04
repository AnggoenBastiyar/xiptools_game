using UnityEngine;

public class LevelPanelManager : MonoBehaviour
{
    // Panel yang akan muncul/hilang
    public GameObject levelPanel;

    // Dipanggil pas tombol PLAY diklik
    public void ShowLevelPanel()
    {
        levelPanel.SetActive(true);
        Debug.Log("Panel Level dibuka!");
    }

    // Dipanggil pas tombol CANCEL diklik
    public void HideLevelPanel()
    {
        levelPanel.SetActive(false);
        Debug.Log("Panel Level ditutup!");
    }
}