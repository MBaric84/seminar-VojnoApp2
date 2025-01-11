using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    // public GameObject activePlayerScreen;
    public GameObject ammo;
    public GameObject compass;
    public GameObject health;
    public GameObject countdown;

    
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject pauseScreen;

    public GameObject winScreenBackround;
    public GameObject loseScreenBackround;
    public GameObject pauseScreenBackround;


    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void TriggerWinScreen()
    {
        gameManager.ChangePauseGameState(true);
        // activePlayerScreen.SetActive(false);
        SetActivePlayerScreen(false);
        winScreenBackround.SetActive(true);
        winScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TriggerLoseScreen()
    {
        gameManager.ChangePauseGameState(true);
        // activePlayerScreen.SetActive(false);
        SetActivePlayerScreen(false);
        loseScreenBackround.SetActive(true);
        loseScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
        gameManager.ChangePauseGameState(true);
        // activePlayerScreen.SetActive(false);
        SetActivePlayerScreen(false);
        pauseScreenBackround.SetActive(true);
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseScreenBackround.SetActive(false);
        pauseScreen.SetActive(false);
        // activePlayerScreen.SetActive(true);
        SetActivePlayerScreen(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameManager.ChangePauseGameState(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void SetActivePlayerScreen(bool isActive)
    {
        ammo.SetActive(isActive);
        compass.SetActive(isActive);
        health.SetActive(isActive);
        countdown.SetActive(isActive);
    }
}
