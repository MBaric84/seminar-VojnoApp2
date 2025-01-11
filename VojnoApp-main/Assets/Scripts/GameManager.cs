using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private UIManager uiManager;

    private bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                uiManager.ResumeGame();
            }
            else
            {
                uiManager.PauseGame();
            }
        }
    }

    public bool CheckIfGamePaused()
    {
        return isGamePaused;
    }

    public void ChangePauseGameState(bool value)
    {
        isGamePaused = value;
    }
}
