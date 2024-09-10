using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class cubeHighLight : MonoBehaviour
{
    private static cubeHighLight selectedCube;
    private Renderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public float flashSpeed = 2f;
    public GameObject mainCamera;

    private bool isFlashing = false;

    public GameObject uiPrefab;
    private GameObject currentUI;
    public GameObject objectToSpawn;

    private PlayerRes playerGold;

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
        // Prevent click action if the mouse is over any UI element
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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

        ShowUI();
    }

    public void DeselectCube()
    {
        if (selectedCube != null)
        {
            selectedCube.StopFlash();
            selectedCube.RevertColor();
            selectedCube.HideUI();
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
        isFlashing = false;
        StopAllCoroutines();
    }

    public void RevertColor()
    {
        cubeRenderer.material.color = originalColor;
    }

    public void ShowUI()
    {
        if (currentUI == null)
        {
            currentUI = Instantiate(uiPrefab);
        }

        currentUI.transform.position = transform.position + new Vector3(0, 1.5f, 0);

        Button spawnButton = currentUI.GetComponentInChildren<Button>();
        spawnButton.onClick.RemoveAllListeners();
        spawnButton.onClick.AddListener(SpawnObjectOnCube);

        currentUI.SetActive(true);
    }

    public void HideUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
    }

    public void SpawnObjectOnCube()
    {
        playerGold = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>();
        int gold = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>().gold;
        if (objectToSpawn != null && gold > 50)
        {
            playerGold.MinusGold(50);
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            DeselectCube();
        }
    }
}
