using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cubeHighLight : MonoBehaviour
{
    private static cubeHighLight selectedCube; // Keeps track of the currently selected cube
    private Renderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public float flashSpeed = 2f;  // Speed of the color flash
    public GameObject mainCamera;

    private bool isFlashing = false;

    public GameObject uiPrefab;  // The prefab for the UI with a button
    private GameObject currentUI; // The current UI instance
    public GameObject objectToSpawn; // The object to spawn on top of the selected cube

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (currentUI != null)
        {
            currentUI.transform.LookAt(currentUI.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }

        // Right-click (Mouse Button 1) deselects the currently selected cube
        if (Input.GetMouseButtonDown(1))
        {
            DeselectCube();
        }
    }

    void OnMouseDown()
    {
        // If a different cube is already selected, stop its flash and reset color
        if (selectedCube != null && selectedCube != this)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
            selectedCube.HideUI();
        }

        // Select the current cube
        selectedCube = this;
        StartCoroutine(FlashColor());

        // Show the UI for the current cube
        ShowUI();
    }

    public void DeselectCube()
    {
        // If there is a cube selected, stop flashing and reset its color
        if (selectedCube != null)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
            selectedCube.HideUI();
            selectedCube = null;  // Clear the current selection
        }
    }

    // Coroutine for smooth flashing between highlight color and original color
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

    // Show the UI above the cube
    public void ShowUI()
    {
        if (currentUI == null)
        {
            currentUI = Instantiate(uiPrefab); // Instantiate the UI prefab
        }

        // Position the UI above the cube
        currentUI.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Adjust the offset

        // Add listener to the button to spawn an object
        Button spawnButton = currentUI.GetComponentInChildren<Button>();
        spawnButton.onClick.RemoveAllListeners(); // Clear previous listeners
        spawnButton.onClick.AddListener(SpawnObjectOnCube);

        currentUI.SetActive(true);
    }

    // Hide the UI
    public void HideUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
    }

    // Spawns an object on top of the selected cube
    public void SpawnObjectOnCube()
    {
        if (objectToSpawn != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0); // Spawn on top of the cube
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            DeselectCube();
        }
    }
}
