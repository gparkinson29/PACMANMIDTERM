using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip buttonHover;
    public AudioClip buttonPressed;

    public void HoverSound()
    {
        musicSource.PlayOneShot(buttonHover);
    }
    public void ClickSound()
    {
        musicSource.PlayOneShot(buttonPressed);
    }
}
