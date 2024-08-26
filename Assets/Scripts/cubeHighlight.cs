using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeHighlight : MonoBehaviour
{
    private Renderer cubeRender;
    private Color originalColour;
    public Color highLightColour = Color.yellow;
    void Start()
    {
        cubeRender = GetComponent<Renderer>();
        originalColour = cubeRender.material.color;
    }

    private void OnMouseDown()
    {
        cubeRender.material.color = highLightColour;
    }
    private void OnMouseUp() {
        cubeRender.material.color = originalColour;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
