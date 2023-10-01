using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject playerSpawnPos;
    [SerializeField]
    private string playerPrefabName;
    [SerializeField]
    public bool isHighScore, isGameOver, isHitFront, isHitBack, hasNoTail;
    [SerializeField]
    private int currentScore;
    private HighScoreManager scoreManager;
    [SerializeField]
    private InputField initialsField;
    [SerializeField]
    private Button submitHighScoreButton;
    [SerializeField]
    private string scoreFileName;
    [SerializeField]
    private Pellet[] pellets;
    private bool isPlayerAlive;
    private int waveCounterNum;
    private int aliveEnemies;


    public GameObject thePauseMenu; 
    public TextMeshProUGUI tailCounter;
    public TextMeshProUGUI waveCounter; 
    public GameObject player1;
    public GameObject oneenemy1;
    public GameObject twoenemy2;
    public GameObject threeenemy3;
    public GameObject fourenemy4; 
    public Player playerInfo;
    public int enemy1dam;
    public int enemy2dam;
    public int enemy3dam;
    public int enemy4dam;
    public bool showMenu;
    
    public GameObject enemyJail;
    public GameObject enemy1spawn;
    public GameObject enemy2spawn;
    public GameObject enemy3spawn;
    public GameObject enemy4spawn;

    GameObject camera;


    void Awake()
    {
        isPlayerAlive = false;
        Debug.Log("I WANT TO LIVE");
        SpawnPlayer();
        isHighScore = false;
        isGameOver = false;
        isHitFront = false;
        isHitBack = false;
        hasNoTail = true;
        waveCounterNum = 1;
        showMenu = false; 
        // initialsField.enabled = false;
        // initialsField.characterLimit = 3;

        player1 = GameObject.FindWithTag("Player");
        playerInfo = player1.GetComponent<Player>();

        oneenemy1 = GameObject.FindWithTag("enemy1");
        twoenemy2 = GameObject.FindWithTag("enemy2");
        threeenemy3 = GameObject.FindWithTag("enemy3");
        fourenemy4 = GameObject.FindWithTag("enemy4");

        camera = GameObject.FindGameObjectWithTag("MainCamera"); // assign camera object 

    }

    // Start is called before the first frame update
    void Start()
    {
        enemy1dam = 5;
        enemy2dam = 5;
        enemy3dam = 5;
        enemy4dam = 5;
        aliveEnemies = 4; 
    }

    // Update is called once per frame
    void Update()
    {
        nextWave();
        pauseMenu(); 
    }

    void FixedUpdate()
    {
        CheckForTail(); 
        tailCounter.text = "Tail Length: " + playerInfo.tailLength.ToString();
        waveCounter.text = "Wave: " + waveCounterNum.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
           // showMenu = (showMenu) ? false : true;
        }
    }

    void pauseMenu()
    {
        if (showMenu)
        {
            thePauseMenu.SetActive(true);
            
        }
        else
        {
            thePauseMenu.SetActive(false);
        }
    
    
    }
    
    
    void SpawnPlayer() //need to make this it's own method because we'll be resetting the player back to the center of the maze between waves
    {
        if (isPlayerAlive == false)
        {
            isPlayerAlive = true; 
            GameObject playerPrefab = (GameObject)Instantiate(Resources.Load(playerPrefabName));
            playerPrefab.transform.position = playerSpawnPos.transform.position;
        }
        else
        {
            Debug.Log("I'm already alive, dummy!");
        }
    }

    void CheckForTail()
    {
        if(playerInfo.tailLength > 0)
        {
            hasNoTail = false; 
        }
    }

    public void EndGame()
    {
        if ((isHitFront) || (isHitBack && hasNoTail))
        {
        //    isHighScore = scoreManager.CheckForHighScore(currentScore);
       //     if (isHighScore)
       //     {
       //         initialsField.gameObject.SetActive(true);
       //         initialsField.enabled = true;
        //        initialsField.ActivateInputField();
        //        submitHighScoreButton.gameObject.SetActive(true);
       //     }
       //     else
        //    {
                SceneManager.LoadScene(0);
            }
            
        }
 //   }

    void ReturnToTitle()
    {
        scoreManager.AddNewScore(currentScore, initialsField.text);
        FileWork.ClearFile(scoreFileName);
        scoreManager.SaveScores();
        SceneManager.LoadScene(0);
    }

    void RespawnPellets()
    {

    }

    void enemyResetPositions()
    {
        oneenemy1.transform.position = enemy1spawn.transform.position;
        twoenemy2.transform.position = enemy2spawn.transform.position;
        threeenemy3.transform.position = enemy3spawn.transform.position;
        fourenemy4.transform.position = enemy4spawn.transform.position;

        oneenemy1.SetActive(true);
        twoenemy2.SetActive(true);
        threeenemy3.SetActive(true);
        fourenemy4.SetActive(true);
    }

    void advancingWaves(int tracks)
    {
        switch (tracks)
        {
            case 2:

                enemy1dam = 5;
                enemy2dam = 5;
                enemy3dam = 5;
                enemy4dam = 6;

                enemyResetPositions();
                
                aliveEnemies = 4;

                
            
                break; 
            
            case 3:

                enemy1dam = 5;
                enemy2dam = 6;
                enemy3dam = 6;
                enemy4dam = 7;

                enemyResetPositions();

                aliveEnemies = 4;

                

                break; 
            
            case 4:

                enemy1dam = 7;
                enemy2dam = 7;
                enemy3dam = 8;
                enemy4dam = 9;

                enemyResetPositions();

                aliveEnemies = 4;

                

                break;
            
            case 5:

                enemy1dam = 8;
                enemy2dam = 9;
                enemy3dam = 10;
                enemy4dam = 10;

                enemyResetPositions();

                aliveEnemies = 4;

                

                break;

            case 6:

            SceneManager.LoadScene(0);
                break; 

        }
    }

    void nextWave()
    {
        if (aliveEnemies == 0)
        {

            waveCounterNum++;
            advancingWaves(waveCounterNum);
            camera.gameObject.SendMessage("PlayWaveChange", SendMessageOptions.DontRequireReceiver);

            //SceneManager.LoadScene(0);
        }
    }

    public void tailTime()
    {
        int wish = playerInfo.tailLength/2;
        playerInfo.DecreaseTail();

    }


    public void checkKill(string tag)
    {
        switch (tag)
        {
            case "enemy1":
                if (playerInfo.tailLength >= enemy1dam)
                {
                    Debug.Log("Enemy 1 down!");
                    playerInfo.DecreaseTail(enemy1dam);
                    oneenemy1.SetActive(false);
                    aliveEnemies--;
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    Debug.Log(aliveEnemies);
                }
                else
                {
                    isHitFront = true;
                    camera.gameObject.SendMessage("PlayEnemyCollisionEnemyWins", SendMessageOptions.DontRequireReceiver);
                    EndGame();
                }
                break;
            case "enemy2":
                if (playerInfo.tailLength >= enemy2dam)
                {
                    Debug.Log("Enemy 2 down!");
                    playerInfo.DecreaseTail(enemy2dam);
                    twoenemy2.SetActive(false);  
                    aliveEnemies--;
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    Debug.Log(aliveEnemies);
                }
                else
                {
                    isHitFront = true;
                    camera.gameObject.SendMessage("PlayEnemyCollisionEnemyWins", SendMessageOptions.DontRequireReceiver);
                    EndGame();
                }
                break;
            case "enemy3":
                if(playerInfo.tailLength >= enemy3dam)
                {
                    Debug.Log("Enemy 3 down!");
                    playerInfo.DecreaseTail(enemy3dam);
                    threeenemy3.SetActive(false);
                    aliveEnemies--;
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    Debug.Log(aliveEnemies);
                }
                else
                {
                    isHitFront = true;
                    camera.gameObject.SendMessage("PlayEnemyCollisionEnemyWins", SendMessageOptions.DontRequireReceiver);
                    EndGame();
                }
                break;
            case "enemy4":
                if(playerInfo.tailLength >= enemy4dam)
                {
                    Debug.Log("Enemy 4 down!");
                    playerInfo.DecreaseTail(enemy4dam);
                    fourenemy4.SetActive(false);
                    aliveEnemies--;
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    Debug.Log(aliveEnemies);
                }
                else
                {
                    isHitFront = true;
                    camera.gameObject.SendMessage("PlayEnemyCollisionEnemyWins", SendMessageOptions.DontRequireReceiver);
                    EndGame();
                }
                break; 
        }
    }

}
