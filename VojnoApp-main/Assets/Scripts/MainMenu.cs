using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel; //main menu panel
    [SerializeField] private GameObject mapSettingsPanel; //map settings panel

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenMapSettings()
    {
        mainMenuPanel.SetActive(false);
        mapSettingsPanel.SetActive(true);
    }

    public void CloseMapSettings()
    {
        mapSettingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
