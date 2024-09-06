using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRes : MonoBehaviour
{
    public int gold = 0;// In enemy script its add 5 gold per kill
    private float goldPerSecond = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        goldPerSecond -= Time.deltaTime;
        if(goldPerSecond <= 0 )
        {
            gold++;
            goldPerSecond = 1f;
        }
    }

}
