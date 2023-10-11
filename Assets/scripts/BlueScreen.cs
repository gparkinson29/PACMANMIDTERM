using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueScreen : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public GameManager info;
    public GameObject player1;
    public Player playerInfo;
    public Vector3 playerLocation;

    string nombre;
    int hunger;



    void Start()
    {
        info = Camera.main.GetComponent<GameManager>();
        nombre = this.tag;
        player1 = GameObject.FindWithTag("Player");
        playerInfo = player1.GetComponent<Player>();
        navMeshAgent.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        checkGameState();
        Hunt(hunger);
    }


    void checkGameState()
    {
        hunger = info.aliveEnemies;
    }

    void Hunt(int pace)
    {
        switch (pace)
        {

            case 3:
                navMeshAgent.speed = 2.5f;
                break;

            case 2:
                navMeshAgent.speed = 3.75f;
                break;

            case 1:
                navMeshAgent.speed = 5.5f;
                break;
        }

        navMeshAgent.SetDestination(player1.transform.position);
    }

    public void OnTriggerEnter(Collider cos)
    {
        if (cos.gameObject.tag.Equals("Player"))
        {

            info.checkKill(nombre);
        
        }
    }
}

