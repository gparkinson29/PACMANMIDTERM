using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectile : MonoBehaviour
{
    private int speed = 20;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private bool isHorizontal;

    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = this.transform.forward * speed;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="enemy1")
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
        if (other.gameObject.tag=="Wall")
        {
            Destroy(this.gameObject);
        }
    }

  
}
