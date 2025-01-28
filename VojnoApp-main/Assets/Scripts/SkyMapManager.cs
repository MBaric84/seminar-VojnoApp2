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

        playerSpawnPos = _playerT.position;
        skymapSpawnPos = _skyMapChildT.localPosition;


        heightSlider.onValueChanged.AddListener((value) => {
            userHeightOffset = value;
            Debug.Log($"Height changed to: {value}");
            SaveSettings();
        });

        offsetXSlider.onValueChanged.AddListener((value) => {
            userOffset.x = value;
            Debug.Log($"Offset X changed to: {value}");
            SaveSettings();
        });

        offsetYSlider.onValueChanged.AddListener((value) => {
            userOffset.y = value;
            Debug.Log($"Offset Y changed to: {value}");
            SaveSettings();
        });

        offsetZSlider.onValueChanged.AddListener((value) => {
            userOffset.z = value;
            Debug.Log($"Offset Z changed to: {value}");
            SaveSettings();
        });

        rotationSlider.onValueChanged.AddListener((value) => {
            userRotationAngle = value;
            Debug.Log($"Rotation changed to: {value}");
            SaveSettings();
        });


        LoadSettings();


        heightSlider.value = userHeightOffset;
        offsetXSlider.value = userOffset.x;
        offsetYSlider.value = userOffset.y;
        offsetZSlider.value = userOffset.z;
        rotationSlider.value = userRotationAngle;
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
        Vector3 offset = new Vector3(
            Mathf.Sin((_camT.eulerAngles.y + userRotationAngle) * Mathf.Deg2Rad), 
            0, 
            Mathf.Cos((_camT.eulerAngles.y + userRotationAngle) * Mathf.Deg2Rad)
        ) * _radius;


        offset += userOffset;

        Vector3 newPosition = new Vector3(
            _playerT.position.x + offset.x, 
            _parentT.position.y + userHeightOffset, 
            _playerT.position.z + offset.z
        );


        _parentT.position = newPosition;
        _skyMapChildT.localPosition = new Vector3(
            skymapSpawnPos.x + (playerSpawnPos.x - _playerT.position.x),
            _skyMapChildT.localPosition.y,
            skymapSpawnPos.z + (playerSpawnPos.z - _playerT.position.z)
        );


        Debug.Log($"Current Settings - Height: {userHeightOffset}, Offset: {userOffset}, Rotation: {userRotationAngle}");
    }
}