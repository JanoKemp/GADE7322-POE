using UnityEngine;
using System.Collections;

public class cubeHighLight : MonoBehaviour
{
    private static cubeHighLight selectedCube; //keeps track of the currently selected cube
    private Renderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public float flashSpeed = 2f;  //speed of the color flash

    private bool isFlashing = false;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    void OnMouseDown()
    {
        // stop flashing and reset the previous cube if another one is selected
        if (selectedCube != null && selectedCube != this)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
        }

        // Set the currently selected cube
        selectedCube = this;
        StartCoroutine(FlashColor());
    }
    //Couroutine for smooth flash from highlighted colour to original
    IEnumerator FlashColor()
    {
        isFlashing = true;
        float t = 0;

        while (isFlashing)
        {
            t += Time.deltaTime * flashSpeed;
            cubeRenderer.material.color = Color.Lerp(originalColor, highlightColor, Mathf.PingPong(t, 1));
            yield return null;
        }
    }

    public void StopFlash()
    {
        isFlashing = false;
        StopAllCoroutines();
    }

    public void RevertColor()
    {
        cubeRenderer.material.color = originalColor;
    }
}
