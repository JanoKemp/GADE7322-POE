using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Runtime.InteropServices;

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
    public GameObject[] Towers;
    private PlayerRes playerGold;

    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    public bool forMap;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer.material.HasProperty(BaseColorID))
        {
            originalColor = cubeRenderer.material.GetColor(BaseColorID);
        }
        else
        {
            Debug.LogWarning("Material does not have a '_BaseColor' property.");
            originalColor = Color.white; // Fallback to white if not found
        }
        //originalColor = cubeRenderer.material.color;
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
        if (forMap)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Debug.Log("1 is pressed");
                objectToSpawn = Towers[0];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                objectToSpawn = Towers[1];
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                objectToSpawn = Towers[2];
            }
        }
        else return;
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
            Color currentFlashColor = Color.Lerp(Color.red, highlightColor, Mathf.PingPong(t, 1));

            if (cubeRenderer.material.HasProperty(BaseColorID))
            {
                cubeRenderer.material.SetColor(BaseColorID, currentFlashColor);
            }
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
        if (cubeRenderer.material.HasProperty(BaseColorID))
        {
            cubeRenderer.material.SetColor(BaseColorID, originalColor);
        }
    }

    public void ShowUI()
    {
        if (currentUI == null)
        {
            currentUI = Instantiate(uiPrefab);
        }

        currentUI.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        if (forMap)
        {
            Button spawnButton = currentUI.GetComponentInChildren<Button>();
            spawnButton.onClick.RemoveAllListeners();
            spawnButton.onClick.AddListener(SpawnObjectOnCube);
        }

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
        int towerIndex = System.Array.IndexOf(Towers, objectToSpawn);
        if (towerIndex == 0 && gold >= 50)
        {
            playerGold.MinusGold(50);
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.8f, 0);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            DeselectCube();
        }
        if(towerIndex == 1 && gold >= 75)
        {
            playerGold.MinusGold(75);
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.48f, 0);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            DeselectCube();
        }
        if (towerIndex == 2 && gold >= 100)
        {
            playerGold.MinusGold(100);
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.48f, 0);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            DeselectCube();
        }


    }

}
