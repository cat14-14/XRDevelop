# F-16 Public Takeoff Procedure Notes

Last updated: 2026-04-12

## Purpose

This file summarizes what can be stated from public F-16 operating sources about takeoff and immediate departure procedures.

Important limit:

- This is not a full Dash-1 checklist.
- It does not attempt to reproduce restricted cockpit switchology or non-public tactics.
- It is a public-source operational outline suitable for simulation design, mission scripting, and general understanding.

## Short Answer

Yes. Publicly available F-16 documentation does expose a meaningful amount of takeoff procedure information.

What is publicly visible is mostly:

- mission and takeoff data requirements
- taxi and arming rules
- end-of-runway inspection flow
- lineup spacing
- takeoff interval rules
- when afterburner takeoff is required
- abort rules
- immediate post-takeoff climb and join-up guidance

What is not fully public in one clean official place is the complete cockpit-by-cockpit switch sequence from engine start through rotation in the level of detail found in a full flight manual checklist.

## Public Procedure Flow

## 1. Mission planning and takeoff data

Before taxi, the pilot is required to carry mission and performance data.

The public AFMAN 11-2F-16 Volume 3 states the Mission Data Card must include minimum takeoff and landing data such as:

- 2,000-foot acceleration check speed when required
- refusal speed
- rotation speed
- takeoff speed
- takeoff distance
- landing speed and landing distance
- heavyweight landing data

This matters because the F-16 takeoff is not treated as a generic "full power and go" event. The takeoff is performance-calculated in advance.

## 2. Taxi and arming

Public USAF guidance gives concrete ground movement rules:

- minimum taxi interval: `150 ft staggered` or `300 ft trail`
- taxi speed limit: `30 knots`
- over a raised cable: `15 knots`
- in turns: `10 knots`

In icing-sensitive conditions, the public guidance also says:

- anti-ice goes on before engine start when conditions warrant
- an ice/FOD monitor watches the inlet when stopped for longer periods
- the pilot holds in the arming spot with the monitor present until cleared for takeoff

This shows the F-16 departure flow is tightly linked to engine protection and FOD discipline, not just pilot actions.

## 3. EOR inspection and before-takeoff checks

At the end of the runway, public guidance requires an EOR inspection immediately before takeoff.

The public AFMAN describes:

- pilot keeps hands visible while quick check and arming/de-arming are in progress
- if no intercom is used, pilot maintains visual contact with the crew chief and uses hand signals
- the EOR inspection is normally done near the runway end or when departing the chock area
- flight members inspect each other for correct configuration and abnormalities

So in practical terms, the public sequence is:

1. taxi to arming / EOR area
2. ground crew quick-check and arm-safe verification
3. visual or intercom coordination
4. final aircraft configuration cross-check
5. taxi to lineup

## 4. Lineup

Public AFMAN guidance gives basic lineup spacing:

- minimum spacing between separated elements or flights: `500 ft`
- wingmen maintain wingtip clearance with lead
- if runway width allows, all aircraft line up with wingtip clearance between aircraft

This is enough to model realistic formation takeoff spacing without needing restricted data.

## 5. Takeoff decision rules

The public manual gives several concrete go / no-go rules:

- do not take off when runway condition reading `RCR` is below `10`
- on training missions, do not take off if computed takeoff roll exceeds `80%` of available runway
- ensure a compatible departure-end cable is raised
- intersection takeoffs require approval if operationally necessary
- use afterburner takeoff whenever computed MIL-power takeoff roll exceeds `50%` of available runway

That means the public F-16 takeoff decision is performance- and runway-driven, not purely pilot preference.

## 6. Start of takeoff roll

Public guidance also exposes a few details about the roll itself:

- comply with minimum takeoff interval between aircraft / elements of `10 seconds`
- use `15 seconds` for afterburner takeoff
- increase to `20 seconds` for join-up on top or when carrying live air-to-surface ordnance, with listed exceptions
- after brake release, steer toward the runway centerline

This is one of the clearest openly published pieces of F-16 takeoff procedure.

## 7. Abort rules

The public AFMAN gives several useful abort rules:

- if aborting during takeoff roll, the pilot calls call sign and intentions when practical
- following aircraft adjust or abort to maintain safe clearance
- if a departure-end cable arrestment is intended, the phrase is `Cable, Cable, Cable`
- if a barrier/net arrestment is intended, the phrase is `Barrier, Barrier, Barrier`
- if aborting at or above `100 KCAS`, lower the hook
- if aborting below `100 KCAS`, lower the hook if there is doubt about stopping on the remaining runway
- if aborting above `120 KCAS`, or whenever hot brakes are suspected, declare a ground emergency and taxi to the hot-brake area

This is valuable because public sources do not just show "how to go," but also "how to stop safely."

## 8. Initial climb and join-up

Public post-takeoff guidance in AFMAN 11-2F-16 Volume 3 states:

- maintain `350 KCAS` while climbing
- or `300 KCAS` at cruise
- until join-up is accomplished
- unless mission requirements brief a different airspeed

This is not the entire departure procedure, but it gives a realistic immediate post-liftoff reference for formation behavior.

## Practical Public Takeoff Flow

If you compress the public-source procedure into a realistic high-level flow, it looks like this:

1. Review mission data and TOLD.
2. Confirm runway, weather, abort factors, and cable status.
3. Taxi with F-16 spacing and speed limits.
4. Enter arming / EOR area for final inspection and configuration check.
5. Line up with proper spacing.
6. Use MIL or afterburner based on runway-performance calculation.
7. Release brakes, hold centerline, and respect formation interval timing.
8. If a problem occurs on the roll, execute public abort logic and hook/cable rules.
9. After liftoff, climb and rejoin using the briefed profile, with `350 KCAS` as the public default reference.

## What You Can Safely Use In This Repo

For simulation or mission logic, the following public items are safe and useful anchors:

- taxi spacing and speed
- EOR inspection stage
- lineup spacing
- runway-condition go / no-go threshold
- runway-usage percentage rules
- MIL vs afterburner decision rule
- takeoff interval timing
- abort callout and hook thresholds
- initial climb / join-up speed reference

## What This File Does Not Claim

This file does not claim to provide:

- full cockpit switch sequence
- exact rotation / liftoff technique by every block and gross weight
- exact flap / trim / control-check sequence from a restricted flight manual
- combat or contingency departure tactics
- customer-specific or nation-specific local supplements

Those details may exist in non-public manuals, training systems, or local operating instructions, but they are outside the scope of this public summary.

## Sources

1. AFMAN 11-2F-16 Volume 3, publicly accessible USAF e-publishing PDF  
   https://static.e-publishing.af.mil/production/1/af_a3/publication/afman11-2f-16v3/afman11-2f-16v3.pdf

2. AFMAN 11-2F-16 Volume 2, publicly accessible USAF e-publishing PDF  
   https://static.e-publishing.af.mil/production/1/af_a3/publication/afman11-2f-16v2/afman11-2f-16v2.pdf

## Key Public Data Pulled From Those Sources

- mission data card requires TOLD items including refusal, rotation, and takeoff speeds
- taxi interval / speed rules
- EOR inspection and before-takeoff checks
- lineup spacing rules
- runway condition and performance limits for takeoff
- afterburner requirement based on runway percentage
- takeoff interval timing
- abort call and hook logic
- initial post-takeoff climb / join-up airspeed guidance

## Repo Note

This file is a companion to:

- `Docs/Flight/F-16.md`

Use `F-16.md` for broad aircraft background and this file for public takeoff / departure procedure notes.
