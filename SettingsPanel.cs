using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle musicToggle;
    public TextMeshProUGUI musicStatusText;

    void Start()
    {
        // Update toggle sesuai status music
        if (MusicManager.instance != null)
        {
            musicToggle.isOn = MusicManager.instance.IsMusicPlaying();
        }

        // Tambah listener ke toggle
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Debug.Log("Settings dibuka!");
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Debug.Log("Settings ditutup!");
    }

    void OnMusicToggleChanged(bool isOn)
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.ToggleMusic();
            musicStatusText.text = isOn ? "ON" : "OFF";
        }
    }
}