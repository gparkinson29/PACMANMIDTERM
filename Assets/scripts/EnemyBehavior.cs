using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public GameManager info;
    public GameObject player1;
    public Player playerInfo; 
    public Vector3 playerLocation;
    GameObject camera; // used for audio
    public bool stunned; //manages enemy states
    private MeshRenderer enemyRenderer;
    private float enemyRed, enemyGreen, enemyBlue;
    [SerializeField]
    private string stunVFXName;
    [SerializeField]
    private Transform spawnVFX, stunnedVFX;


    private bool chase, flee, lured;
    string nombre; 

    int m_CurrentWaypointIndex = 0;

    void Awake()
    {
        //SetSpawnVFXActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        m_CurrentWaypointIndex = 0; 
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        chase = false;
        flee = false;
        info = Camera.main.GetComponent<GameManager>();
        nombre = this.tag;
        player1 = GameObject.FindWithTag("Player");
        playerInfo = player1.GetComponent<Player>();
        stunned = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera"); // assign camera object
        enemyRenderer = this.gameObject.GetComponent<MeshRenderer>();
        enemyRed = enemyRenderer.material.color.r;
        enemyGreen = enemyRenderer.material.color.g;
        enemyBlue = enemyRenderer.material.color.b;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        behaviorCheck();
        behaviorTree(); 
    }


    void behaviorCheck()
    {
        float dist = Vector3.Distance(this.transform.position, player1.transform.position);

        //Debug.Log(this.tag + " " + dist);
        
        if(dist < 5 && !lured)
        {
            chase = true;
            
        }
        else if(dist > 5)
        {
            chase = false; 
        }
    }
    
    void behaviorTree()
    {
        if (chase) {

            navMeshAgent.SetDestination(player1.transform.position);
            camera.gameObject.SendMessage("PlayEnemyAlert", SendMessageOptions.DontRequireReceiver);

        }

        else if (flee) { }
        else if (lured) { }
        else if (!lured && !chase)
        {
            patrol();
        }
        else
        {

        }
    }

    void patrol()
    {

        if (this.tag == "enemy1")
        {
            navMeshAgent.SetDestination(player1.transform.position);
        }
        else if (this.tag == "enemy2")
        {
            Vector3 sneaky = new Vector3(player1.transform.position.x - 1f, player1.transform.position.y + 0f, player1.transform.position.z - 1f);
            navMeshAgent.SetDestination(sneaky);
        }
        else if (this.tag == "enemy3")
        {
            Vector3 sneaky = new Vector3(player1.transform.position.x + 2f, player1.transform.position.y + 0f, player1.transform.position.z + 2f);
            navMeshAgent.SetDestination(sneaky);
        }
            
        
      else {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {

                int randomnum = Random.Range(0, waypoints.Length);

                navMeshAgent.SetDestination(waypoints[randomnum].position);
                //Debug.Log(randomnum);

            }
        }
        
    }

    public void OnTriggerEnter(Collider cos)
    {
        if (cos.gameObject.tag.Equals("Player"))
        {
            Debug.Log("boop");
            //camera.gameObject.SendMessage("PlayEnemyCollision", SendMessageOptions.DontRequireReceiver);   This was a test before collsion could determine who won the clash
            info.checkKill(nombre); 
        }
        if (cos.gameObject.tag == "Stun") //if the enemy is struck by a stun projectile, call the stun behavior and destroy the stun projectile
        {
            camera.gameObject.SendMessage("PlayStunShotHit", SendMessageOptions.DontRequireReceiver);
            Stun();
            Destroy(cos.gameObject);
        }
    }

    //---Responses to powerups---
    public void Stun()
    {
        stunned = true;
        navMeshAgent.speed = 1.5f;
        navMeshAgent.radius = 0.1f;
        stunnedVFX.gameObject.SetActive(true);
        enemyRenderer.material.color = new Color(enemyRed, enemyGreen, enemyBlue, 0.2f);
        Collider dan = this.GetComponent<Collider>();
        dan.enabled = false;
        StartCoroutine(StunCoroutine(dan));
    }

    public void Lure(Vector3 positionToMove)
    {
        chase = false;

        navMeshAgent.SetDestination(positionToMove);
           
        lured = true;
        StartCoroutine(LureCoroutine(positionToMove));
    }


    IEnumerator StunCoroutine(Collider cos)
    {
        yield return new WaitForSeconds(5f);
        stunned = false;
        navMeshAgent.radius = 1f;
        navMeshAgent.speed = 3.5f;
        stunnedVFX.gameObject.SetActive(false);
        enemyRenderer.material.color = new Color(enemyRed, enemyGreen, enemyBlue, 1f);
        cos.enabled = true; 

    }

    IEnumerator LureCoroutine(Vector3 positionToMove)
    {
        while (lured)
        {
            if (Vector3.Distance(this.transform.position, positionToMove) < 2f)
            {

                yield return new WaitForSeconds(1.25f);
                lured = false;
            }
            yield return null;
        }
    }


    public bool GetLured()
    {
        return lured;
    }

    public void SetSpawnVFXActive(bool isActive)
    {
        spawnVFX.gameObject.SetActive(isActive);
    }

}
