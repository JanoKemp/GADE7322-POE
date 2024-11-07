using UnityEngine;

public class ScreenFlashEffect : MonoBehaviour
{
    public Material flashMaterial; // Assign ScreenFlashMaterial here in the Inspector
    public float flashDuration = 0.2f; // Adjust duration for how long the flash appears
    private float flashTimer;

    private void Start()
    {
        flashMaterial.SetFloat("_FlashStrength", 0); // Start with no flash effect
    }

    private void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            float strength = flashTimer / flashDuration;
            flashMaterial.SetFloat("_FlashStrength", strength);
        }
        else
        {
            flashMaterial.SetFloat("_FlashStrength", 0);
        }
    }

    public void TriggerFlash()
    {
        flashTimer = flashDuration;
        flashMaterial.SetFloat("_FlashStrength", 1); // Start with full flash intensity
    }
}
