
using UnityEngine;
using System.Collections.Generic;

public class MetroSafeManager : MonoBehaviour
{
    public List<MetroSafeAgent> agents = new List<MetroSafeAgent>();
    public List<MetroSafeExit> exits = new List<MetroSafeExit>();

    public bool isPaused;
    public bool incidentActive = true;
    public float elapsedTime;
    public Transform incidentBarrier;
    public Renderer incidentRenderer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            isPaused = !isPaused;

        if (Input.GetKeyDown(KeyCode.B))
            ToggleIncident();

        if (Input.GetKeyDown(KeyCode.R))
            ResetSimulation();

        if (!isPaused)
            elapsedTime += Time.deltaTime;
    }

    public MetroSafeExit FindNearestOpenExit(Vector3 position)
    {
        MetroSafeExit best = null;
        float bestDistance = float.MaxValue;

        foreach (var exit in exits)
        {
            if (exit == null || !exit.isOpen)
                continue;

            float distance =
                (exit.Position - position).sqrMagnitude;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = exit;
            }
        }

        return best;
    }

    public int EvacuatedCount
    {
        get
        {
            int count = 0;
            foreach (var agent in agents)
                if (agent != null && agent.evacuated)
                    count++;

            return count;
        }
    }

    public int RemainingCount =>
        Mathf.Max(0, agents.Count - EvacuatedCount);

    public void ToggleIncident()
    {
        incidentActive = !incidentActive;

        if (incidentBarrier != null)
            incidentBarrier.gameObject.SetActive(incidentActive);

        if (incidentRenderer != null)
            incidentRenderer.material.color = incidentActive
                ? new Color(1f, 0.32f, 0.035f)
                : new Color(0.05f, 0.8f, 0.3f);

        foreach (var exit in exits)
        {
            if (exit != null &&
                exit.exitLabel.ToUpper().Contains("NORTH"))
            {
                exit.SetOpen(!incidentActive);
            }
        }
    }

    public void ResetSimulation()
    {
        elapsedTime = 0f;

        foreach (var agent in agents)
            if (agent != null)
                agent.ResetAgent();
    }
}
