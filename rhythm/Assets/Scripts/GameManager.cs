using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static string musicSelectedName = "helilonger2";
    public static int musicSelectedIndex = 0;

    public AudioSource[] songSources;

    public AudioSource theMusic;
    public AudioSource missSound;

    public float startTime;
    public float songDelayInSeconds;
    public BeatSpawner[] lanes;

    public float bT;

    public string fileLocation;
    public float noteTime;
    public float BPM;
    public float score;
    public float scorePerNote = 100;
    public int streak;

    private int hits;
    public static int[] scoreData = {0,0,0,0}; //fine, nice, excellent, miss
    private int accSum = 0;

    public static GameManager instance;
    public BeatSpawner bSpawner;
    public bool startPlaying;
    public bool playedSong;
    public MenuController pauseMenu;
    public KeyCode keyToPress;


    public BScroller theBS;

    public Text scoreText;
    public Text streakText;
    public Text timeText;
    public Text accuracyText;

    public static MidiFile midiFile;

    public GameObject resultScreen;
    public Text[] resultText;
    public RawImage imageRank;
    public Texture[] rankTextures = new Texture[6];

    public int[] hitScore = {50,200,300,0};    
    public string[] hitTexts = {"Fine", "Nice", "Excellent", "Miss"};

    public List<GameObject> hitObjects;
    List<GameObject> objs = new List<GameObject>();

    public string mainMenuScene;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        fileLocation = GameManager.musicSelectedName + ".mid";
        theMusic = songSources[GameManager.musicSelectedIndex];

        ReadFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying)
        {
            if(Input.GetKeyDown(keyToPress) && !playedSong){
                playedSong = true;
                resultScreen.SetActive(false);
                Time.timeScale = 1f;

                score = 0f;
                streak = 0;
                hits = 0;
                Array.Clear(scoreData, 0, scoreData.Length);

                scoreText.text = "Score: " + score;
                streakText.text = "" + streak;

                startPlaying = true;
                
                theMusic.transform.localScale = new Vector3(0,0,0);
                theMusic.time = GameManager.instance.theMusic.transform.localScale.x;

                theMusic.Play();
            }
        }
        else{

            if(theMusic.isPlaying || pauseMenu.isPaused) //either the music is playing or we are paused.
            {
                //timeText.text = "Time:" + GetAudioSourceTime() + "s\n";
            }
            else{ // song over, pop up the results
                startPlaying = false;

                theMusic.transform.localScale = new Vector3(0,0,0);
                theMusic.time = GameManager.instance.theMusic.transform.localScale.x;

                Time.timeScale = 1f;

                showResults();
            }
        }
        //timeText.text = "Time:" + GetAudioSourceTime() + "s\n";
    }
    ///////
    public void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach(var lane in lanes) 
        {
            lane.resetNotes();
            lane.SetTimeStamps(array);   
        }
    }
    //////
    public void NoteHit(float timeHit){
        int scoretype = 0;

        if(timeHit < 0.15f) scoretype  = 0;
        if(timeHit < 0.09f) scoretype = 1;
        if(timeHit < 0.05f) scoretype = 2;

        score += hitScore[scoretype] + Mathf.Ceil(streak * 5.5f);
        scoreData[scoretype] += 1;
        accSum += hitScore[scoretype];

        streak += 1;
        hits += 1;

        scoreText.text = "Score: " + score;
        streakText.text = ""+ streak;
        adjustAccuracy();

        spawnHitMessage(scoretype);
    }

    public void NoteMiss(){
        scoreData[3] += 1;

        missSound.Play();

        hits += 1;
        streak = 0;
        streakText.text = "" + streak;
        adjustAccuracy();

        spawnHitMessage(3);
    }

    public void adjustAccuracy()
    {
        float tempAcc = (float) accSum / (hits * 3f);
        accuracyText.text = "Accuracy: " + tempAcc.ToString("0.00") + "%";
    }

    public void spawnHitMessage(int scoretype)
    {
        var msg = Instantiate(hitObjects[scoretype], transform);
        objs.Add(msg);

        if(objs.Count > 1) // i want to destroy the gameObject? 
        {
            Destroy(objs[0]);
            objs.Remove(objs[0]);
        }
    }

    public void showResults()
    {
        resultScreen.SetActive(true);

        int sumScore = 0;
        int numElem = 0;
        string finalScore = score + "-";


        for(int i=0; i<scoreData.Length; i++)
        {
            resultText[i].text = "" + scoreData[i];
            sumScore += (scoreData[i] * hitScore[i]);
            numElem += scoreData[i];

            // begin saving data

            finalScore += scoreData[i].ToString() + ";";
        }

        float acc = (float)sumScore / (numElem * 3f); // (sum/(elem * 300) * 100)

        resultText[4].text = acc.ToString("0.00") + "%";
        resultText[5].text = "" + score;

        float[] R = {1f, 1f, 0f, 0f, 0.4902f, 0.7176f};
        float[] G = {0.8178f, 0.8178f, 0.7169f, 0.6370f, 0f, 0f};
        float[] B = {0f, 0f, 0.0044f, 0.7176f, 0.7176f, 0f};

        int indx = returnRank(acc);
        imageRank.texture = rankTextures[indx];
        imageRank.color = new Color(R[indx], G[indx], B[indx], 1f);

        // time to save the data...
        
        if(PlayerPrefs.HasKey(GameManager.musicSelectedName))
        {            
            string getScore = PlayerPrefs.GetString(GameManager.musicSelectedName);
            
            string[] splitWord = getScore.Split("-");
            int tempScore = int.Parse(splitWord[0]);

            Debug.Log("Comparing other score...\nPrevious highscore: " + tempScore);

            if(score > tempScore)
            {
                Debug.Log("New highscore for " + GameManager.musicSelectedName + ": " + finalScore);
                PlayerPrefs.SetString(GameManager.musicSelectedName, finalScore);
            }
        }else //no data
        {
            PlayerPrefs.SetString(GameManager.musicSelectedName, finalScore);
            Debug.Log("Logged score for " + GameManager.musicSelectedName + ": " + finalScore);
        }
        
    }

    public static int returnRank(float percentage) // {SS, S, A, B, C, D}
    {   
        if(scoreData[3] == 0)
        {
            if(percentage == 100) return 0;
            if(percentage > 95) return 1;
        }
        if(percentage > 90) return 2;
        if(percentage > 80) return 3;
        if(percentage > 70) return 4;
        return 5;
    }
    /////////
    public void replay()
    {
        playedSong = false;
        resultScreen.SetActive(false);
        streakText.text = "Press \"space\" to play";
    }
    public void returnToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    /////////
    public static bool playing()
    {
        return instance.theMusic.isPlaying;
    }
    public static double GetAudioSourceTime()
    {
        return (double)instance.theMusic.timeSamples / instance.theMusic.clip.frequency;
    }
}
