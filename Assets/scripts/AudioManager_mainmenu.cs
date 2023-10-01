using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager_mainmenu : MonoBehaviour
{
    public AudioSource musicSource;
    //public AudioSource buttonSource;

    public AudioClip background;
    public AudioClip buttonHover;
    public AudioClip buttonPressed;

    public static AudioManager_mainmenu instance;
   
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            AudioManager_mainmenu.instance.GetComponent<AudioSource>().Stop();
        }
    }
    
}
