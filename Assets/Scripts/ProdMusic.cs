using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProdMusic : MonoBehaviour
{

    public AudioSource audioSource;
    [Header("Place Clips here ")]
    public AudioClip[] listAmbient;
    public AudioClip[] listAction;
    public AudioClip[] listOther;
    [Header("Player information")]
    
    public PlayerRes playerInformation;
    public float gold;

    private AudioClip currentClip;
    private AudioClip lastClip;

    //Fetching number of enemies in scene
    private GameObject[] Enemies; 
    private int enemiesCount;

    
    void Start()
    {
        // Get player information for gold tracking
        playerInformation = GameObject.FindGameObjectWithTag("WorldController").GetComponent<PlayerRes>();

        // Start with a random ambient track
        PlayRandomClip(listAmbient);
        
    }

    void Update()
    {
        gold = playerInformation.gold; // Update gold value from player information
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesCount = Enemies.Length;
        CheckPlayerGold();

        // Restart the current clip if it has finished playing
        if (!audioSource.isPlaying)
        {
            PlayRandomClip(GetCurrentPlaylist());
        }
    }

    private void CheckPlayerGold()
    {
        
        if (enemiesCount > 1 || gold > 300)
        {
            if (!IsCurrentPlaylist(listAction))
                PlayRandomClip(listAction);
        }
        else if ( enemiesCount > 5 && gold > 200 )
        {
            if (!IsCurrentPlaylist(listOther))
                PlayRandomClip(listOther);
        }
        else
        {
            if (!IsCurrentPlaylist(listAmbient))
                PlayRandomClip(listAmbient);
        }
    }

    private void PlayRandomClip(AudioClip[] playlist)
    {
        if (playlist.Length == 0) return;

        
        int randomIndex = Random.Range(0, playlist.Length);
        currentClip = playlist[randomIndex];

        if (currentClip == lastClip && playlist.Length > 1)
        {
            // Select the next clip in the list if it's the same as the last clip
            randomIndex = (randomIndex + 1) % playlist.Length;
            currentClip = playlist[randomIndex];
        }

        lastClip = currentClip;

        audioSource.clip = currentClip;
        audioSource.Play();
    }

    private AudioClip[] GetCurrentPlaylist()
    {
        if (gold > 300) return listAction;
        if (gold > 100) return listOther;
        return listAmbient;
    }

    private bool IsCurrentPlaylist(AudioClip[] playlist)
    {
        return System.Array.Exists(playlist, clip => clip == currentClip);
    }
}
