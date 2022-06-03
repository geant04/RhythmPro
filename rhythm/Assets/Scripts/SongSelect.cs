using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    public Text songTitle;
    public string firstLevel;
    public string menu;
    public int index = 0;

    public AudioSource[] songSources;
    public AudioSource previewMusic;
    public string[] songNames;

    public RawImage sample;
    public Texture[] textures;
    
    public RawImage imageRank;
    public Texture[] rankTextures;

    public Text accuracy;
    public Text displayScore;

    // Start is called before the first frame update
    void Start()
    {
        index = GameManager.musicSelectedIndex; // switcheroo

        GameManager.musicSelectedName = songSources[index].name;
        
        sample.texture = textures[index];

        songTitle.text = songNames[index];

        previewMusic = songSources[index];
        previewMusic.Play();
        previewMusic.time = 1.2f;

        showSongInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showSongInfo()
    {
        if(PlayerPrefs.HasKey(GameManager.musicSelectedName))
        {
            imageRank.enabled = true;

            string getScore = PlayerPrefs.GetString(GameManager.musicSelectedName);
            
            Debug.Log("Obtained: " + getScore + "for map name: " + GameManager.musicSelectedName);

            string[] splitWord = getScore.Split("-");
            displayScore.text = splitWord[0];


            string noteData = splitWord[1];
            string[] dataString = noteData.Split(";");
            
            int sumScore = 0;
            int numElem = 0;
            int[] scoreValues = {50,200,300,0};

            for(int i=0; i<4; i++){
                sumScore += ( int.Parse(dataString[i])  * scoreValues[i] );
                numElem += int.Parse(dataString[i]);
            }
            Debug.Log("Passed calculations");

            float acc = (float)sumScore / (numElem * 3f); // (sum/(elem * 300) * 100)
            Debug.Log("Calculated accuracy: " + acc);

            accuracy.text = acc.ToString("0.00") + "%";
            
            float[] R = {1f, 1f, 0f, 0f, 0.4902f, 0.7176f};
            float[] G = {0.8178f, 0.8178f, 0.7169f, 0.6370f, 0f, 0f};
            float[] B = {0f, 0f, 0.0044f, 0.7176f, 0.7176f, 0f};

            int indx = rrank(acc, int.Parse(dataString[3]));

            imageRank.texture = rankTextures[indx];
            imageRank.color = new Color(R[indx], G[indx], B[indx], 1f);

            Debug.Log("Obtained indx: " + indx);
        }else // no song detected
        {
            imageRank.enabled = false;
            accuracy.text = "";
            displayScore.text = "";
        }
    }
    public int rrank(float p, int m)
    {
        if(m == 0)
        {
            if(p == 100) return 0;
            if(p > 95) return 1;
        }
        if(p > 90) return 2;
        if(p > 80) return 3;
        if(p > 70) return 4;
        return 5;
    }
    public void nextSong(){
        previewMusic.Stop();

        index = (index + 1) % songSources.Length;

        GameManager.musicSelectedIndex = index;
        GameManager.musicSelectedName = songSources[index].name;

        sample.texture = textures[index];


        songTitle.text = songNames[index];

        previewMusic = songSources[index];
        previewMusic.Play();

        float t = 1.2f;
        if(index == 1)
        {
            t = 2.5f;
        }
        previewMusic.time = t;

        showSongInfo();
    }
    public void returnMenu()
    {
        SceneManager.LoadScene(menu);
    }
    public void playSong(){
        
        SceneManager.LoadScene(firstLevel);
    }
}
