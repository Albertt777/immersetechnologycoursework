using UnityEngine;
using UnityEngine.XR;

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
    public XRNode leftHandNode = XRNode.LeftHand;
    
    [Header("Steering Wheel")]
    public Transform steeringWheel;
    public float steeringWheelRotationMultiplier = 3f;
    
    private float currentSpeed;
    private float motorInput;
    private float steeringInput;
    
    void Update()
    {
        GetVRInput();
        HandleSteering();
        HandleMotor();
        UpdateWheelMeshes();
        UpdateSteeringWheel();
    }
    
    void GetVRInput()
    {
        // Get trigger input for acceleration/brake from right controller
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightHandNode);
        rightDevice.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
        
        // Get joystick input for steering from left controller
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftHandNode);
        leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftJoystick);
        
        motorInput = rightTrigger;
        steeringInput = leftJoystick.x;
        
        // Alternative: Use grip for brake
        rightDevice.TryGetFeatureValue(CommonUsages.grip, out float brake);
        if (brake > 0.1f)
        {
            motorInput = -brake;
        }
    }
    
    void HandleSteering()
    {
        float steering = steeringAngle * steeringInput;
        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }
    
    void HandleMotor()
    {
        currentSpeed = 2 * Mathf.PI * frontLeftWheel.radius * frontLeftWheel.rpm * 60 / 1000;
        
        if (currentSpeed < maxSpeed && motorInput > 0)
        {
            frontLeftWheel.motorTorque = motorInput * acceleration * 500;
            frontRightWheel.motorTorque = motorInput * acceleration * 500;
        }
        else
        {
            frontLeftWheel.motorTorque = 0;
            frontRightWheel.motorTorque = 0;
        }
        
        // Apply brakes
        if (motorInput < 0)
        {
            ApplyBrakes(Mathf.Abs(motorInput) * brakeForce);
        }
        else
        {
            ApplyBrakes(0);
        }
    }
    
    void ApplyBrakes(float brakeAmount)
    {
        frontLeftWheel.brakeTorque = brakeAmount * 500;
        frontRightWheel.brakeTorque = brakeAmount * 500;
        rearLeftWheel.brakeTorque = brakeAmount * 500;
        rearRightWheel.brakeTorque = brakeAmount * 500;
    }
    
    void UpdateWheelMeshes()
    {
        UpdateWheelMesh(frontLeftWheel, frontLeftMesh);
        UpdateWheelMesh(frontRightWheel, frontRightMesh);
        UpdateWheelMesh(rearLeftWheel, rearLeftMesh);
        UpdateWheelMesh(rearRightWheel, rearRightMesh);
    }
    
    void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        if (mesh == null) return;
        
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        
        mesh.position = position;
        mesh.rotation = rotation;
    }
    
    void UpdateSteeringWheel()
    {
        if (steeringWheel != null)
        {
            steeringWheel.localRotation = Quaternion.Euler(
                0, 
                0, 
                -steeringInput * steeringAngle * steeringWheelRotationMultiplier
            );
        }
    }
}
