# F-16 First-Airframe Implementation Plan

## Goal

This project will support multiple airframes. The first reference implementation is the F-16.

The immediate objective is not full-fidelity certification. The objective is to build a public-data-based F-16 baseline that is:

- structurally separated into control law, aerodynamics, engine, and validation
- tunable from data assets instead of hardcoded values
- traceable against a small set of public target metrics

## Public Baseline Targets

The following baseline values are the first-pass targets for the F-16 implementation. These are public, non-classified reference values and should be treated as tuning anchors, not as a complete aerodynamic truth set.

### Official U.S. Air Force Fact Sheet Targets

Source:

- https://www.af.mil/About-Us/Fact-Sheets/Display/%20tabid/224/Article/104505/f-16-fighting-falcon/

Key values pulled from the fact sheet:

- Powerplant thrust: 27,000 lbf for F-16C/D
- Wingspan: 32 ft 8 in / 9.8 m
- Length: 49 ft 5 in / 14.8 m
- Height: 16 ft / 4.8 m
- Empty weight: 19,700 lb / 8,936 kg
- Maximum takeoff weight: 37,500 lb / 16,875 kg
- Internal fuel: 7,000 lb / 3,175 kg
- Max speed: 1,500 mph / Mach 2 at altitude
- Ceiling: above 50,000 ft / 15 km
- Structural limit with full internal fuel: 9 g

### Public NASA / NASA-NTRS Control-Law Cues

Source:

- https://ntrs.nasa.gov/api/citations/19840012524/downloads/19840012524.pdf

Useful design cues from the AFTI/F-16 digital flight-control paper:

- Standard F-16 normal-mode longitudinal control is centered on normal-acceleration command.
- Roll axis is treated as roll-rate command.
- Gear-down / approach handling benefited from more pitch-rate-command-like behavior and improved angle-of-attack stability.
- Advanced air-to-air pitch-stick control benefited from pitch-rate-error-based adaptive gain shaping.
- Envelope expansion discussed low-speed high-angle-of-attack operation and Mach numbers up to 1.2.

### Public NASA High-Angle-of-Attack Cues

Source:

- https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/19800005879.pdf

Useful design cues from the high-AoA study:

- An F-16-based relaxed-stability fighter can resist classical yaw departure but still be vulnerable to pitch departure during rapid large-amplitude rolling at low speed.
- Deep-stall behavior and recovery logic need explicit handling.

## First Tuning Sheet

This is the first working target sheet for the simulation. These values should be used to tune the initial F-16 assets and validator thresholds.

### Geometry / Mass

- Wingspan target: 9.8 m
- Length reference: 14.8 m
- Empty-mass anchor: 8,936 kg
- First simulation combat-mass target:
  - 11,500 to 12,500 kg
  - This range is the preferred first-pass tuning mass because it is more representative than empty weight for flight-model validation.

### Engine

- Sea-level military-power anchor: 27,000 lbf class public reference
- First-pass engine objective:
  - realistic static acceleration feel
  - transonic thrust dip
  - afterburner recovery above transonic region

### Flight Envelope

- High-speed objective: Mach 2 class at altitude
- Ceiling objective: above 50,000 ft
- Normal-load limit target: 9 g
- First-pass AoA limiter target: 25 deg class with soft limit behavior

### Handling / Control-Law Direction

- Pitch:
  - base mode should move toward `Nz command + AoA limiter`
- Roll:
  - base mode should move toward `roll-rate command`
- Yaw:
  - base mode should remain stability-augmentation and departure-prevention driven

### Validator Targets To Track

These are the minimum values to track in the first F-16 pass:

- takeoff roll distance
- liftoff speed
- max climb rate
- maximum instantaneous turn rate
- best sustained turn rate
- peak AoA
- stall onset AoA
- stall recovery time

## Execution Order

### Phase 1: Asset Baseline

1. Create `F16_AeroDatabase.asset`
2. Create `F16_EngineDatabase.asset`
3. Create `F16_ControlLawConfig.asset`
4. Populate all three with first-pass public baseline values

Exit criteria:

- all three assets exist
- the aircraft reads them at runtime
- the sim still builds cleanly

### Phase 2: F-16 Control Law

1. Add an F-16-specific runtime path for pitch law
2. Implement first-pass `Nz command`
3. Add soft AoA limiter behavior
4. Keep roll as roll-rate command
5. Keep yaw as stability/departure logic

Exit criteria:

- generic control law is no longer the only active path
- F-16 config drives pitch/roll/yaw behavior

### Phase 3: Validator-Guided Tuning

1. Run takeoff tests
2. Run climb tests
3. Run turning tests
4. Run stall and recovery tests
5. Adjust aero, engine, and control law against validator metrics

Exit criteria:

- takeoff, climb, turn, and stall metrics are numerically plausible
- protection logic is stable in repeated tests

### Phase 4: Aero Model Upgrade

1. Replace purely formula-based coefficient shaping with data-table interpolation
2. Introduce coefficient tables by AoA / Beta / Mach
3. Add control-surface effectiveness by flight condition

Exit criteria:

- aero model no longer depends only on scalar coefficient formulas
- F-16 data tables become the primary source of force and moment behavior

## Immediate Next Task

The next concrete implementation step is:

1. generate first-pass F-16 assets
2. connect them in the scene
3. then implement F-16 pitch law as `Nz command + AoA limiter`

That is the correct next move from the current codebase state.
