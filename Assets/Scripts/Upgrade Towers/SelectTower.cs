using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Runtime.InteropServices;

public class SelectedTower : MonoBehaviour
{
    private static SelectedTower selectedCube;
    private Renderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public float flashSpeed = 2f;

    private bool isFlashing = false;

    public GameObject upgradeUI;//set in inspector

   

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
        
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedCube != null)
            {
                
                DeselectCube();
               
            }
        }
        
       
    }

    void OnMouseDown()
    {
        
        // If a different cube is already selected, stop its flash and reset color
        if (selectedCube != null && selectedCube != this)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
            
        }

        // Select the current cube
        selectedCube = this;
        StartCoroutine(FlashColor());
        upgradeUI.SetActive(true);

    }

    public void DeselectCube()
    {
        if (selectedCube != null)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
          
            selectedCube = null;
        }
    }

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
        upgradeUI.SetActive(false);
        isFlashing = false;
        StopAllCoroutines();
    }

    public void RevertColor()
    {
        cubeRenderer.material.color = originalColor;
    }

}
