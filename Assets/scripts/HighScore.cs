using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HighScore
{
    private int score;
    private string playerInitials;

    public HighScore(int score, string playersInitials)
    {
        this.score = score;
        this.playerInitials = playersInitials;
    }

    public override string ToString()
    {
        return playerInitials + ", " + score;
    }


    public float getHighScore()
    {
        return score;
    }

    public void setHighScore(int score)
    {
        this.score = score;
    }

}
