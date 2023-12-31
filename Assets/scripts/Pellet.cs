using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet: MonoBehaviour
{

    GameObject camera;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            this.gameObject.SetActive(false);
            other.gameObject.SendMessage("IncreaseTail", SendMessageOptions.DontRequireReceiver);
            Score.playerScore = Score.playerScore + 5; 

            //send message to play audio for pellet pick up
           camera.gameObject.SendMessage("PlayPickUp", SendMessageOptions.DontRequireReceiver);

        }
    }
}
