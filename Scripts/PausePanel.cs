using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] public GameObject gearBox;
    [SerializeField] public GameObject turnArrows;
    

    public void Quit_button()
    {
        
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Resume_button()
    {
        Time.timeScale = 1;
        gearBox.SetActive(true);
        turnArrows.SetActive(true);
        gameObject.SetActive(false);
        
    }

    public void Restart_button()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
