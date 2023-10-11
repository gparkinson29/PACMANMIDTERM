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
    public static bool playerDied, playerWon;
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
    public static int waveCounterNum;
    private int aliveEnemies;
    public List<EnemyBehavior> enemies;
    [SerializeField]
    private GameObject errorDialogueBox;
    [SerializeField]
    private GameObject pelletParent;
    private Transform[] allPellets;
    private Transform[] currentPellets;

    public GameObject thePauseMenu;
    public GameObject waveMenu;
    public TextMeshProUGUI enemy1HP;
    public TextMeshProUGUI enemy2HP;
    public TextMeshProUGUI enemy3HP;
    public TextMeshProUGUI enemy4HP; 
    public TextMeshProUGUI tailCounter;
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI scoreCounter; 
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
    public bool betweenWaves; 
    
    public GameObject enemyJail;
    public GameObject enemy1spawn;
    public GameObject enemy2spawn;
    public GameObject enemy3spawn;
    public GameObject enemy4spawn;

    GameObject camera;
    


    void Awake()
    {

        Score.playerScore = 0; 
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

        enemies.Add(oneenemy1.GetComponent<EnemyBehavior>());
        enemies.Add(twoenemy2.GetComponent<EnemyBehavior>());
        enemies.Add(threeenemy3.GetComponent<EnemyBehavior>());
        enemies.Add(fourenemy4.GetComponent<EnemyBehavior>());

        camera = GameObject.FindGameObjectWithTag("MainCamera"); // assign camera object 
        allPellets = pelletParent.GetComponentsInChildren<Transform>(true);
        playerDied = false;
        playerWon = false;
        betweenWaves = true;
        
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
        wavePause();
        if (Input.GetKeyDown(KeyCode.X) && betweenWaves)
        {
            betweenWaves = false;
            StartCoroutine(SpawnVFXTimeout(false));
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showMenu = (showMenu) ? false : true;
        }

        menuCheck();

    }

    void menuCheck()
        {
            if (showMenu)
            {
                thePauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            if (!showMenu && !betweenWaves)
            {
                thePauseMenu.SetActive(false);
                Time.timeScale = 1f; 
            }
            if (showMenu && betweenWaves)
            {
                waveMenu.SetActive(false);
                thePauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        
        }
    

    void FixedUpdate()
    {

        nextWave();

        CheckForTail(); 
        tailCounter.text = "Tail Length: " + playerInfo.tailLength.ToString();
        waveCounter.text = "Wave: " + waveCounterNum.ToString();
        scoreCounter.text = "Score: " + Score.playerScore.ToString();
        UpdatePelletCount();
        RespawnPellets();
        
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
            player1.transform.position = playerSpawnPos.transform.position;
        }
    }

    void SpawnPellets()
    {
        for (int i=0; i<allPellets.Length; i++)
        {
            allPellets[i].gameObject.SetActive(true);
        }
    }

    void CheckForTail()
    {
        if(playerInfo.tailLength > 0)
        {
            hasNoTail = false; 
        }
    }

    void UpdatePelletCount()
    {
        currentPellets = pelletParent.GetComponentsInChildren<Transform>(false);
    }

    public void SetLure(Vector3 positionToMove)
    {
        for (int i = 0; i < aliveEnemies; i++)
        {
            enemies[i].Lure(positionToMove);
        }
    }

    public void SpawnVFXHandler(bool isActive)
    {
        for (int i = 0; i < aliveEnemies; i++)
        {
            enemies[i].SetSpawnVFXActive(isActive);
        }
    }

    IEnumerator SpawnVFXTimeout(bool isActive)
    {
        yield return new WaitForSeconds(1f);
        SpawnVFXHandler(false);
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
            //SceneManager.LoadScene(0);
            //}
            playerDied = true;
            SceneManager.LoadScene(5);

        }
           }

        public void ReturnToTitle()
    {
        // scoreManager.AddNewScore(currentScore, initialsField.text);
        // FileWork.ClearFile(scoreFileName);
        // scoreManager.SaveScores();


        
        //SceneManager.LoadScene(0);
    }


    void AddPellets()
    {
        
    }

    void RespawnPellets()
    {
        if (currentPellets.Length<=enemy4dam)
        {
            for (int i = 0; i<5; i++)
            {
                int randomIndex = Random.Range(0, allPellets.Length);
                if (!allPellets[randomIndex].gameObject.activeSelf)
                {
                    allPellets[randomIndex].gameObject.SetActive(true);
                }
                else
                {
                    continue;
                }
            }
        }
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

                enemy1dam = 6;
                enemy2dam = 5;
                enemy3dam = 5;
                enemy4dam = 6;

                enemyResetPositions();
                Score.playerScore = Score.playerScore + 100;

                aliveEnemies = 4;
                enemies.Add(oneenemy1.GetComponent<EnemyBehavior>());
                enemies.Add(twoenemy2.GetComponent<EnemyBehavior>());
                enemies.Add(threeenemy3.GetComponent<EnemyBehavior>());
                enemies.Add(fourenemy4.GetComponent<EnemyBehavior>());
                SpawnVFXHandler(true);
                betweenWaves = true;
               // wavePause();

                break; 
            
            case 3:

                enemy1dam = 5;
                enemy2dam = 6;
                enemy3dam = 6;
                enemy4dam = 7;

                enemyResetPositions();
                Score.playerScore = Score.playerScore + 100;


                aliveEnemies = 4;
                enemies.Add(oneenemy1.GetComponent<EnemyBehavior>());
                enemies.Add(twoenemy2.GetComponent<EnemyBehavior>());
                enemies.Add(threeenemy3.GetComponent<EnemyBehavior>());
                enemies.Add(fourenemy4.GetComponent<EnemyBehavior>());
                SpawnVFXHandler(true);
                betweenWaves = true;
               // wavePause();

                break; 
            
            case 4:

                enemy1dam = 7;
                enemy2dam = 7;
                enemy3dam = 8;
                enemy4dam = 9;

                enemyResetPositions();
                Score.playerScore = Score.playerScore + 100;
                SpawnVFXHandler(true);

                aliveEnemies = 4;
                enemies.Add(oneenemy1.GetComponent<EnemyBehavior>());
                enemies.Add(twoenemy2.GetComponent<EnemyBehavior>());
                enemies.Add(threeenemy3.GetComponent<EnemyBehavior>());
                enemies.Add(fourenemy4.GetComponent<EnemyBehavior>());
                SpawnVFXHandler(true);
                betweenWaves = true; 
               // wavePause();

                break;
            
            case 5:

                enemy1dam = 8;
                enemy2dam = 9;
                enemy3dam = 10;
                enemy4dam = 10;

                enemyResetPositions();
                Score.playerScore = Score.playerScore + 100;
                SpawnVFXHandler(true);

                aliveEnemies = 4;
                enemies.Add(oneenemy1.GetComponent<EnemyBehavior>());
                enemies.Add(twoenemy2.GetComponent<EnemyBehavior>());
                enemies.Add(threeenemy3.GetComponent<EnemyBehavior>());
                enemies.Add(fourenemy4.GetComponent<EnemyBehavior>());
                SpawnVFXHandler(true);
                betweenWaves = true; 
                //wavePause(); 

                break;

            case 6:

            SceneManager.LoadScene(5);

                playerWon = true;
                break; 

        }

    }

    void nextWave()
    {
        if (aliveEnemies == 0)
        {

            waveCounterNum++;
            advancingWaves(waveCounterNum);
            SpawnPellets();
            camera.gameObject.SendMessage("PlayWaveChange", SendMessageOptions.DontRequireReceiver);
            SpawnPlayer();
            
            //SceneManager.LoadScene(0);
        }
    }

    public void wavePause()
    {
        enemy1HP.text = "The red enemy has " + enemy1dam.ToString() + " hit points.";
        enemy2HP.text = "The blue enemy has " + enemy2dam.ToString() + " hit points.";
        enemy3HP.text = "The orange enemy has " + enemy3dam.ToString() + " hit points.";
        enemy4HP.text = "The purple enemy has " + enemy4dam.ToString() + " hit points.";

        if (betweenWaves && !showMenu)
        {
            waveMenu.SetActive(true);
            thePauseMenu.SetActive(false);
            Time.timeScale = 0f;
            
            
        }
        if(!betweenWaves)
        {
            Time.timeScale = 1f;
            waveMenu.SetActive(false);
            
        }

    }

    public void tailTime()
    {
        int wish = playerInfo.tailLength/2;
        playerInfo.DecreaseTail(wish);

    }


    public void checkKill(string tag)
    {
        switch (tag)
        {
            case "enemy1":
                if ((playerInfo.tailLength >= enemy1dam) && (!oneenemy1.GetComponent<EnemyBehavior>().stunned))
                {
                    Debug.Log("Enemy 1 down!");
                    Score.playerScore = Score.playerScore + 50;
                    enemies.Remove(oneenemy1.GetComponent<EnemyBehavior>());
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    playerInfo.DecreaseTail(enemy1dam);
                    oneenemy1.SetActive(false);
                    //Destroy(oneenemy1);
                    aliveEnemies--;
                }
                else if (oneenemy1.GetComponent<EnemyBehavior>().stunned)
                {

                }
                else
                {
                    isHitFront = true;
                    EndGame();
                }
                break;
            case "enemy2":
                if ((playerInfo.tailLength >= enemy2dam) && (!twoenemy2.GetComponent<EnemyBehavior>().stunned))
                {
                    Debug.Log("Enemy 2 down!");
                    Score.playerScore = Score.playerScore + 50;
                    enemies.Remove(twoenemy2.GetComponent<EnemyBehavior>());
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    playerInfo.DecreaseTail(enemy2dam);
                    twoenemy2.SetActive(false);
                    //Destroy(twoenemy2);
                    aliveEnemies--;
                }
                else if (twoenemy2.GetComponent<EnemyBehavior>().stunned)
                {

                }
                else
                {
                    isHitFront = true;
                    EndGame();
                }
                break;
            case "enemy3":
                if ((playerInfo.tailLength >= enemy3dam) && (!threeenemy3.GetComponent<EnemyBehavior>().stunned))
                {
                    Debug.Log("Enemy 3 down!");
                    Score.playerScore = Score.playerScore + 50;
                    enemies.Remove(threeenemy3.GetComponent<EnemyBehavior>());
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    playerInfo.DecreaseTail(enemy3dam);
                    threeenemy3.SetActive(false);
                    //Destroy(threeenemy3);
                    aliveEnemies--;
                }
                else if (threeenemy3.GetComponent<EnemyBehavior>().stunned)
                {

                }
                else
                {
                    isHitFront = true;
                    EndGame();
                }
                break;
            case "enemy4":
                if ((playerInfo.tailLength >= enemy4dam) && (!fourenemy4.GetComponent<EnemyBehavior>().stunned))
                {
                    Debug.Log("Enemy 4 down!");
                    Score.playerScore = Score.playerScore + 50;
                    camera.gameObject.SendMessage("PlayEnemyCollisionPlayerWins", SendMessageOptions.DontRequireReceiver);
                    enemies.Remove(fourenemy4.GetComponent<EnemyBehavior>());
                    playerInfo.DecreaseTail(enemy4dam);
                    fourenemy4.SetActive(false);
                    //Destroy(fourenemy4);
                    aliveEnemies--;
                }
                else if (fourenemy4.GetComponent<EnemyBehavior>().stunned)
                {

                }
                else
                {
                    isHitFront = true;
                    EndGame();
                }
                break;
        }
    }


    //---UI---
    public void StartErrorDialogueBox()
    {
        errorDialogueBox.gameObject.SetActive(true);
        StartCoroutine(DisplayErrorDialogue());
    }

    IEnumerator DisplayErrorDialogue()
    {
        yield return new WaitForSeconds(2f);
        errorDialogueBox.gameObject.SetActive(false);
    }



}
