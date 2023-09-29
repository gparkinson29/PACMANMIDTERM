using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    //public GameManager gameManager;

    void Update()
    {
        //if (gameManager.isGameOver == true)
        //{
        //SceneManager.LoadScene("GameOver");
      }


        public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {
        
        Application.Quit();
    }
    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }
    public void HowTo()
    {
        SceneManager.LoadScene("HowTo");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
