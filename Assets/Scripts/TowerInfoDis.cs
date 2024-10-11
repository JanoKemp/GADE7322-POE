using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerInfoDis : MonoBehaviour
{
    // Reference to the TextMeshPro component
    public TextMeshProUGUI towerInfoText;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize text as empty
        towerInfoText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Detect key presses and display corresponding tower info
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DisplayTowerInfo("Base Tower: 50 Gold");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DisplayTowerInfo("Ripple Tower: 75 Gold");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DisplayTowerInfo("Mortar Tower: 100 Gold");
        }
    }

    // Method to display tower info text
    private void DisplayTowerInfo(string info)
    {
        towerInfoText.text = info;
    }
}
