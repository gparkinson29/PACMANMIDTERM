using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Text finalWaveStat;
    [SerializeField]
    private Text endGameStateMsg;
    [SerializeField]
    private AudioSource gameOverConditionSound;
    [SerializeField]
    private AudioClip winSound, lossSound;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.playerDied && !GameManager.playerWon)
        {
            endGameStateMsg.text = "You died!";
            gameOverConditionSound.clip = lossSound;
            gameOverConditionSound.Play();
        }
        else if (!GameManager.playerDied && GameManager.playerWon)
        {
            endGameStateMsg.text = "Congratulations! You completed every wave!";
            gameOverConditionSound.clip = winSound;
            gameOverConditionSound.Play();
        }
        finalWaveStat.text = "\nWaves Survived: " + GameManager.waveCounterNum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
