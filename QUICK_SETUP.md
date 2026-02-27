# Quick Setup Guide - VR Car Driving Scene

## Unity Hierarchy Structure

```
Scene
├── XR Origin (VR Camera Rig)
│   ├── Camera Offset
│   │   ├── Main Camera
│   │   ├── LeftHand Controller
│   │   └── RightHand Controller
│   └── [VRCameraRig.cs attached]
│
├── Car
│   ├── CarBody (Box Collider)
│   ├── DriverSeat (Empty Transform)
│   ├── SteeringWheel (Cylinder)
│   ├── FrontLeftWheel (Wheel Collider)
│   │   └── WheelMesh (Cylinder visual)
│   ├── FrontRightWheel (Wheel Collider)
│   │   └── WheelMesh (Cylinder visual)
│   ├── RearLeftWheel (Wheel Collider)
│   │   └── WheelMesh (Cylinder visual)
│   └── RearRightWheel (Wheel Collider)
│       └── WheelMesh (Cylinder visual)
│   [Rigidbody attached]
│   [VRCarController.cs attached]
│
├── Ground (Plane with Collider)
│
└── Directional Light
```

## Component Settings Reference

### Car Rigidbody
```
Mass: 1500
Drag: 0.05
Angular Drag: 0.5
Use Gravity: ✓
Is Kinematic: ✗
Interpolate: Interpolate
Collision Detection: Continuous
```

### Wheel Collider Settings
```
Mass: 20
Radius: 0.3
Wheel Damping Rate: 0.25
Suspension Distance: 0.2
Force App Point Distance: 0

Spring:
  - Spring: 35000
  - Damper: 4500
  - Target Position: 0.5

Forward Friction:
  - Extremum Slip: 0.4
  - Extremum Value: 1
  - Asymptote Slip: 0.8
  - Asymptote Value: 0.5
  - Stiffness: 1

Sideways Friction:
  - Extremum Slip: 0.2
  - Extremum Value: 1
  - Asymptote Slip: 0.5
  - Asymptote Value: 0.75
  - Stiffness: 1
```

### VRCarController Inspector Settings
```
Max Speed: 100
Acceleration: 20
Brake Force: 50
Steering Angle: 30

Wheel Colliders:
  - Front Left Wheel: [assign]
  - Front Right Wheel: [assign]
  - Rear Left Wheel: [assign]
  - Rear Right Wheel: [assign]

Wheel Meshes:
  - Front Left Mesh: [assign]
  - Front Right Mesh: [assign]
  - Rear Left Mesh: [assign]
  - Rear Right Mesh: [assign]

VR Input:
  - Right Hand Node: RightHand
  - Left Hand Node: LeftHand

Steering Wheel:
  - Steering Wheel: [assign transform]
  - Steering Wheel Rotation Multiplier: 3
```

## Step-by-Step Scene Creation

### 1. Ground Setup
```
Create → 3D Object → Plane
Name: Ground
Position: (0, 0, 0)
Scale: (10, 1, 10)
Add Material: Choose a road texture or dark gray
```

### 2. Car Body
```
Create → 3D Object → Cube
Name: Car
Position: (0, 1, 0)
Scale: (2, 0.8, 4)
Add Component → Rigidbody (settings above)
Add Component → VRCarController script
```

### 3. Wheels (Repeat 4 times)
```
Create → Create Empty
Name: FrontLeftWheel (adjust for each wheel)
Positions:
  - FrontLeft: (-0.8, 0.5, 1.5)
  - FrontRight: (0.8, 0.5, 1.5)
  - RearLeft: (-0.8, 0.5, -1.5)
  - RearRight: (0.8, 0.5, -1.5)

Add Component → Wheel Collider (settings above)

Create child:
  - Create → 3D Object → Cylinder
  - Name: WheelMesh
  - Position: (0, 0, 0)
  - Rotation: (0, 0, 90)
  - Scale: (0.6, 0.1, 0.6)
```

### 4. Driver Seat
```
In Car GameObject:
Create → Create Empty
Name: DriverSeat
Position: (0, 0.5, 0) (relative to Car)
```

### 5. Steering Wheel (Optional)
```
In Car GameObject:
Create → 3D Object → Cylinder
Name: SteeringWheel
Position: (0, 0.8, 0.5) (relative to DriverSeat)
Rotation: (90, 0, 0)
Scale: (0.4, 0.05, 0.4)
```

### 6. VR Camera Rig
```
Delete Main Camera

Create → XR → XR Origin (Action-based)
This creates the complete VR rig structure

Add Component → VRCameraRig script
  - Assign DriverSeat to "Driver Seat Position"
```

### 7. Assign Components
```
Select Car GameObject
In VRCarController:
  - Drag each Wheel Collider to corresponding slot
  - Drag each WheelMesh to corresponding mesh slot
  - Drag SteeringWheel to steering wheel slot
```

## Testing Checklist

- [ ] XR Plugin Management enabled in Project Settings
- [ ] XR Interaction Toolkit installed
- [ ] All Wheel Colliders assigned in VRCarController
- [ ] All Wheel Meshes assigned
- [ ] Rigidbody on Car with proper settings
- [ ] Ground has collider
- [ ] VR Origin has Camera, LeftHand, RightHand controllers
- [ ] DriverSeat assigned in VRCameraRig

## Quick Test (Without VR)
1. Modify VRCarController.cs to accept keyboard input temporarily:
```csharp
// In Update() method, add:
motorInput = Input.GetAxis("Vertical");
steeringInput = Input.GetAxis("Horizontal");
```
2. Press Play and use arrow keys/WASD to test car physics

## Build Settings for VR

### For Meta Quest:
```
File → Build Settings
Platform: Android
Switch Platform

Player Settings:
  - Company Name: Your Name
  - Product Name: VR Car Driving
  - Minimum API Level: Android 10.0
  - XR Plug-in Management → Android → Oculus ✓
```

### For PCVR (SteamVR):
```
File → Build Settings
Platform: Windows
Switch Platform

Player Settings:
  - XR Plug-in Management → PC → OpenXR ✓
  - Add OpenXR Interaction Profiles
```

Now you're ready to build and test on your VR headset!
