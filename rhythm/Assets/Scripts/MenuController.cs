using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public string mainMenuScene;
    public GameObject pauseMenu;
    public bool isPaused;

    //public static MenuController instance;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                resumeGame();
            } else
            {
                pauseGame();
            }
        }
        if(!Application.isFocused && !isPaused)
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        Debug.Log("pause");
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
                
        GameManager.instance.theMusic.Pause();
        GameManager.instance.theMusic.transform.localScale = new Vector3(GameManager.instance.theMusic.time,0,0);
    }

    public void resumeGame()
    {
        Debug.Log("resume");
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
        if(GameManager.instance.theMusic.transform.localScale.x != 0)
        {
            GameManager.instance.theMusic.Play();
            GameManager.instance.theMusic.time = GameManager.instance.theMusic.transform.localScale.x;
        }
    }
    public void returnMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
