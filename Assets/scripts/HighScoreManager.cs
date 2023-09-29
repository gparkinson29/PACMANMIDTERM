using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    HighScore[] scoreList;
    [SerializeField]
    private string scoreFileName;
    
    // Start is called before the first frame update
    void Awake()
    {
        scoreList = new HighScore[5];
        LoadScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScores()
    {
        scoreList = FileWork.ReadScoresFile(scoreFileName);
    }

    public void SaveScores()
    {
        FileWork.SaveScoresToFile(scoreFileName, scoreList);
    }

    public bool CheckForHighScore(int score)
    {
       if (score> scoreList[scoreList.Length-1].getHighScore())
        {
            return true;
        }
       else
        {
            return false;
        }
    }
    
    public void AddNewScore(int score, string initials)
    {
        for (int i = 0; i < scoreList.Length; i++)
        {
            if (score > scoreList[i].getHighScore())
            {
                for (int j= scoreList.Length-1; j>i; j--)
                {
                    scoreList[j] = scoreList[i - 1];
                }
                scoreList[i] = new HighScore(score, initials);
                break;
            }
        }
    }


  

   
}
