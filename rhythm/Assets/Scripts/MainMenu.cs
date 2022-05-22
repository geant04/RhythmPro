using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public AudioSource menuMusic;
    public bool PlayingSong;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayingSong)
        {  
            PlayingSong = true;
            menuMusic.Play();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
        PlayingSong = false;
        menuMusic.Stop();
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");

        menuMusic.Stop();
    }

}
