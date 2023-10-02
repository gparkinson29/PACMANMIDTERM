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
    }

    void Update()
    {

        if (pastPositions.Count > 110)
        {
            pastPositions.RemoveAt(110);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        movementDirection = new Vector3 (xInput, 0f, zInput);
        nma.Move(movementDirection * Time.deltaTime * nma.speed);
        
        if(movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
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
        }
    }

    void OnDash()
    {
        if (ValidateComponentRemoval(2))
        {
            nma.speed = 10;
            DecreaseTail(2);
        }
        else
        {
            Debug.LogError("tail length not long enough");
        }
        StartCoroutine(DashCoroutine());
    }

    void OnLure()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500))
        {
            float distanceToHitPoint = Vector3.Distance(this.transform.position, hit.point);
            if (distanceToHitPoint < 5f)
            {
                if (ValidateComponentRemoval(2))
                {
                    DecreaseTail(2);
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
                    gm.SetLure(hit.point);
                    gm.StartErrorDialogueBox();
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
        int offset = 50;
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
        yield return new WaitForSeconds(2f);
        nma.speed = 5; 
    }
}
