using UnityEngine;

public class VRCameraRig : MonoBehaviour
{
    [Header("Camera Rig")]
    public Transform cameraRigRoot;
    public Transform centerEyeAnchor;
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;
    
    [Header("Car Reference")]
    public Transform driverSeatPosition;
    
    void Start()
    {
        // Position the VR rig at the driver's seat
        if (driverSeatPosition != null)
        {
            transform.position = driverSeatPosition.position;
            transform.rotation = driverSeatPosition.rotation;
        }
    }
    
    void LateUpdate()
    {
        // Keep the rig locked to the car's driver seat
        if (driverSeatPosition != null)
        {
            transform.position = driverSeatPosition.position;
            transform.rotation = driverSeatPosition.rotation;
        }
    }
}
