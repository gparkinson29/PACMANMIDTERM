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
    public Vector3 playerLocation;
    GameObject camera; // used for audio

    private bool chase, flee;
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
        //playerInfo = player1.GetComponent<Player>();

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
        if(dist < 5)
        {
            chase = true;
            camera.gameObject.SendMessage("PlayEnemyAlert", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            chase = false; 
        }
    }
    
    void behaviorTree()
    {
        if (chase) {

            navMeshAgent.SetDestination(player1.transform.position);

        }

        else if (flee) { }

        else {

            patrol();
        
        }
    }

    void patrol()
    {

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {

            int randomnum = Random.Range(0, 4);
            
            navMeshAgent.SetDestination(waypoints[randomnum].position);
            //Debug.Log(randomnum);

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

    void Stun()
    {
        navMeshAgent.radius = 0.1f;
        StartCoroutine(StunCoroutine());
    }


    IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(5f);
        navMeshAgent.radius = 1.0f;

    }


}
