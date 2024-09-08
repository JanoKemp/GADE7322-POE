using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform focusObject;   // The object the camera will rotate around
    public float rotationSpeed = 100f;  // Speed of camera rotation
    private float distanceFromObject = 20f;  // Distance from the object
    private float height = 14f;  // Fixed height from the ground

    public Vector3 offset;

    void Start()
    {
        // Just set for debuging sake
        offset = transform.position;
        
    }
    
    void Update()
    {
        // Fetches Centre Cube after generation from WorldController
        GameObject centreCube = GameObject.FindGameObjectWithTag("WorldController").GetComponent<LandscapeGeneration>().centreCube;
        
        focusObject = centreCube.transform;
        offset = transform.position - focusObject.position; // Calculates the offset position from the focusPoint
        
        // rotate around the focusObject on pressing 'A' (left) and 'D' (right)
        if (Input.GetKey(KeyCode.A))
        {
            RotateAroundObject(-1);  // counterclockwise
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotateAroundObject(1);   // clockwise
        }
    }

    void RotateAroundObject(int direction)
    {
        // Rotates around the focusObject (CentreCube)
        transform.RotateAround(focusObject.position, Vector3.up, direction * rotationSpeed * Time.deltaTime);

        // Maintain the camera's distance from the object
        Vector3 desiredPosition = focusObject.position + (transform.position - focusObject.position).normalized * distanceFromObject;

        // Maintain the camera's height
        desiredPosition.y = focusObject.position.y + height;

        // Set the camera's position to the desired position
        transform.position = desiredPosition;

        // Make sure the camera is always looking at the object
        transform.LookAt(focusObject);
    }
}
