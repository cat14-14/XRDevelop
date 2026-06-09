# Flight Startup Trim

This file records the startup-value changes made to stop the aircraft from drifting right at scene start during keyboard-only validation.

## Applied Changes

### Code Defaults

1. `Assets/Script/Flight/AircraftCore.cs`
   `initialAirspeedMps`: `90f` -> `0f`
   `initialThrottle01`: `0.35f` -> `0f`

2. `Assets/Script/Flight/KeyboardFlightOverrideInputSource.cs`
   `initialThrottle01`: `0.35f` -> `0f`

3. `Assets/Script/Flight/SimpleJetEngineModel.cs`
   `idleThrustNewtons`: `15000f` -> `0f`

### Scene Instance Values

1. `Assets/Scenes/ACS.unity`
   `KeyboardFlightOverrideInputSource.initialThrottle01`: `0.35` -> `0`

2. `Assets/Scenes/ACS.unity`
   `SimpleJetEngineModel.idleThrustNewtons`: `15000` -> `0`

3. `Assets/Scenes/ACS.unity`
   `AircraftCore.initialAirspeedMps`: `90` -> `0`
   `AircraftCore.initialThrottle01`: `0.35` -> `0`

## Why

- `AircraftCore.initialAirspeedMps = 90` gave the aircraft immediate forward velocity on play.
- `AircraftCore.initialThrottle01 = 0.35` and `KeyboardFlightOverrideInputSource.initialThrottle01 = 0.35` started the command pipeline with throttle already applied.
- `SimpleJetEngineModel.idleThrustNewtons = 15000` produced thrust even with low throttle.

For keyboard-only structure validation, the intended startup behavior is:

- no initial airspeed
- no initial throttle
- no idle thrust

## Note

If the aircraft still slides sideways after these changes, the remaining cause is likely transform orientation:
`Aircraft_F16_Root` must have its local forward axis on `+Z`, and mesh-only visual correction should happen on the child model object instead of the root.

## Follow-up Ground Fixes

### Ground Behavior Defaults

4. `Assets/Script/Flight/SimpleGroundHandlingModel.cs`
   Added ground-state processing so that:
   `pitch` is blocked while grounded below takeoff speed
   `roll` is blocked while grounded
   `roll` can be reused as taxi steering input on the ground
   the aircraft auto-levels to the runway while grounded at low speed
   the contact height is measured from child renderers so the model does not sink into the ground plane

5. `Assets/Script/Flight/AircraftCore.cs`
   Grounded state is refreshed earlier in the fixed step and ground command processing now runs before angular-rate updates.

### Scene Hierarchy Alignment

4. `Assets/Scenes/ACS.unity`
   Added `Aircraft_VisualRig` under `Aircraft_F16_Root`
   Reparented `F-16_Model` and `Player` under `Aircraft_VisualRig`
   Rotated `Aircraft_VisualRig` to `Y = 90`

This keeps the simulation root on the intended body frame while aligning the visible aircraft and cockpit rig with forward flight.

## Latest Input / Mode Split

### Ground vs Air Control Split

6. `Assets/Script/Flight/SimpleGroundHandlingModel.cs`
   `command.roll` is always suppressed while grounded and reused as taxi steering.
   `command.pitch` is now blocked on the ground by default and only allows positive takeoff rotation above `pitchUnlockSpeedMps`.
   Added `takeoffRotationAuthority = 0.55f` so high-speed runway rotation is softer than full in-air pitch authority.

7. `Assets/Scenes/ACS.unity`
   `SimpleGroundHandlingModel.takeoffRotationAuthority`: added as `0.55`

### Keyboard Throttle / XR Simulator Toggle

8. `Assets/Script/Flight/KeyboardFlightOverrideInputSource.cs`
   Added automatic lookup/toggling of the scene object named `XR Interaction Simulator` when keyboard override is ON.
   Added alternate throttle keys: `UpArrow` for throttle up, `DownArrow` for throttle down.
   `throttleChangePerSecond`: `0.35f` -> `0.6f`

9. `Assets/Scenes/ACS.unity`
   `KeyboardFlightOverrideInputSource.autoToggleXRInteractionSimulator`: `true`
   `KeyboardFlightOverrideInputSource.xrInteractionSimulatorObjectName`: `XR Interaction Simulator`
   `KeyboardFlightOverrideInputSource.throttleChangePerSecond`: `0.35` -> `0.6`

### HUD Guidance

10. `Assets/Script/Flight/AircraftDebugHUD.cs`
    Added on-screen control hints for:
    throttle keys
    ground steering mode
    air control mode

## 2026-04-11 Change Journal

### Realistic F-16 Takeoff Speed Split

11. `Assets/Script/Flight/SimpleGroundHandlingModel.cs`
    Added `takeoffReleaseMinSpeedMps` so runway rotation speed and actual liftoff release speed are no longer forced to be the same value.
    Default `takeoffIntentSpeedMps`: `55f` -> `75f`
    Default `takeoffReleaseMinSpeedMps`: added as `85f`
    `GetTakeoffReleaseSpeedMps()` now uses the dedicated release speed floor instead of the lower of the rotation values.

12. `Assets/Scenes/ACS.unity`
    `SimpleGroundHandlingModel.takeoffIntentSpeedMps`: `24` -> `75`
    `SimpleGroundHandlingModel.takeoffReleaseMinSpeedMps`: added as `85`
    `SimpleGroundHandlingModel.pitchUnlockSpeedMps`: `24` -> `75`

### Why

- The old scene values allowed rotation and liftoff in the `24 m/s` range, which is far below representative F-16 takeoff speeds.
- A more realistic split is:
  rotation input starts around `75 m/s`
  ground release is held until about `85 m/s`
- This keeps the aircraft from lifting off unrealistically early while still allowing nose-up rotation slightly before wheel-off.

### Journal Rule

- Continue appending new change entries to this file instead of overwriting older notes.
