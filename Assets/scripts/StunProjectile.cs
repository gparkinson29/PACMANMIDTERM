using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectile : MonoBehaviour
{
    private int speed = 20;
    [SerializeField]
    private Rigidbody rb;

    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = this.transform.forward * speed;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnTriggerEnter(Collider other) //moved to EnemyBehavior, keeping this code commented in case that has any problems 
    {
        /*if (other.gameObject.tag=="enemy1")
        {
            other.gameObject.SendMessage("Stun", SendMessageOptions.DontRequireReceiver);
            camera.gameObject.SendMessage("PlayStunShotHit", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "enemy2")
        {
            other.gameObject.SendMessage("Stun", SendMessageOptions.DontRequireReceiver);
            camera.gameObject.SendMessage("PlayStunShotHit", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "enemy3")
        {
            other.gameObject.SendMessage("Stun", SendMessageOptions.DontRequireReceiver);
            camera.gameObject.SendMessage("PlayStunShotHit", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "enemy4")
        {
            other.gameObject.SendMessage("Stun", SendMessageOptions.DontRequireReceiver);
            camera.gameObject.SendMessage("PlayStunShotHit", SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
        */
    }

    void OnCollisionEnter(Collision other) //do not revise!!! project needs to have a non-trigger collider so it doesn't go through walls!
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

  
}
