using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureSystem : MonoBehaviour
{
    private GameManager gm;
    private bool isDoneLuring;

    // Start is called before the first frame update
    void Start()
    {
        gm = Camera.main.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForLureComplete();
    }

    void CheckForLureComplete()
    {
        isDoneLuring = gm.enemies.TrueForAll(element => element.GetLured() == false);
        if (isDoneLuring)
        {
            GameObject fadeout = (GameObject)Instantiate(Resources.Load("Particle Systems/Dissipate"), this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
