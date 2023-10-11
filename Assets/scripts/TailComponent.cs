using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailComponent : MonoBehaviour
{
    [SerializeField]
    private int value;
    [SerializeField]
    public MeshRenderer renderer;
    [SerializeField]
    private Material value1Mat, value2Mat, value3Mat, value4Mat, value5Mat;
    public GameManager info;

    void Start()
    {
        value = 1;
        info = Camera.main.GetComponent<GameManager>();
        renderer = this.GetComponent<MeshRenderer>();
        ReskinComponent();
    }


    public int GetComponentValue()
    {
        return value;
    }

    public void RaiseValue(int amountToRaise)
    {
        value += amountToRaise;
        ReskinComponent();
    }

    public void LowerValue(int amountToLower)
    {
        value -= amountToLower;
        ReskinComponent();
    }

    public void ReskinComponent()
    {
        if (value == 1)
        {
            renderer.material = value1Mat;
        }
        else if (value == 2)
        {
            renderer.material = value2Mat;
        }
        else if (value == 3)
        {
            renderer.material = value3Mat;
        }
        else if (value == 4)
        {
            renderer.material = value4Mat;
        }
        else if (value==5)
        {
            renderer.material = value5Mat;
        }
        else
        {
            Debug.LogError("no additional tail components allowed");
        }
    }

    public void OnTriggerEnter(Collider cos)
    {
        if (cos.gameObject.tag.Equals("Player"))
        {
            Debug.Log("you hit your tail");
            info.isHitFront = true;
            info.EndGame();
        }
        else if (cos.gameObject.tag.Equals("enemy1"))
        {
            Debug.Log("enemy1 contact tail!");
            info.tailTime();
            
        }
        else if (cos.gameObject.tag.Equals("enemy2"))
        {
            Debug.Log("enemy2 contact tail!");
            info.tailTime();
            
        }
        else if (cos.gameObject.tag.Equals("enemy3"))
        {
            Debug.Log("enemy3 contact tail!");
            info.tailTime();
            
        }
        else if (cos.gameObject.tag.Equals("enemy4"))
        {
            Debug.Log("enemy4 contact tail!");
            info.tailTime();
            
        }

    }


}
