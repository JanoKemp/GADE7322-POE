using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;

    public void Start()
    {
        mainCamera = Camera.main;
    }
    public void UpdateHealth(float currentVal, float maxVal)
    {
        slider.value = currentVal/maxVal;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;

    }
}
