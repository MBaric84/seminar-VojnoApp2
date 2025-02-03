using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkyMapManager : MonoBehaviour
{
    [SerializeField] private Transform _parentT; //object that will follow the camera - parent of sky map
    [SerializeField] private Transform _camT; //Player camera 
    [SerializeField] private Transform _playerT; //Player
    [SerializeField] private Transform _skyMapChildT;//sky map 

    private readonly float _radius = 100f; //radius of the circle around the player - offset (how much in front of the player is the map) 
    private Vector3 playerSpawnPos; //player spawn position
    private Vector3 skymapSpawnPos; //sky map start position


    public Slider heightSlider;
    public Slider offsetXSlider;
    public Slider offsetZSlider;
    public Slider rotationSlider;
    public Slider tiltSlider;


    private float userHeightOffset = 0f;
    private Vector3 userOffset = Vector3.zero;
    private float userRotationAngle = 0f;
    private float userTiltAngle = 0f;
    private string csvFilePath;

    void Start()
    {
        csvFilePath = Path.Combine(Application.persistentDataPath, "UserSettings.csv");

        // Initialize positions
        playerSpawnPos = _playerT.position;
        skymapSpawnPos = _skyMapChildT.localPosition;

        heightSlider.onValueChanged.AddListener((value) => {
            userHeightOffset = value;
            SaveSettings();
        });

        offsetXSlider.onValueChanged.AddListener((value) => {
            userOffset.x = value;
            SaveSettings();
        });

        offsetZSlider.onValueChanged.AddListener((value) => {
            userOffset.z = value;
            SaveSettings();
        });

        rotationSlider.onValueChanged.AddListener((value) => {
            userRotationAngle = value;
            SaveSettings();
        });

        tiltSlider.onValueChanged.AddListener((value) => { 
            userTiltAngle = value;
            SaveSettings();
        });

        LoadSettings();
        
        heightSlider.value = userHeightOffset;
        offsetXSlider.value = userOffset.x;
        offsetZSlider.value = userOffset.z;
        rotationSlider.value = userRotationAngle;
        tiltSlider.value = userTiltAngle;

        Debug.Log("Sliders initialized with values: " +
            $"Height: {heightSlider.value}, " +
            $"Offset: ({offsetXSlider.value}, {offsetZSlider.value}), " +
            $"Rotation: {rotationSlider.value}, " +
            $"Tilt: {tiltSlider.value}");
    }

    private void SaveSettings()
    {
        string data = $"{DateTime.Now}, {userHeightOffset}, {userOffset.x}, {userOffset.z}, {userRotationAngle}, {userTiltAngle}\n";
        File.AppendAllText(csvFilePath, data);
    }

    private void LoadSettings()
    {
        if (File.Exists(csvFilePath))
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            if (lines.Length > 0)
            {
                string[] lastLine = lines[lines.Length - 1].Split(',');
                if (lastLine.Length == 6)
                {
                    userHeightOffset = float.Parse(lastLine[1]);
                    userOffset = new Vector3(float.Parse(lastLine[2]), 0, float.Parse(lastLine[3]));
                    userRotationAngle = float.Parse(lastLine[4]);
                    userTiltAngle = float.Parse(lastLine[5]);
                }
            }
        }
    }

    
    void Update()
    {
        Vector3 newPosition = playerSpawnPos + new Vector3(userOffset.x, userHeightOffset, userOffset.z);
        Quaternion newRotation = Quaternion.Euler(userTiltAngle, userRotationAngle, 0);
        _parentT.SetPositionAndRotation(newPosition, newRotation);


        Debug.Log($"SkyMap Updated: Position {newPosition}, Rotation {newRotation.eulerAngles.y}");
    }
}