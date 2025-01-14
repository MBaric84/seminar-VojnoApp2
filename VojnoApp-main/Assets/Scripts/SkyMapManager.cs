using System;
using System.IO;
using UnityEngine;

public class SkyMapManager : MonoBehaviour
{
    [SerializeField] private Transform _parentT; //object that will follow the camera - parent of sky map
    [SerializeField] private Transform _camT; //Player camera 
    [SerializeField] private Transform _playerT; //Player
    [SerializeField] private Transform _skyMapChildT;//sky map 

    [Header("Custom limits")]
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 20f;
    [SerializeField] private float maxOffset = 50f;

    [Header("Default values")]
    [SerializeField] private float defaultHeight = 10f;
    [SerializeField] private Vector3 defaultOffset = Vector3.zero;
    [SerializeField] private float defaultRotation = 0f;

    [Header("Test Adjustments")]
    [Range(0f, 20f)]
    [SerializeField] private float testHeight;


    private readonly float _radius = 100f; //radius of the circle around the player - offset (how much in front of the player is the map) 
    private Vector3 playerSpawnPos; //player spawn position
    private Vector3 skymapSpawnPos; //sky map start position

    private float mapHeight; //height of the map
    private Vector3 mapOffset; //offset of the map
    private float mapRotationAngle; //angle of the map
    private float lastTestHeight;	
    private string csvFilePath;
    void Start()
    {
        // save initial positions of both player and sky map
        playerSpawnPos = _playerT.position;
        skymapSpawnPos = _skyMapChildT.localPosition;
        lastTestHeight = testHeight;

        csvFilePath = Path.Combine(Application.persistentDataPath, "UserSettings.csv");
        if (File.Exists(csvFilePath))
        {
            File.WriteAllText(csvFilePath, "Timestamp, Height, OffsetX, OffsetY, OffsetZ, RotationAngle\n");
        }
    }


    void Update()
    {
        // Calculate the offset position based on the camera's rotation and radius (point in cricle aorund the player where the map has to be positioned)
        // The offset is calculated using the sin and cos of the camera's rotation in radians
        // The offset is then multiplied by the radius to get the final offset
        Vector3 offset = new Vector3(Mathf.Sin(_camT.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(_camT.eulerAngles.y * Mathf.Deg2Rad)) * _radius;

        // Calculate the new position of the map parent object based on the player's position and the offset
        Vector3 newPosition = new Vector3(_playerT.position.x + offset.x + mapOffset.x, _parentT.position.y, _playerT.position.z + offset.z + mapOffset.z);

        // Set the parent object's position to the new calculated position
        _parentT.position = newPosition;

        //Move sky map child locally within parent according to player movement.
        //We are moving the parent with the player, therefore the map within the parent has to move in the opposite direction
        // This is done by subtracting the player's position from the initial player position - we are only moving the map for the delta of player movement
        _skyMapChildT.localPosition = new Vector3(skymapSpawnPos.x + (playerSpawnPos.x - _playerT.position.x), _skyMapChildT.localPosition.y, skymapSpawnPos.z + (playerSpawnPos.z - _playerT.position.z));

        _parentT.rotation = Quaternion.Euler(0, mapRotationAngle, 0);

    }

    public void AdjustHeight(float height)
    {
        mapHeight= Mathf.Clamp(height, minHeight, maxHeight);
        if (Application.isPlaying)
        {
            SaveSettings();
        }
    }

    public void AdjustOffset(Vector3 offset)
    {
        mapOffset = Vector3.ClampMagnitude(offset, maxOffset);
        SaveSettings();
    }

    public void AdjustRotation(float angle) 
    {
        mapRotationAngle = angle % 360;
        SaveSettings();
    }

    public void ResetSettings()
    {
        mapHeight = defaultHeight;
        mapOffset = defaultOffset;
        mapRotationAngle = defaultRotation;
        SaveSettings();
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            AdjustHeight(testHeight);
        }
        else
            {
                mapHeight = Mathf.Clamp(testHeight, minHeight, maxHeight);
            }
        lastTestHeight = testHeight;
    }

    private void SaveSettings()
    {
        if (Application.isPlaying && !string.IsNullOrEmpty(csvFilePath))
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string data = $"{timestamp}, {mapHeight}, {mapOffset.x}, {mapOffset.y}, {mapOffset.z}, {mapRotationAngle}\n";
            File.AppendAllText(csvFilePath, data);
            Debug.Log("Settings saved.");
        }
    }
}
