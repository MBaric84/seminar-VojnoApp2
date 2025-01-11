using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardDistaceRelevant : MonoBehaviour
{
    private Camera mainCamera;

    // Minimum and maximum scales for the object
    public float minScale = 0.1f;
    public float maxScale = 1.0f;

    // Minimum and maximum distances for scaling
    public float minDistance = 10.0f;
    public float maxDistance = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the object face the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);

        // Adjust the scale based on the distance to the camera
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Normalize the distance to a 0-1 range
        float normalizedDistance = Mathf.InverseLerp(minDistance, maxDistance, distance);

        // Calculate the scale based on the normalized distance
        float scale = Mathf.Lerp(minScale, maxScale, normalizedDistance);

        // Apply the scale to the object
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
