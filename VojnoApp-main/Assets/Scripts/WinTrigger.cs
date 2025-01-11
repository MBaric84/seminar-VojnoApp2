using UnityEngine;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    private float requiredTime = 10.0f;
    private float timeSpentInTrigger = 0.0f;
    private bool isPlayerInside = false;

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside)
        {
            timeSpentInTrigger += Time.deltaTime;
            countdownText.text = (requiredTime - timeSpentInTrigger).ToString("F1");
            if (timeSpentInTrigger >= requiredTime)
            {
                WinGame();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            timeSpentInTrigger = 0;
            countdownText.text = "";
        }
    }

    private void WinGame()
    {
        countdownText.text = "";
        uiManager.TriggerWinScreen();
    }
}
