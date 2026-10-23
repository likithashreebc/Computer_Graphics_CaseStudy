# CS4104 Case Study: MetroSafe VR — Emergency Evacuation and Crowd Dynamics

## 1. Abstract
MetroSafe VR is a real-time, interactive prototype for visualising pedestrian evacuation from a metro station under changing exit availability. The environment uses procedural 3D architecture, material-based visual differentiation, autonomous agents, steering and local separation, exit-state changes, an on-screen dashboard and CSV trial snapshots.

## 2. Problem statement
Transport operators need to examine how exit closures and crowd concentration may affect evacuation flow. Physical drills are expensive and difficult to repeat under controlled conditions. A virtual environment enables repeatable scenarios, visual inspection and controlled comparison. This prototype is educational and must not be used as a certified life-safety model.

## 3. Objectives
- Construct a recognizable 3D metro-station environment in Unity.
- Implement autonomous pedestrians with goal-seeking and local separation.
- Demonstrate a dynamic incident that changes an exit's availability.
- Display measurable progress and export trial snapshots.
- Discuss frame-time, collision, rendering and VR comfort constraints.

## 4. Graphics pipeline and design rationale
The Unity real-time pipeline transforms object-space vertices through model, view and projection transformations, performs clipping and rasterization, then shades fragments and composites the final image. Materials distinguish structural concrete, metal, floor surfaces, wayfinding strips and hazard states. Emissive materials make exit markers visible, but emissive materials alone do not illuminate nearby objects unless supported by suitable real-time/global illumination settings.

The current builder uses procedural primitive meshes and built-in/URP-compatible materials. This is intentionally self-contained; a production visual upgrade would replace primitives with licensed modular station assets, optimized human meshes, physically based material textures and baked lighting.

## 5. Agent behaviour
Each agent selects the nearest available exit:
`e* = arg min_e ||p - e.position||`, considering only exits with `isOpen = true`.

The desired goal direction is:
`d_goal = normalize(e*.position - p)`.

A local separation vector is accumulated from nearby agents:
`S = Σ normalize(p - p_j) × (r - d_j)/r`, for `0 < d_j < r`.

The movement direction combines goal seeking and separation, then a CharacterController moves the agent. This is a simple steering model, not a calibrated social-force model. It can produce unrealistic bottlenecks or local minima and should be evaluated rather than assumed correct.

## 6. Collision and movement
Unity's CharacterController provides capsule-based movement collision against colliders. The station shell and columns use colliders; pedestrian decorative body parts have colliders removed to avoid excess physics cost. Separation is a lightweight local avoidance approximation. A more advanced system could use Unity NavMesh with carving obstacles, reciprocal velocity obstacles, or a spatial grid for neighbour queries.

## 7. Dynamic scenario
Press `B` to toggle the central incident barrier. In this educational demonstration, the north exit is marked unavailable while the incident is active. Agents then re-evaluate their nearest open exit. This models route disruption, not smoke propagation or real-world fire dynamics.

## 8. Performance and VR comfort
A 90 Hz headset refresh implies a frame budget of:
`1000 ms / 90 = 11.11 ms per frame`.
The CPU and GPU work must fit inside the frame budget; stereo rendering may increase GPU workload. Measure with Unity Profiler on target hardware. Use occlusion/frustum culling, LODs, baked lighting, GPU instancing, simplified colliders, pooled agents and spatial partitioning when scaling up. Do not claim a measured frame rate until it has been measured.

## 9. Evaluation protocol
Run at least three trials per scenario, resetting the same initial conditions:
A. All exits open.
B. Incident active / north exit blocked.
C. Higher crowd count (only after validating performance and spawn spacing).

Record evacuation completion time, remaining agents at a fixed time, average and minimum FPS, CPU frame time, GPU frame time and visible bottlenecks. Report mean and standard deviation across trials. Do not fabricate values.

## 10. Limitations and ethics
This is a student prototype with simplified behaviour, no validated human panic model, no smoke/visibility model, no accessibility-specific movement model, no calibrated density-flow relationships, and no physical VR headset configuration. It should not inform real emergency planning without validation by domain experts.

## 11. Conclusion
MetroSafe demonstrates the integration of real-time computer graphics, 3D scene design, autonomous-agent steering, collision-aware movement, interaction and experiment logging in Unity. The most important next step is rigorous testing and measured performance analysis.
