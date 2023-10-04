using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissipationScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(this.gameObject);
    }
}
