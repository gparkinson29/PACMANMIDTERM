using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource pickUp;
    public AudioSource EnemyCollision;

    // Start is called before the first frame update
    void Start()
    {
        
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

    void PlayEnemyCollision()
    {
        EnemyCollision.Play();
    }
}
