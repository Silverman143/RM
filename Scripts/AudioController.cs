using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    AudioSource backGroundSound;
    private bool soundOn;
    private bool musicOn;

    public AudioSource wheelsCreak;

    private void Start()
    {
        wheelsCreak = GameObject.Find("WheelsCreak").GetComponent<AudioSource>();
        WheelsCreakController(true);
        soundOn = SettingsButtons.soundOn;
        musicOn = SettingsButtons.musicOn;

        AudioSource bgMusic = GameObject.Find("BGSound").GetComponent<AudioSource>();
        AudioListener audioL = GameObject.Find("Main Camera").GetComponent<AudioListener>();

        audioL.enabled = soundOn;
        bgMusic.enabled = musicOn;
    }

    public void WheelsCreakController(bool _enabled)
    {
        if (_enabled == true & wheelsCreak.isPlaying != true)
        {
            wheelsCreak.Play();
        }
        if (_enabled == false & wheelsCreak.isPlaying != false)
        {
            wheelsCreak.Stop();
        }

    }
}
