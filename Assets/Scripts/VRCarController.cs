using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// VR Car Controller - Works with VR headsets (Meta Quest, PCVR) and in-editor keyboard.
/// 
/// CONTROLS:
///   VR:       Right Trigger = Accelerate | Right Grip = Brake | Left Joystick = Steer
///   Keyboard: W/Up = Accelerate | S/Down = Brake | A-D/Left-Right = Steer
/// </summary>
public class VRCarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxSpeed = 100f;
    public float acceleration = 20f;
    public float brakeForce = 50f;
    public float steeringAngle = 30f;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("VR Input")]
    public XRNode rightHandNode = XRNode.RightHand;
    public XRNode leftHandNode  = XRNode.LeftHand;

    [Header("Steering Wheel (Optional)")]
    public Transform steeringWheel;
    public float steeringWheelRotationMultiplier = 3f;

    [Header("Debug Info (Read-Only)")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float motorInput;
    [SerializeField] private float steeringInput;
    [SerializeField] private bool vrDeviceDetected;

    void Update()
    {
        GetInput();
        HandleSteering();
        HandleMotor();
        UpdateWheelMeshes();
        UpdateSteeringWheelVisual();
    }

    void GetInput()
    {
        // --- Try VR Input first ---
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightHandNode);
        InputDevice leftDevice  = InputDevices.GetDeviceAtXRNode(leftHandNode);

        vrDeviceDetected = rightDevice.isValid;

        if (rightDevice.isValid && leftDevice.isValid)
        {
            rightDevice.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
            rightDevice.TryGetFeatureValue(CommonUsages.grip,    out float rightGrip);
            leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftJoystick);

            motorInput    = rightTrigger;
            steeringInput = leftJoystick.x;

            if (rightGrip > 0.1f)
                motorInput = -rightGrip; // grip = brake (overrides trigger)
        }
        else
        {
            // --- Keyboard fallback for in-editor testing ---
            motorInput    = Input.GetAxis("Vertical");   // W/S or Up/Down
            steeringInput = Input.GetAxis("Horizontal"); // A/D or Left/Right
        }
    }

    void HandleSteering()
    {
        float steering = steeringAngle * steeringInput;
        frontLeftWheel.steerAngle  = steering;
        frontRightWheel.steerAngle = steering;
    }

    void HandleMotor()
    {
        // Calculate speed in km/h
        currentSpeed = 2f * Mathf.PI * frontLeftWheel.radius * frontLeftWheel.rpm * 60f / 1000f;

        if (motorInput > 0f)
        {
            if (currentSpeed < maxSpeed)
            {
                float torque = motorInput * acceleration * 500f;
                frontLeftWheel.motorTorque  = torque;
                frontRightWheel.motorTorque = torque;
            }
            else
            {
                frontLeftWheel.motorTorque  = 0f;
                frontRightWheel.motorTorque = 0f;
            }
            ApplyBrakes(0f);
        }
        else if (motorInput < 0f)
        {
            frontLeftWheel.motorTorque  = 0f;
            frontRightWheel.motorTorque = 0f;
            ApplyBrakes(Mathf.Abs(motorInput) * brakeForce);
        }
        else
        {
            frontLeftWheel.motorTorque  = 0f;
            frontRightWheel.motorTorque = 0f;
            ApplyBrakes(0f);
        }
    }

    void ApplyBrakes(float brakeAmount)
    {
        float torque = brakeAmount * 500f;
        frontLeftWheel.brakeTorque  = torque;
        frontRightWheel.brakeTorque = torque;
        rearLeftWheel.brakeTorque   = torque;
        rearRightWheel.brakeTorque  = torque;
    }

    void UpdateWheelMeshes()
    {
        UpdateWheelMesh(frontLeftWheel,  frontLeftMesh);
        UpdateWheelMesh(frontRightWheel, frontRightMesh);
        UpdateWheelMesh(rearLeftWheel,   rearLeftMesh);
        UpdateWheelMesh(rearRightWheel,  rearRightMesh);
    }

    void UpdateWheelMesh(WheelCollider col, Transform mesh)
    {
        if (mesh == null || col == null) return;
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    void UpdateSteeringWheelVisual()
    {
        if (steeringWheel != null)
        {
            steeringWheel.localRotation = Quaternion.Euler(
                0f,
                0f,
                -steeringInput * steeringAngle * steeringWheelRotationMultiplier
            );
        }
    }
}
