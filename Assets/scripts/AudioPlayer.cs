using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource pickUp;
    public AudioSource EnemyCollisionEnemyWins;
    public AudioSource EnemyCollisionPlayerWins;
    public AudioSource WaveChange;
    public AudioSource EnemyAlert;
    public AudioSource StunShotHit;
    public AudioSource StunShotFired;
    [SerializeField]
    private AudioSource[] allAudio;

    // Start is called before the first frame update
    void Start()
    {
        allAudio = (AudioSource[])GameObject.FindObjectsOfType(typeof(AudioSource));
        foreach (AudioSource audio in allAudio)
        {
            audio.volume = PlayerPrefs.GetFloat("Volume");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methods to recieve messages from other scripts and trigger appropriate audio

    void PlayPickUp()
    {
        pickUp.Play();
    }

    void PlayEnemyCollisionEnemyWins()
    {
        EnemyCollisionEnemyWins.Play();
    }

    void PlayEnemyCollisionPlayerWins()
    {
        EnemyCollisionPlayerWins.Play();
    }

    void PlayWaveChange()
    {
        WaveChange.Play();
    }

    void PlayEnemyAlert()
    {
        EnemyAlert.Play();
    }

    void PlayStunShotHit()
    {
        StunShotHit.Play();
    }

    void PlayStunShotFired()
    {
        StunShotFired.Play();
    }
}
