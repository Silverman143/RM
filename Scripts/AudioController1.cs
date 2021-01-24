using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController1 : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioS = GameObject.Find("BGSound").GetComponent<AudioSource>();

        gameObject.SetActive(SettingsButtons.soundOn);
        audioS.enabled = SettingsButtons.musicOn;
    }
}
