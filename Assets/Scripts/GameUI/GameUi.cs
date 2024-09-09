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
    private bool updateGameOverUI = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        playerRes = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        mainTower = GameObject.FindGameObjectWithTag("PlayerTower").GetComponent<MainTower>();
        gameOver = mainTower.gameOver;
        if(gameOver)
        {
            updateGameOverUI = true;
        }
        if(gameOver && updateGameOverUI)
        {

            finalUpdate();
            updateGameOverUI=false;
        }
        
        
        
    }

    private void finalUpdate()
    {
        Time.timeScale = 0.2f;
        lossTitle.text = "Game Over - You Lost";
        enemiesKilled.text = "Enemies Killed: " + playerRes.enemiesKilled.ToString();
        timeSurvived.text = "Time Survived: " + playerRes.TimeSurvived.ToString();

    }

    public void OnClickExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
