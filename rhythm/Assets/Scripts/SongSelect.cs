using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    public Text songTitle;
    public string firstLevel;
    public int index = 0;

    public AudioSource[] songSources;
    public AudioSource previewMusic;
    public string[] songNames = {"Yellow - Coldplay", "Helicopter - Bloc Party"};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void nextSong(){
        previewMusic.Stop();

        index = (index + 1) % songSources.Length;

        GameManager.musicSelectedIndex = index;
        GameManager.musicSelectedName = songSources[index].name;


        songTitle.text = songNames[index];

        previewMusic = songSources[index];
        previewMusic.Play();
    }
    public void playSong(){
        SceneManager.LoadScene(firstLevel);
    }
}
