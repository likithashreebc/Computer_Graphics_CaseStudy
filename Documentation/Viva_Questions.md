# Viva Questions and Short Answers

1. **What is the problem being solved?**  
   Visualising evacuation behaviour when station exits are available or blocked.

2. **What is an autonomous agent?**  
   An entity that observes local state and chooses movement according to behaviour rules without hand-authored frame-by-frame paths.

3. **How is the target exit selected?**  
   The nearest exit among exits whose `isOpen` state is true.

4. **What is separation steering?**  
   A local steering term that discourages agents from occupying the same space.

5. **What is the limitation of nearest-exit routing?**  
   It does not account for queue length, route capacity, smoke, group behaviour or global congestion.

6. **What does CharacterController do?**  
   It provides capsule-based movement and collision handling without requiring a Rigidbody for each pedestrian.

7. **Why use emissive materials?**  
   To make exit markers and wayfinding strips visually prominent.

8. **What is the 90 Hz frame budget?**  
   Approximately 11.11 ms per frame.

9. **How can the scene be optimized?**  
   LODs, occlusion/frustum culling, baked lighting, GPU instancing, simplified colliders and spatial partitioning.

10. **Is this a validated evacuation tool?**  
    No. It is an educational prototype; real safety use requires calibrated models and validation.

11. **What data does the CSV export contain?**  
    A snapshot of elapsed time, total agents, evacuated, remaining, completion percentage and incident state.

12. **How would you make the crowd model more realistic?**  
    Use a calibrated social-force or velocity-obstacle model, density-aware path planning, realistic character animation and validated flow parameters.
