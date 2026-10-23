# MetroSafe VR — Professional Unity Case Study

A Unity C# case-study starter for **CS4104 Computer Graphics & Virtual Reality**:
metro-station emergency evacuation, autonomous pedestrian agents, exit routing,
dynamic exit blocking, basic metrics, and a procedural architectural scene.

## Important scope
This package creates a substantially more detailed *stylized real-time simulation* using procedural Unity geometry and materials. It does not include licensed photorealistic character assets, certified evacuation modelling, real haptic integration, or a validated safety-engineering tool. Results must be measured on your own machine.

## Requirements
- Unity 6 (6000.5.x) is the editor shown in the user's screenshot. The code uses built-in Unity APIs and should also be adaptable to Unity 2022.3 LTS.
- Windows/macOS/Linux editor.
- No external packages required for the desktop simulation.

## Install into your existing project
1. Back up your current project or duplicate it first.
2. Close Unity, then extract this ZIP.
3. Copy the `Assets/Editor/MetroSafeSceneBuilder.cs` file into your project's `Assets/Editor/` folder.
4. Copy every file in `Assets/Scripts/` into your project's `Assets/Scripts/` folder.
5. Reopen Unity and wait until script compilation finishes.
6. In the top menu, choose **CS4104 → Build MetroSafe Professional Scene**.
7. Confirm the rebuild prompt. The builder creates and saves `Assets/Scenes/MetroSafe_Professional.unity`.
8. Open that scene and press Play.

Do not copy this ZIP's whole `Assets` folder over your current Assets folder blindly; merge/copy the included files so your existing files remain safe.

## Controls
- `B`: toggle the central incident barrier / hazard state.
- `Space`: pause/resume simulation.
- `R`: reset pedestrian starting positions.
- Mouse drag: orbit camera.
- Mouse wheel: zoom camera.

## Features
- Architectural station shell, ceiling ribs and luminous strips
- Tiled floor seams, directional evacuation markings and exit portals/signage
- Stylized pedestrian models with torso, head, arms and legs
- Autonomous nearest-open-exit steering, local separation and obstacle avoidance
- Dynamic blocked exit demonstration
- On-screen metrics: elapsed time, evacuated/remaining, percentage, exit status and approximate FPS
- CSV export button for experiment logging
- Experiment plan, theory notes, test plan and viva prompts in `Documentation/`

## Before submission
- Run at least 3 trials per scenario and record measured results.
- Take screenshots of the scene, incident toggle, dashboard and Unity Profiler.
- Add XR Interaction Toolkit/OpenXR only if a physical headset is required. This starter is a desktop simulation with VR-oriented case-study design, not a configured headset build.
- Review all scripts and be prepared to explain the algorithms.
