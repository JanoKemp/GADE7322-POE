using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRes : MonoBehaviour
{
    public int gold = 0;// In enemy script its add 5 gold per kill
    private float goldPerSecond = 1f;
    public GameObject gamePlayUI;
    public TextMeshProUGUI goldText;

    public int enemiesKilled = 0;
    public int bulletsFired = 0;
    public float TimeSurvived = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        goldText.text = DisplayGold();
        goldPerSecond -= Time.deltaTime;
        if(goldPerSecond <= 0 )
        {
            gold++;
            goldPerSecond = 1f;
        }
        TimeSurvived = Time.realtimeSinceStartup;
    }

    public string DisplayGold()
    {
        string displayGold = $"Gold: {gold}";
        return displayGold;
    }

}
