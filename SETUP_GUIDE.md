# VR Car Driving — Unity Hub Setup Guide

## Opening in Unity Hub

1. Open **Unity Hub**
2. Click **Open** → **Add project from disk**
3. Navigate to and select the `VRCarDriving_UnityProject` folder
4. Choose **Unity 2022.3 LTS** (or 2021.3 LTS) as the editor version
5. Click **Open** — Unity will import the project

---

## First-Time Package Setup (inside Unity Editor)

Once the project opens, the `Packages/manifest.json` will automatically request:

| Package | Version | Purpose |
|---|---|---|
| XR Interaction Toolkit | 2.5.2 | VR hand controllers & grabbing |
| XR Plugin Management | 4.4.0 | Headset platform support |
| OpenXR Plugin | 1.9.1 | Meta Quest / PCVR support |
| Input System | 1.7.0 | New input backend |
| Universal RP | 14.0.9 | Better graphics performance |

Unity will download and install these automatically. **Wait for import to finish.**

### Import XR Toolkit Starter Assets
1. **Window → Package Manager**
2. Find **XR Interaction Toolkit** → **Samples** tab
3. Click **Import** next to **Starter Assets**
4. This adds preconfigured input action maps for VR controllers

---

## Configure XR Plug-in Management

1. **Edit → Project Settings → XR Plug-in Management**
2. **PC tab**: Check ✅ **OpenXR** (for PCVR / SteamVR)
3. **Android tab**: Check ✅ **Oculus** (for Meta Quest)
4. Under **OpenXR → Interaction Profiles**, add:
   - Oculus Touch Controller Profile
   - Meta Quest Touch Pro Controller Profile (if applicable)

---

## Build the Scene Hierarchy

Open the scene: **Assets/Scenes/VRCarDriving.unity**, then build the following hierarchy:

```
Scene
├── Directional Light              ← already in scene
├── Ground                         ← Plane, Scale (10,1,10)
├── Car                            ← Cube + Rigidbody + VRCarController.cs
│   ├── FrontLeftWheel             ← Empty + WheelCollider
│   │   └── WheelMesh              ← Cylinder (visual)
│   ├── FrontRightWheel
│   │   └── WheelMesh
│   ├── RearLeftWheel
│   │   └── WheelMesh
│   ├── RearRightWheel
│   │   └── WheelMesh
│   ├── DriverSeat                 ← Empty, pos (0, 0.5, 0)
│   └── SteeringWheel              ← Cylinder + VRSteeringWheelGrab.cs
└── XR Origin (Action-based)       ← Create via GameObject → XR → XR Origin
    └── [VRCameraRig.cs attached]
```

---

## Car Setup (Step-by-Step)

### 1. Ground
- **GameObject → 3D Object → Plane**  
  Name: `Ground`, Scale: `(10, 1, 10)`, Position: `(0, 0, 0)`

### 2. Car Body
- **GameObject → 3D Object → Cube**  
  Name: `Car`, Position: `(0, 1, 0)`, Scale: `(2, 0.8, 4)`
- **Add Component → Rigidbody**  
  Mass: 1500 | Drag: 0.05 | Angular Drag: 0.5 | Use Gravity: ✅ | Is Kinematic: ❌  
  Interpolate: Interpolate | Collision Detection: Continuous
- **Add Component → VRCarController** (from Assets/Scripts)

### 3. Wheels (repeat 4 times)
- Inside `Car`, **Create Empty**, name: `FrontLeftWheel`  
  Positions: FL `(-0.8, 0.5, 1.5)` | FR `(0.8, 0.5, 1.5)` | RL `(-0.8, 0.5, -1.5)` | RR `(0.8, 0.5, -1.5)`
- **Add Component → WheelCollider** with settings:
  ```
  Mass: 20 | Radius: 0.3 | Suspension Distance: 0.2
  Spring: 35000 | Damper: 4500 | Target Position: 0.5
  ```
- Add child **Cylinder** named `WheelMesh`, Scale: `(0.6, 0.1, 0.6)`, Rotation: `(0, 0, 90)`

### 4. Driver Seat
- Inside `Car`, **Create Empty** named `DriverSeat`, Position: `(0, 0.5, 0)`

### 5. Steering Wheel (Optional)
- Inside `Car`, **3D Object → Cylinder** named `SteeringWheel`  
  Position: `(0, 0.8, 0.5)` | Rotation: `(90, 0, 0)` | Scale: `(0.4, 0.05, 0.4)`
- **Add Component → XR Grab Interactable**
- **Add Component → VRSteeringWheelGrab**

### 6. Assign Wheels in VRCarController Inspector
Select the `Car` GameObject and in the **VRCarController** component:
- Drag each `WheelCollider` to the matching slot
- Drag each `WheelMesh` cylinder to the mesh slot
- Drag `SteeringWheel` to the steering wheel slot

### 7. VR Camera Rig
- Delete the default `Main Camera`
- **GameObject → XR → XR Origin (Action-based)**
- Select **XR Origin**, **Add Component → VRCameraRig**
- Drag `DriverSeat` into the **Driver Seat Position** field

---

## Controls

| Input | VR | Keyboard (Editor) |
|---|---|---|
| Accelerate | Right Trigger | W / Up Arrow |
| Brake | Right Grip | S / Down Arrow |
| Steer | Left Joystick | A-D / Left-Right |

---

## Testing in the Editor (No Headset Required)

1. Press **Play**
2. Use **W/A/S/D** or arrow keys to drive
3. For VR simulation: **Window → XR → XR Device Simulator**

---

## Build for Meta Quest (Android)

1. **File → Build Settings → Android → Switch Platform**
2. Player Settings:
   - Min API: Android 10.0 (API 29)
   - XR Plug-in Management → Android → ✅ Oculus
3. Connect Quest via USB, enable Developer Mode on headset
4. **Build and Run**

## Build for PCVR (SteamVR / OpenXR)

1. **File → Build Settings → Windows → Switch Platform**
2. Player Settings → XR Plug-in Management → PC → ✅ OpenXR
3. Add Oculus Touch / OpenXR interaction profiles
4. **Build and Run**

---

## Troubleshooting

| Problem | Fix |
|---|---|
| Car doesn't move | Check WheelColliders are assigned; Rigidbody is not Kinematic |
| VR not detected | Check XR Plugin Management is enabled for your platform |
| Car flips | Lower Rigidbody center of mass; increase spring strength |
| Steering lag | Reduce steeringAngle value in Inspector |
| Package errors | Window → Package Manager → refresh; re-import Starter Assets |
