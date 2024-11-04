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

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
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
        }
        else return;
    }
    #endregion
    #region Ripple Tower Upgrades
    public void UpgradeRippleHealth()
    {
        GameObject ripple = transform.root.gameObject;
        RippleDefender rippleS = ripple.GetComponent<RippleDefender>();
        if (rippleS.upgradeHealthCounter < upgradeMax)
        {
            rippleS.maxHealth += 50;
            rippleS.health += 50;
            rippleS.healthBar.UpdateHealth(rippleS.health, rippleS.maxHealth); //Updates the towers UI
        }
        else return;
    }
    public void UpgradeRippleFireRate()
    {
        GameObject rippleTower = transform.root.gameObject;
        DefenderTower rippleAttrib = rippleTower.GetComponent<DefenderTower>(); //Allows you to change the attributes of the generic yellow Tower
        rippleAttrib.upgradeFireRateCounter += 1;
        if (rippleAttrib.upgradeFireRateCounter < upgradeMax)
        {
            rippleAttrib.rpm -= 0.33f;
        }
        else return;
    }
    #endregion
    #region Mortar Defender Upgrade
    public void UpgradeMortarHealth()
    {
        GameObject morterTower = transform.root.gameObject;
        DefenderTower mortarAttrib = morterTower.GetComponent<DefenderTower>(); //Allows you to change the attributes of the generic yellow Tower
        mortarAttrib.upgradeHealthCounter += 1;
        //Sets the counters for easier use
        if (mortarAttrib.upgradeHealthCounter < upgradeMax)
        {
            mortarAttrib.maxHealth += 50;
            mortarAttrib.health += 50;
            mortarAttrib.healthBar.UpdateHealth(mortarAttrib.health, mortarAttrib.maxHealth); //Updates the towers UI
        }
        else return;
    }
    public void UpgradeMortarFireRate()
    {
        GameObject mortarTower = transform.root.gameObject;
        DefenderTower mortarAttrib = mortarTower.GetComponent<DefenderTower>(); //Allows you to change the attributes of the generic yellow Tower
        mortarAttrib.upgradeFireRateCounter += 1;
        if (mortarAttrib.upgradeFireRateCounter < upgradeMax)
        {
            mortarAttrib.rpm -= 2f;
        }
        else return;
    }
    #endregion
}
