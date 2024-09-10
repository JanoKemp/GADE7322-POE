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
        
    }

    // Update is called once per frame
    void Update()
    {
        mainTower = GameObject.FindGameObjectWithTag("PlayerTower").GetComponent<MainTower>();
        gameOver = mainTower.gameOver;
        if(gameOver)
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
