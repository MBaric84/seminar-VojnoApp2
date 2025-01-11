using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Compass : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Image compassImage;

    private float compassAddition = 90;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Tutorial")
            compassAddition = 90f;
        else
            compassAddition = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.CheckIfGamePaused())
        { return; }

        if (player != null && compassImage != null)
        {
            float rotation = player.eulerAngles.y;
            compassImage.rectTransform.localEulerAngles = new Vector3(0, 0, rotation+compassAddition);
        }
    }
}