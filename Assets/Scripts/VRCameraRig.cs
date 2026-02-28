using UnityEngine;

/// <summary>
/// Attaches the XR Origin to the car's driver seat so the player rides with the car.
/// Attach this script to the XR Origin GameObject, then assign the DriverSeat transform.
/// </summary>
public class VRCameraRig : MonoBehaviour
{
    [Header("Car Reference")]
    [Tooltip("Empty GameObject inside the Car, representing where the driver sits.")]
    public Transform driverSeatPosition;

    void Start()
    {
        SnapToSeat();
    }

    void LateUpdate()
    {
        SnapToSeat();
    }

    void SnapToSeat()
    {
        if (driverSeatPosition != null)
        {
            transform.position = driverSeatPosition.position;
            transform.rotation = driverSeatPosition.rotation;
        }
    }
}
