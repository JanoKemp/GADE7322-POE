using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Image damageOverlay; // Assign your Image component here
    public float flashDuration = 0.2f; // Duration of the flash effect
    private float flashTimer;

    private void Start()
    {
        damageOverlay.color = new Color(1, 0, 0, 0); // Set initial color to transparent red
    }

    private void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            float alpha = flashTimer / flashDuration;
            damageOverlay.color = new Color(1, 0, 0, alpha); // Gradually make the flash transparent
        }
        else
        {
            // Ensure the Image is fully invisible when not flashing
            damageOverlay.color = new Color(1, 0, 0, 0);
        }
    }

    public void DamageFlash()
    {
        flashTimer = flashDuration;
        damageOverlay.color = new Color(1, 0, 0, 0.05f); // Set alpha to a visible level (e.g., 0.5)
    }
}
