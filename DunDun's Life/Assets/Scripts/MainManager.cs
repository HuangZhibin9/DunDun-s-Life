using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject RecordPanel;
    public GameObject MainPanel;
    public void NewStart()
    {
        SceneManager.LoadScene("FirstScene");
    }
    public void LoadGame()
    {
        RecordPanel.SetActive(true);
        MainPanel.SetActive(false);
    }

    public void Return()
    {
        RecordPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
