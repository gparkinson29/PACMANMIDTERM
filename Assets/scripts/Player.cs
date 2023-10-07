using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float xInput, zInput;
    [SerializeField]
    private NavMeshAgent nma;
    [SerializeField]
    private Vector3 movementDirection, currentPosition, lastPosition;
    [SerializeField]
    public int tailLength, spacing;
    [SerializeField]
    private string pelletPrefabName;
    [SerializeField]
    public List<Vector3> pastPositions;
    [SerializeField]
    private List<TailComponent> tailComponents;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawn;
    private GameManager gm;
    [SerializeField]
    private AudioSource errorAudio;
    [SerializeField]
    private ParticleSystem lureBeacon;
    private bool hasTail;
    GameObject camera;  //Used for Audio

    void Awake()
    {
        xInput = 0.0f;
        zInput = 0.0f;
        tailLength = 0;
        spacing = 10;
        tailComponents = new List<TailComponent>(10);
        pastPositions = new List<Vector3>(100);

        camera = GameObject.FindGameObjectWithTag("MainCamera");  //assign camera
        gm = Camera.main.GetComponent<GameManager>();
        hasTail = false;
    }

    void Update()
    {

        if (pastPositions.Count > 210)
        {
            pastPositions.RemoveAt(210);
        }

        if (tailComponents.Count > 0)
        {
            hasTail = true;
        }
        else
        {
            hasTail = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion newRot = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.fixedDeltaTime * 10);
        }



        //if(movementDirection != Vector3.zero)
        //{
        //transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //}

        movementDirection = new Vector3(xInput, 0f, zInput);
        if (hasTail)
        {
            Vector3 toFirstComponent = this.transform.position - tailComponents[0].transform.position;
            if (Vector3.Dot(movementDirection.normalized, toFirstComponent.normalized) <=-0.5)
            {
                movementDirection = Vector3.zero;
            }
            else
            {
                nma.Move(movementDirection * Time.fixedDeltaTime * nma.speed);
            }
        }
        else
        {
            nma.Move(movementDirection * Time.fixedDeltaTime * nma.speed);
        }




        if (movementDirection != Vector3.zero)
        {
            lastPosition = currentPosition;
            currentPosition = this.transform.position;
            if (lastPosition!=currentPosition)
            {
                pastPositions.Insert(0, this.transform.position);
                DrawTail();
            } 
        }
        


    }

    //---Collision Handling
    void OnCollisionEnter(Collision other)
    {
    }

    void OnTriggerEnter(Collider other)
    {

    }

    //---Input Action Events---
    void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        xInput = inputVector.x;
        zInput = inputVector.y;
    }

    void OnStun()
    {
        if (ValidateComponentRemoval(1))
        {
            camera.gameObject.SendMessage("PlayStunShotFired", SendMessageOptions.DontRequireReceiver); //trigger appropriate audio
            DecreaseTail(1);
            Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        }
        else
        {
            Debug.LogError("tail length not long enough");
            gm.StartErrorDialogueBox();
        }
    }

    void OnDash()
    {
        if (ValidateComponentRemoval(2))
        {
            nma.speed = 6.5f;
            DecreaseTail(2);
        }
        else
        {
            Debug.LogError("tail length not long enough");
            gm.StartErrorDialogueBox();
        }
        StartCoroutine(DashCoroutine());
    }

    void OnLure()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1<<7))
        {
            Debug.Log("You clicked on a wall!");
            errorAudio.Play();
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1<<8))
        {
            Debug.Log(hit.collider.gameObject.layer);
            Debug.Log(hit.collider.gameObject.name);
            float distanceToHitPoint = Vector3.Distance(this.transform.position, hit.point);
            if (distanceToHitPoint < 5f)
            {
                if (ValidateComponentRemoval(2))
                {
                    DecreaseTail(2);
                    GameObject lure = (GameObject)Instantiate(Resources.Load("Particle Systems/Lure"), hit.point, Quaternion.identity);
                    gm.SetLure(hit.point);
                }
                else
                {
                    Debug.Log("tail length not long enough");
                    errorAudio.Play();
                    gm.StartErrorDialogueBox();
                }
            }
            else if (distanceToHitPoint >= 5f && distanceToHitPoint < 10f)
            {
                if (ValidateComponentRemoval(5))
                {
                    DecreaseTail(4);
                    GameObject lure = (GameObject)Instantiate(Resources.Load("Particle Systems/Lure"), hit.point, Quaternion.identity);
                    gm.SetLure(hit.point);
                }
                else
                {
                    Debug.Log("tail length not long enough");
                    errorAudio.Play();
                    gm.StartErrorDialogueBox();
                }

            }
            else if (distanceToHitPoint >= 10f)
            {
                if (ValidateComponentRemoval(10))
                {
                    DecreaseTail(8);
                    GameObject lure = (GameObject)Instantiate(Resources.Load("Particle Systems/Lure"), hit.point, Quaternion.identity);
                    gm.SetLure(hit.point);
                }
                else
                {
                    Debug.Log("tail length not long enough");
                    errorAudio.Play();
                    gm.StartErrorDialogueBox();
                }
            }
        }
        else
        {
            Debug.Log("Point clicked was not on the NavMesh");
            errorAudio.Play();
        }
    }

        public bool ValidateComponentRemoval(int amountToRemove)
    {
        if (tailLength >= amountToRemove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //---Tail Handling Functions---
    void DrawTail()
    {
        int offset = 80;
        foreach (TailComponent tc in tailComponents)
        {
            int indexToRef = (int)(offset / nma.speed);
            Vector3 tailSegmentPosition = pastPositions[indexToRef];
            tc.transform.position = tailSegmentPosition;
            tc.transform.LookAt(tailSegmentPosition);
            offset += 50;
        }
    }

    void IncreaseTail()
    {
        int lastTailIndex = tailLength % 10;
        if (tailLength<10)
        {
            GameObject newTailPellet = (GameObject)Instantiate(Resources.Load(pelletPrefabName), new Vector3(0,-10,0), this.transform.rotation);
            tailComponents.Insert(lastTailIndex, newTailPellet.GetComponent<TailComponent>());
            tailComponents[lastTailIndex].GetComponentValue();
            
        }
        else
        {
            tailComponents[lastTailIndex].RaiseValue(1);
        }
        tailLength++;
    }

    public void DecreaseTail()
    {
        int lastTailIndex = tailLength % 10;
        if (tailLength > 10)
        {
            tailComponents[lastTailIndex-1].LowerValue(1);
            
        }
        else
        {
            TailComponent deadTailPellet = tailComponents[tailLength - 1];
            tailComponents.Remove(deadTailPellet);
            Destroy(deadTailPellet.gameObject);
        }

        tailLength--;
    }
        
        //when being called for manual reduction for powerups or killing ghosts
    
        public void DecreaseTail(int times)
        {
            

        for (int i = 0; i < times; i++) {

            if (tailLength > 10)
            {
                int lastTailIndex = tailLength % 10;
                tailLength--;
                tailComponents[lastTailIndex-1].LowerValue(1);
                
            }
            else
            {
                TailComponent deadTailPellet = tailComponents[tailLength - 1];
                tailComponents.Remove(deadTailPellet);
                Destroy(deadTailPellet.gameObject);
                //tailComponents.Insert(tailLength-1, new TailComponent t);

                tailLength--;
            }

        }
        }

    //---Skill Handling Coroutines---
    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(3f);
        nma.speed = 5; 
    }
}
