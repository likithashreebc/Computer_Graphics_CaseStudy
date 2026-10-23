using UnityEngine;
using System.IO;
using System.Text;

public class MetroSafeDashboard : MonoBehaviour
{
    public MetroSafeManager manager;
    GUIStyle titleStyle, bodyStyle, buttonStyle;
    Rect panel = new Rect(18, 18, 340, 250);

    void OnGUI()
    {
        if (manager == null) return;
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 19, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            bodyStyle = new GUIStyle(GUI.skin.label) { fontSize = 13, normal = { textColor = new Color(.85f,.92f,1f) } };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 12, fontStyle = FontStyle.Bold };
        }

        panel.width = Mathf.Min(355, Screen.width - 36);
        GUI.Box(panel, GUIContent.none);
        GUILayout.BeginArea(new Rect(panel.x + 14, panel.y + 10, panel.width - 28, panel.height - 20));
        GUILayout.Label("METROSAFE  /  EVACUATION CONTROL", titleStyle);
        GUILayout.Label("CS4104 • Crowd Dynamics Research Prototype", bodyStyle);
        GUILayout.Space(7);
        GUILayout.Label("ELAPSED TIME     " + manager.elapsedTime.ToString("0.0") + " s", bodyStyle);
        GUILayout.Label("EVACUATED        " + manager.EvacuatedCount, bodyStyle);
        GUILayout.Label("REMAINING        " + manager.RemainingCount, bodyStyle);
        float pct = manager.agents.Count == 0 ? 0 : 100f * manager.EvacuatedCount / manager.agents.Count;
        GUILayout.Label("COMPLETION       " + pct.ToString("0.0") + "%", bodyStyle);
        GUILayout.Label("EXIT STATUS       " + ExitSummary(), bodyStyle);
        GUILayout.Label("FRAME RATE        " + (1f / Mathf.Max(Time.unscaledDeltaTime, .0001f)).ToString("0") + " FPS (approx.)", bodyStyle);
        GUILayout.Space(7);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(manager.isPaused ? "RESUME [SPACE]" : "PAUSE [SPACE]", buttonStyle)) manager.isPaused = !manager.isPaused;
        if (GUILayout.Button("TOGGLE INCIDENT [B]", buttonStyle)) manager.ToggleIncident();
        if (GUILayout.Button("RESET [R]", buttonStyle)) manager.ResetSimulation();
        GUILayout.EndHorizontal();
        if (GUILayout.Button("EXPORT CSV RESULTS", buttonStyle)) ExportCsv();
        GUILayout.EndArea();
    }

    string ExitSummary()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var e in manager.exits) if (e != null) { if (sb.Length > 0) sb.Append(" | "); sb.Append(e.exitLabel.Replace(" EXIT","")).Append(e.isOpen ? ":OPEN" : ":BLOCKED"); }
        return sb.ToString();
    }

    void ExportCsv()
    {
        string path = Path.Combine(Application.persistentDataPath, "MetroSafe_trial.csv");
        float pct = manager.agents.Count == 0 ? 0 : 100f * manager.EvacuatedCount / manager.agents.Count;
        string csv = "elapsed_seconds,total_agents,evacuated,remaining,completion_percent,incident_active\n" +
            manager.elapsedTime.ToString("F2") + "," + manager.agents.Count + "," + manager.EvacuatedCount + "," +
            manager.RemainingCount + "," + pct.ToString("F2") + "," + manager.incidentActive + "\n";
        File.WriteAllText(path, csv);
        Debug.Log("MetroSafe CSV exported to: " + path);
    }
}
