using UnityEngine;

public class ViewDependentTransparency : MonoBehaviour
{
    public Material targetMaterial;
    private float minimumAngle = 60;
    private float maximumAngle = 80;
    private Camera mainCamera;
    public bool lookingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetMaterial != null)
        {
            float angle;
            if (lookingUp)
            {
                angle = Vector3.Angle(mainCamera.transform.forward, Vector3.up);
            }
            else
            {
                angle = Vector3.Angle(mainCamera.transform.forward, Vector3.down);
            }

            float normalizedAngle = (angle - minimumAngle) / (maximumAngle - minimumAngle);
            normalizedAngle = Mathf.Clamp01(normalizedAngle); // Ensure it stays within 0 to 1   
            float alpha = 1 - normalizedAngle;

            if (targetMaterial != null && targetMaterial.shader.name == "Unlit/MapTransparentShader")
            {
                targetMaterial.SetFloat("_Transparency", alpha);
            }
            else
            {
                Color currentColor = targetMaterial.GetColor("_Color");
                currentColor.a = alpha;
                targetMaterial.SetColor("_Color", currentColor);
            }
        }
    }
}
