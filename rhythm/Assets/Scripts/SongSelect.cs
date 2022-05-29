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
    public string[] songNames = {"Yellow - Coldplay", "Helicopter - Bloc Party", "Flower Dance - DJ Okawari"};

    public RawImage sample;
    public Texture[] textures = new Texture[3];
    
    public RawImage imageRank;
    public Texture[] rankTextures = new Texture[6];

    public Text accuracy;
    public Text displayScore;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.musicSelectedIndex = index;
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
            
            Debug.Log("Obtained: " + getScore);

            string[] splitWord = getScore.Split("-");
            displayScore.text = splitWord[0];


            string noteData = splitWord[1];
            string[] dataString = noteData.Split(";");
            
            int sumScore = 0;
            int numElem = 0;
            int[] scoreValues = {50,200,300,0};

            Debug.Log("Test");

            for(int i=0; i<4; i++){
                sumScore += ( int.Parse(dataString[i])  * scoreValues[i] );
                numElem += int.Parse(dataString[i]);
            }

            float acc = (float)sumScore / (numElem * 3f); // (sum/(elem * 300) * 100)
            accuracy.text = acc.ToString("0.00") + "%";
            
            float[] R = {1f, 1f, 0f, 0f, 0.4902f, 0.7176f};
            float[] G = {0.8178f, 0.8178f, 0.7169f, 0.6370f, 0f, 0f};
            float[] B = {0f, 0f, 0.0044f, 0.7176f, 0.7176f, 0f};

            int indx = GameManager.returnRank(acc);
            imageRank.texture = rankTextures[indx];
            imageRank.color = new Color(R[indx], G[indx], B[indx], 1f);
        }else // no song detected
        {
            imageRank.enabled = false;
            accuracy.text = "";
            displayScore.text = "";
        }
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
    public void playSong(){
        
        SceneManager.LoadScene(firstLevel);
    }
}
