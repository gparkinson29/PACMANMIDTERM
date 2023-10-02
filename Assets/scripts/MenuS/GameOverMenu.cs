using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Text finalWaveStat;

    // Start is called before the first frame update
    void Start()
    {
        finalWaveStat.text = "\nWaves Survived: " + GameManager.waveCounterNum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
