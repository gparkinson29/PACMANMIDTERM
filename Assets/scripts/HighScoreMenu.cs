using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] scoreText;
    [SerializeField]
    private string fileName;
    private HighScore[] highScoreDisplay;
    
    void Awake()
    {
        highScoreDisplay = FileWork.ReadScoresFile(fileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FillText()
    {
        for (int i = 0; i<scoreText.Length; i++)
        {
            scoreText[i].text = highScoreDisplay[i].ToString();
        }
    }
}
