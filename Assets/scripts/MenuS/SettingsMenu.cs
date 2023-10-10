using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private AudioSource audioSource;

    void Awake()
    {
        soundSlider.onValueChanged.AddListener((t) => OnVolumeSliderChange(soundSlider.value));
    }

    // Start is called before the first frame update
    void Start()
    {
        soundSlider.value = PlayerPrefs.GetFloat("Volume");
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
    }

    public void OnVolumeSliderChange(float newValue)
    {
        soundSlider.value = newValue;
        audioSource.volume = newValue;
        PlayerPrefs.SetFloat("Volume", newValue);
        PlayerPrefs.Save();
    }

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
