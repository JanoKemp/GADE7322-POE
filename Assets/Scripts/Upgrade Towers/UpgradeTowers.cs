using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTowers : MonoBehaviour
{
    
    public GameObject mainCamera;//Set in inspector
    public GameObject thisCanvas;// ^^^^^^^^
    
    private int upgradeMax = 3;
    private GameObject selectedTower;

    private PlayerRes playerInformation;
    private float gold;

    private bool health1 = false;
    private bool health2 = false;
    private bool health3 = false;
    private bool fireRate1 = false;
    private bool fireRate2 = false;
    private bool fireRate3 = false;
        

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerInformation = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>();
    }
    private void Update()
    {
        gold = playerInformation.gold;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        thisCanvas.transform.LookAt(thisCanvas.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    
    #region Generic Tower Upgrade Functions
    public void UpgradeGenericHealth() // Starting Spawning Tower
    { 
        
        GameObject genericTowerSelected = transform.root.gameObject;
        DefenderTower genericTowerAtt = genericTowerSelected.GetComponent<DefenderTower>(); //Allows you to change the attributes of the generic yellow Tower
        genericTowerAtt.upgradeHealthCounter += 1;
        
        //Sets the counters for easier use
        if (genericTowerAtt.upgradeHealthCounter < upgradeMax)
        {
            playerInformation.gold -= 50;
            genericTowerAtt.maxHealth += 50;
            genericTowerAtt.health += 50;
            genericTowerAtt.healthBar.UpdateHealth(genericTowerAtt.health, genericTowerAtt.maxHealth); //Updates the towers UI
        }
        else return;
        
    }
    public void UpgradeGenericFireRate()
    {
        GameObject genericTowerSelected = transform.root.gameObject;
        DefenderTower genericTowerAtt = genericTowerSelected.GetComponent<DefenderTower>(); //Allows you to change the attributes of the generic yellow Tower
        genericTowerAtt.upgradeFireRateCounter += 1;
        if (genericTowerAtt.upgradeFireRateCounter < upgradeMax)
        {
            genericTowerAtt.rpm -= 0.25f;
            playerInformation.gold -= 75;
        }
        else return;
    }
    #endregion
    #region Ripple Tower Upgrades
    public void UpgradeRippleHealth()
    {
        GameObject ripple = transform.root.gameObject;
        RippleDefender rippleS = ripple.GetComponent<RippleDefender>();
        rippleS.upgradeHealthCounter += 1;
        if (rippleS.upgradeHealthCounter < upgradeMax)
        {
            playerInformation.gold -= 75;
            rippleS.maxHealth += 50;
            rippleS.health += 50;
            rippleS.healthBar.UpdateHealth(rippleS.health, rippleS.maxHealth); //Updates the towers UI
        }
        else return;
    }
    public void UpgradeRippleFireRate()
    {
        GameObject rippleTower = transform.root.gameObject;
        RippleDefender rippleAttrib = rippleTower.GetComponent<RippleDefender>(); //Allows you to change the attributes of the generic yellow Tower
        rippleAttrib.upgradeFireRateCounter += 1;
        if (rippleAttrib.upgradeFireRateCounter < upgradeMax)
        {
            playerInformation.gold -= 80;
            rippleAttrib.rpm -= 0.33f;
        }
        else return;
    }
    #endregion
    #region Mortar Defender Upgrade
    public void UpgradeMortarHealth()
    {
        GameObject morterTower = transform.root.gameObject;
        MortarDefender mortarAttrib = morterTower.GetComponent<MortarDefender>(); //Allows you to change the attributes of the generic yellow Tower
        mortarAttrib.upgradeHealthCounter += 1;
        //Sets the counters for easier use
        if (mortarAttrib.upgradeHealthCounter < upgradeMax)
        {
            playerInformation.gold -= 80;
            mortarAttrib.maxHealth += 50;
            mortarAttrib.health += 50;
            mortarAttrib.healthBar.UpdateHealth(mortarAttrib.health, mortarAttrib.maxHealth); //Updates the towers UI
        }
        else return;
    }
    public void UpgradeMortarFireRate()
    {
        GameObject mortarTower = transform.root.gameObject;
        MortarDefender mortarAttrib = mortarTower.GetComponent<MortarDefender>(); //Allows you to change the attributes of the generic yellow Tower
        mortarAttrib.upgradeFireRateCounter += 1;
        if (mortarAttrib.upgradeFireRateCounter < upgradeMax)
        {
            playerInformation.gold -= 500;
            mortarAttrib.rpm -= 2f;
        }
        else return;
    }
    #endregion
}
