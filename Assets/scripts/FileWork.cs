using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileWork
{
    //---setting up a high score file if none exists
    public static void CreateDefaultScoreList(string highScoreFile)
    {
        StreamWriter sw = null;
        try
        {

            sw = new StreamWriter(Application.persistentDataPath + "/" + highScoreFile);
            for (int i = 0; i < 5; i++)
            {
                sw.WriteLine("Name, 0.0");
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            sw.Dispose();
        }
    }

    public static void ClearFile(string highScoreFile)
    {
        StreamWriter sw = null;
        try
        {
            sw = new StreamWriter(Application.persistentDataPath + "/" + highScoreFile);
            for (int i = 0; i < 5; i++)
            {
                sw.WriteLine("");
            }
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            sw.Dispose();
        }
    }

    public static void SaveScoresToFile(string highScoreFile, HighScore[] scoreList)
    {
        StreamWriter sw = null;
        try
        {
            sw = new StreamWriter(Application.persistentDataPath + "/" + highScoreFile);
            for (int i = 0; i < scoreList.Length; i++)
            {
                sw.WriteLine(scoreList[i].ToString());
                sw.Flush();
            }
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message);
        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            sw.Dispose();
        }
    }

    public static HighScore[] ReadScoresFile(string highScoreFile)
    {
        HighScore[] scores = new HighScore[5];
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(Application.persistentDataPath + "/" + highScoreFile);
            string dataLine = "", initials;
            int count = 0, score;
            dataLine = sr.ReadLine();
            while (dataLine != null)
            {
                string[] values = dataLine.Split(",");
                initials = values[0];
                score = Int32.Parse(values[1]);
                HighScore highScore = new HighScore(score, initials);
                scores[count] = highScore;
                count++;
                dataLine = sr.ReadLine();
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            sr.Close();
        }
        return scores;
    }
}
