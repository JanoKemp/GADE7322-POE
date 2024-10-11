using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUi : MonoBehaviour
{
    public TextMeshProUGUI lossTitle;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI timeSurvived;
    public TextMeshProUGUI towerInfo;

    private PlayerRes playerRes;
    private MainTower mainTower;
    private bool gameOver;
    public GameObject UI;
    private int updateGameOverUI = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        playerRes = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>();
        updateGameOverUI = 0;
        towerInfo.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        mainTower = GameObject.FindGameObjectWithTag("PlayerTower").GetComponent<MainTower>();
        gameOver = mainTower.gameOver;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Key 1 Pressed");
            towerInfo.text = "Base Tower: 50 Gold"; // Display for key 1
        }
       
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            towerInfo.text = "Ripple Tower: 75 Gold"; // Display for key 2
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            towerInfo.text = "Mortar Tower: 100 Gold"; // Display for key 3
        }

        if (gameOver)
        {
            
        }
        if(gameOver && updateGameOverUI < 1)
        {
            updateGameOverUI += 1;
            finalUpdate();
            
        }
        
        
        
    }

    private void finalUpdate()
    {
        Time.timeScale = 0.2f;
        lossTitle.text = "Game Over - You Lost";
        //enemiesKilled.text = "Enemies Killed: " + playerRes.enemiesKilled.ToString();
        timeSurvived.text = "Time Survived: " + playerRes.TimeSurvived.ToString();

    }

    public void OnClickExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
