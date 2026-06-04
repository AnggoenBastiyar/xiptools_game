using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;

    public void OpenInfo()
    {
        infoPanel.SetActive(true);
        Debug.Log("Info dibuka!");
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        Debug.Log("Info ditutup!");
    }
}