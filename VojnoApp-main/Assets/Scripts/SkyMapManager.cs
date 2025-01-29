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
    public Slider offsetYSlider;
    public Slider offsetZSlider;
    public Slider rotationSlider;


    private float userHeightOffset = 0f;
    private Vector3 userOffset = Vector3.zero;
    private float userRotationAngle = 0f;
    private string csvFilePath;

    void Start()
    {
        csvFilePath = Path.Combine(Application.persistentDataPath, "UserSettings.csv");

        // Initialize positions
        playerSpawnPos = _playerT.position;
        skymapSpawnPos = _skyMapChildT.localPosition;

        // Add more verbose debug logging
        heightSlider.onValueChanged.AddListener((value) => {
            Debug.Log($"Height slider moved to: {value}");
            userHeightOffset = value;
            SaveSettings();
        });

        offsetXSlider.onValueChanged.AddListener((value) => {
            Debug.Log($"Offset X slider moved to: {value}");
            userOffset.x = value;
            SaveSettings();
        });

        offsetYSlider.onValueChanged.AddListener((value) => {
            Debug.Log($"Offset Y slider moved to: {value}");
            userOffset.y = value;
            SaveSettings();
        });

        offsetZSlider.onValueChanged.AddListener((value) => {
            Debug.Log($"Offset Z slider moved to: {value}, current userOffset.z: {userOffset.z}");
            userOffset.z = value;
            Debug.Log($"After update - userOffset.z: {userOffset.z}");
            SaveSettings();
        });

        rotationSlider.onValueChanged.AddListener((value) => {
            Debug.Log($"Rotation slider moved to: {value}, current userRotationAngle: {userRotationAngle}");
            userRotationAngle = value;
            Debug.Log($"After update - userRotationAngle: {userRotationAngle}");
            SaveSettings();
        });

        // Load settings after setting up listeners
        LoadSettings();

        // Initialize slider values
        heightSlider.value = userHeightOffset;
        offsetXSlider.value = userOffset.x;
        offsetYSlider.value = userOffset.y;
        offsetZSlider.value = userOffset.z;
        rotationSlider.value = userRotationAngle;

        Debug.Log("Sliders initialized with values: " +
            $"Height: {heightSlider.value}, " +
            $"Offset: ({offsetXSlider.value}, {offsetYSlider.value}, {offsetZSlider.value}), " +
            $"Rotation: {rotationSlider.value}");
    }

    private void SaveSettings()
    {
        string data = $"{DateTime.Now}, {userHeightOffset}, {userOffset.x}, {userOffset.y}, {userOffset.z}, {userRotationAngle}\n";
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
                    userOffset = new Vector3(float.Parse(lastLine[2]), float.Parse(lastLine[3]), float.Parse(lastLine[4]));
                    userRotationAngle = float.Parse(lastLine[5]);
                }
            }
        }
    }

    void Update()
    {
        // Add specific debug for Z offset and rotation
        Debug.Log($"Update - Z offset: {userOffset.z}, Rotation: {userRotationAngle}");
        
        Vector3 offset = new Vector3(
            Mathf.Sin((_camT.eulerAngles.y + userRotationAngle) * Mathf.Deg2Rad), 
            0, 
            Mathf.Cos((_camT.eulerAngles.y + userRotationAngle) * Mathf.Deg2Rad)
        ) * _radius;

        Vector3 totalOffset = offset + userOffset;

        Vector3 newPosition = new Vector3(
            _playerT.position.x + totalOffset.x,
            playerSpawnPos.y + userHeightOffset,
            _playerT.position.z + totalOffset.z
        );

        _parentT.position = newPosition;
        _skyMapChildT.localPosition = new Vector3(
            skymapSpawnPos.x + (playerSpawnPos.x - _playerT.position.x),
            skymapSpawnPos.y, // Keep original Y position
            skymapSpawnPos.z + (playerSpawnPos.z - _playerT.position.z)
        );

        Debug.Log($"Current Settings - Height: {userHeightOffset}, Offset: {userOffset}, Rotation: {userRotationAngle}");
    }
}