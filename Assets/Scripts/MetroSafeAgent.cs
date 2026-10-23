using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MetroSafeAgent : MonoBehaviour
{
    public MetroSafeManager manager;
    public int agentId;
    public float walkSpeed = 1.4f;
    public bool evacuated;
    public float personalSpace = .72f;
    CharacterController controller;
    Vector3 startPosition;
    Renderer[] renderers;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position;
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (manager == null || manager.isPaused || evacuated) return;
        MetroSafeExit exit = manager.FindNearestOpenExit(transform.position);
        if (exit == null) return;

        Vector3 toExit = exit.Position - transform.position;
        toExit.y = 0;
        if (toExit.magnitude < 1.25f)
        {
            evacuated = true;
            foreach (var r in renderers) if (r != null) r.enabled = false;
            controller.enabled = false;
            return;
        }

        Vector3 desired = toExit.normalized;
        // Local separation: steer away from nearby agents to reduce overlap.
        Vector3 separation = Vector3.zero;
        foreach (var other in manager.agents)
        {
            if (other == null || other == this || other.evacuated) continue;
            Vector3 away = transform.position - other.transform.position;
            away.y = 0;
            float d = away.magnitude;
            if (d > .001f && d < personalSpace)
                separation += away.normalized * ((personalSpace - d) / personalSpace);
        }
        if (separation.sqrMagnitude > .001f)
            desired = (desired + separation.normalized * 1.25f).normalized;

        // Simple central-obstacle steering for the incident barrier.
        if (manager.incidentActive && manager.incidentBarrier != null)
        {
            Vector3 away = transform.position - manager.incidentBarrier.position;
            away.y = 0;
            if (away.magnitude < 2.3f && away.sqrMagnitude > .001f)
                desired = (desired + away.normalized * 1.5f).normalized;
        }

        if (desired.sqrMagnitude > .001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desired), 7f * Time.deltaTime);
            controller.Move(desired * walkSpeed * Time.deltaTime + Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    public void ResetAgent()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        evacuated = false;
        transform.position = startPosition;
        if (controller != null) controller.enabled = true;
        if (renderers == null) renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) if (r != null) r.enabled = true;
    }
}
