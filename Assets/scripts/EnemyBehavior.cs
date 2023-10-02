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

    private bool chase, flee, lured;
    string nombre; 

    int m_CurrentWaypointIndex = 0;



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
    }

    // Update is called once per frame
    void Update()
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

                int randomnum = Random.Range(0, 4);

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
    }

    //---Responses to powerups---
    public void Stun()
    {
        navMeshAgent.speed = 0f;
        StartCoroutine(StunCoroutine());
    }

    public void Lure(Vector3 positionToMove)
    {
        navMeshAgent.SetDestination(positionToMove);
        lured = true;
        StartCoroutine(LureCoroutine(positionToMove));
    }


    IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(5f);
        navMeshAgent.speed = 3f;
    }

    IEnumerator LureCoroutine(Vector3 positionToMove)
    {
        while (lured)
        {
            if (Vector3.Distance(this.transform.position, positionToMove) < 2f)
            {

                yield return new WaitForSeconds(0.5f);
                lured = false;
            }
            yield return null;
        }
    }


    public bool GetLured()
    {
        return lured;
    }




}
