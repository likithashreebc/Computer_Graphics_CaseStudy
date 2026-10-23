
using UnityEngine;

public class MetroSafeExit : MonoBehaviour
{
    public string exitLabel = "EXIT";
    public bool isOpen = true;
    public Renderer statusRenderer;

    public Vector3 Position => transform.position;

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (statusRenderer != null)
        {
            statusRenderer.material.color = open
                ? new Color(0.08f, 1f, 0.25f)
                : new Color(1f, 0.12f, 0.04f);
        }
    }
}
