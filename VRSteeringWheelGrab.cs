using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSteeringWheelGrab : MonoBehaviour
{
    [Header("Steering Wheel Settings")]
    public Transform steeringWheel;
    public float maxRotation = 450f; // Maximum degrees wheel can turn
    public VRCarController carController;
    
    [Header("Grab Points")]
    public Transform leftGrabPoint;
    public Transform rightGrabPoint;
    
    private XRGrabInteractable grabInteractable;
    private float currentRotation = 0f;
    private bool isGrabbed = false;
    private Vector3 lastHandPosition;
    
    void Start()
    {
        // Add XR Grab Interactable component if not present
        grabInteractable = steeringWheel.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = steeringWheel.gameObject.AddComponent<XRGrabInteractable>();
        }
        
        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }
    
    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        lastHandPosition = args.interactorObject.transform.position;
    }
    
    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }
    
    void Update()
    {
        if (isGrabbed && grabInteractable.isSelected)
        {
            // Get current hand position
            var interactor = grabInteractable.interactorsSelecting[0];
            Vector3 currentHandPosition = interactor.transform.position;
            
            // Calculate rotation based on hand movement
            Vector3 wheelCenter = steeringWheel.position;
            Vector3 lastDirection = lastHandPosition - wheelCenter;
            Vector3 currentDirection = currentHandPosition - wheelCenter;
            
            // Project to wheel plane
            Vector3 wheelForward = steeringWheel.forward;
            lastDirection = Vector3.ProjectOnPlane(lastDirection, wheelForward);
            currentDirection = Vector3.ProjectOnPlane(currentDirection, wheelForward);
            
            // Calculate angle
            float angle = Vector3.SignedAngle(lastDirection, currentDirection, wheelForward);
            
            // Update rotation
            currentRotation += angle;
            currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);
            
            // Apply rotation to steering wheel
            steeringWheel.localRotation = Quaternion.Euler(0, 0, -currentRotation);
            
            // Send steering input to car
            if (carController != null)
            {
                float normalizedSteering = currentRotation / maxRotation;
                // This would need to be integrated with the car controller
                // For now, this demonstrates the concept
            }
            
            lastHandPosition = currentHandPosition;
        }
    }
    
    public float GetNormalizedSteering()
    {
        return Mathf.Clamp(currentRotation / maxRotation, -1f, 1f);
    }
}
