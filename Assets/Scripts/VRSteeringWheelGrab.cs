using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Physical steering wheel grab — attach to the SteeringWheel GameObject.
/// Requires XR Interaction Toolkit. The wheel must also have an XRGrabInteractable component.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class VRSteeringWheelGrab : MonoBehaviour
{
    [Header("Steering Wheel Settings")]
    public float maxRotation = 450f;
    public VRCarController carController;

    private XRGrabInteractable grabInteractable;
    private float currentRotation = 0f;
    private bool  isGrabbed       = false;
    private Vector3 lastHandPosition;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed        = true;
        lastHandPosition = args.interactorObject.transform.position;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void Update()
    {
        if (!isGrabbed || !grabInteractable.isSelected) return;

        var interactor           = grabInteractable.interactorsSelecting[0];
        Vector3 currentHandPos   = interactor.transform.position;

        Vector3 wheelCenter  = transform.position;
        Vector3 lastDir      = Vector3.ProjectOnPlane(lastHandPosition - wheelCenter, transform.forward);
        Vector3 currentDir   = Vector3.ProjectOnPlane(currentHandPos  - wheelCenter, transform.forward);

        float angle      = Vector3.SignedAngle(lastDir, currentDir, transform.forward);
        currentRotation  = Mathf.Clamp(currentRotation + angle, -maxRotation, maxRotation);

        transform.localRotation = Quaternion.Euler(0f, 0f, -currentRotation);

        // Pass to car controller
        // (VRCarController.steeringInput is private; expose via property or direct field if needed)

        lastHandPosition = currentHandPos;
    }

    public float GetNormalizedSteering()
    {
        return Mathf.Clamp(currentRotation / maxRotation, -1f, 1f);
    }
}
