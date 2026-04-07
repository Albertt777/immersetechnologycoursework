# VR Car Driving for Unity

This template provides a complete VR car driving experience for Unity Hub with XR support.

## Requirements

- Unity 2021.3 LTS or newer
- XR Interaction Toolkit (install via Package Manager)
- XR Plugin Management (install via Package Manager)
- A VR headset (Meta Quest, PCVR, etc.)

## Installation Steps

### 1. Create New Unity Project
1. Open Unity Hub
2. Create a new 3D project (URP recommended for better performance)
3. Name it "VR Car Driving"

### 2. Install Required Packages

1. Go to **Window → Package Manager**
2. Install the following packages (from Unity Registry):
   - **XR Plugin Management**
   - **XR Interaction Toolkit** (version 2.0 or newer)
   - **OpenXR Plugin** (or Oculus XR Plugin for Meta Quest)

3. Configure XR:
   - Go to **Edit → Project Settings → XR Plug-in Management**
   - Enable your VR platform (OpenXR recommended, or Oculus for Quest)
   - Under OpenXR, add interaction profiles for your controllers

### 3. Set Up the Scene

#### Create the Car GameObject:

1. Create an empty GameObject, name it "Car"
2. Add a **Rigidbody** component:
   - Mass: 1500
   - Drag: 0.05
   - Angular Drag: 0.5
   - Enable "Use Gravity"

3. Add a **Box Collider** for the car body

4. Create child GameObjects for wheels:
   - FrontLeftWheel (position: -0.8, -0.3, 1.5)
   - FrontRightWheel (position: 0.8, -0.3, 1.5)
   - RearLeftWheel (position: -0.8, -0.3, -1.5)
   - RearRightWheel (position: 0.8, -0.3, -1.5)

5. Add **Wheel Collider** to each wheel GameObject:
   - Mass: 20
   - Radius: 0.3
   - Wheel Damping Rate: 0.25
   - Suspension Distance: 0.2
   - Force App Point Distance: 0
   - Spring: 35000
   - Damper: 4500
   - Target Position: 0.5

6. Create visual wheel meshes (cylinders):
   - Add Cylinder meshes as children of each wheel GameObject
   - Scale: (0.6, 0.1, 0.6)
   - Rotate: (0, 0, 90)

#### Create Driver Seat Position:

1. Inside the Car GameObject, create an empty GameObject called "DriverSeat"
2. Position it at (0, 0.5, 0) - adjust based on your car model
3. This is where the VR camera will be positioned

#### Create VR Camera Rig:

1. Delete the Main Camera
2. Create an empty GameObject called "XR Origin"
3. Add the **XR Origin** component
4. Create child GameObjects:
   - "Camera Offset" (empty GameObject)
   - Under Camera Offset:
     - "Main Camera" (add Camera component and **Tracked Pose Driver**)
     - "LeftHand Controller" (add **XR Controller** component, set to Left Hand)
     - "RightHand Controller" (add **XR Controller** component, set to Right Hand)

#### Optional: Create Steering Wheel:

1. Inside Car, create a Cylinder GameObject called "SteeringWheel"
2. Position: (0, 0.8, 0.5) relative to DriverSeat
3. Scale: (0.4, 0.05, 0.4)
4. Rotate: (90, 0, 0)

### 4. Add Scripts

1. Copy **VRCarController.cs** to your Assets/Scripts folder
2. Copy **VRCameraRig.cs** to your Assets/Scripts folder

3. Attach **VRCarController** to the Car GameObject:
   - Assign all Wheel Colliders
   - Assign all Wheel Meshes
   - Assign Steering Wheel (if created)
   - Set maxSpeed, acceleration, etc.

4. Attach **VRCameraRig** to the XR Origin:
   - Assign DriverSeat to "Driver Seat Position"

### 5. Create a Ground/Road

1. Create a Plane GameObject (3D Object → Plane)
2. Scale it: (10, 1, 10)
3. Position: (0, 0, 0)
4. Add a material or texture for the road

### 6. Add Input Actions (XR Interaction Toolkit 2.0+)

1. Go to **Window → Package Manager → XR Interaction Toolkit → Samples**
2. Import "Starter Assets"
3. This adds pre-configured Input Actions for VR controllers

## Controls

- **Right Controller Trigger**: Accelerate
- **Right Controller Grip**: Brake
- **Left Controller Joystick (Horizontal)**: Steering

## Testing

### In Unity Editor:
1. Press Play
2. Use the XR Device Simulator (Window → XR → XR Device Simulator)
3. Control with keyboard/mouse to test before deploying to headset

### On VR Headset:
1. Build Settings → Add Open Scenes
2. Switch platform to Android (Quest) or Windows (PCVR)
3. Build and Run

## Customization Tips

### Adjust Car Physics:
- Modify Rigidbody mass for heavier/lighter feel
- Adjust Wheel Collider spring/damper for suspension
- Change maxSpeed and acceleration in VRCarController

### Add Car Models:
- Import 3D car models from Asset Store
- Replace the basic shapes with your model
- Ensure wheel meshes align with Wheel Colliders

### Add Sound:
- Add Audio Source components for engine sounds
- Pitch engine sound based on currentSpeed

### Add Environment:
- Import environment assets
- Add obstacles, buildings, scenery
- Create multiple road layouts

## Troubleshooting

**Car not moving?**
- Check Wheel Colliders are assigned
- Verify Rigidbody is not set to Kinematic
- Check ground has a collider

**VR not working?**
- Verify XR Plugin Management is enabled
- Check headset is connected and recognized
- Import XR Interaction Toolkit samples

**Steering not responsive?**
- Check controller input is being received
- Adjust steeringAngle value
- Verify left controller joystick is configured

**Car flips over?**
- Lower center of mass in Rigidbody
- Increase suspension spring strength
- Reduce turning speed

## Additional Features to Add

1. **Gear System**: Add manual/automatic transmission
2. **Speedometer UI**: Display speed in VR canvas
3. **Multiple Cars**: Create car selection system
4. **AI Traffic**: Add other cars on the road
5. **Checkpoints**: Create race track with timing
6. **Hand Tracking**: Use hand positions for steering wheel interaction
7. **Multiplayer**: Add networking for multiplayer racing

## Resources

- Unity XR Documentation: https://docs.unity3d.com/Manual/XR.html
- XR Interaction Toolkit: https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest
- Unity Forums: https://forum.unity.com/

---

Created for Unity Hub VR Car Driving Template
