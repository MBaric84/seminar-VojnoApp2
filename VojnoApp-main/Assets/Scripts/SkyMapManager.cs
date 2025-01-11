using UnityEngine;

public class SkyMapManager : MonoBehaviour
{
    [SerializeField] private Transform _parentT; //object that will follow the camera - parent of sky map
    [SerializeField] private Transform _camT; //Player camera 
    [SerializeField] private Transform _playerT; //Player
    [SerializeField] private Transform _skyMapChildT;//sky map 

    private readonly float _radius = 100f; //radius of the circle around the player - offset (how much in front of the player is the map) 
    private Vector3 playerSpawnPos; //player spawn position
    private Vector3 skymapSpawnPos; //sky map start position

    void Start()
    {
        // save initial positions of both player and sky map
        playerSpawnPos = _playerT.position;
        skymapSpawnPos = _skyMapChildT.localPosition;
    }

    void Update()
    {
        // Calculate the offset position based on the camera's rotation and radius (point in cricle aorund the player where the map has to be positioned)
        // The offset is calculated using the sin and cos of the camera's rotation in radians
        // The offset is then multiplied by the radius to get the final offset
        Vector3 offset = new Vector3(Mathf.Sin(_camT.eulerAngles.y * Mathf.Deg2Rad), _parentT.position.y / _radius, Mathf.Cos(_camT.eulerAngles.y * Mathf.Deg2Rad)) * _radius;

        // Calculate the new position of the map parent object based on the player's position and the offset
        Vector3 newPosition = new Vector3(_playerT.position.x + offset.x, _parentT.position.y, _playerT.position.z + offset.z);

        // Set the parent object's position to the new calculated position
        _parentT.position = newPosition;

        //Move sky map child locally within parent according to player movement.
        //We are moving the parent with the player, therefore the map within the parent has to move in the opposite direction
        // This is done by subtracting the player's position from the initial player position - we are only moving the map for the delta of player movement
        _skyMapChildT.localPosition = new Vector3(skymapSpawnPos.x + (playerSpawnPos.x - _playerT.position.x), _skyMapChildT.localPosition.y, skymapSpawnPos.z + (playerSpawnPos.z - _playerT.position.z));
    }
}
