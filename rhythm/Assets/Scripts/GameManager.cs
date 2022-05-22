using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

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

    private int hits = 0;
    private int misses = 0;

    public static GameManager instance;
    public BeatSpawner bSpawner;
    public bool startPlaying;
    public MenuController pauseMenu;
    public KeyCode keyToPress;


    public BScroller theBS;

    public Text scoreText;
    public Text streakText;
    public Text timeText;

    public static MidiFile midiFile;

    public GameObject resultScreen;
    public Text pHitValue, mHitValue, percHit, rank, fScore;

    public int[] hitScore = {50,100,300};
    public string[] hitTexts = {"Fine", "Nice", "Excellent", "Miss"};

    public List<GameObject> hitObjects;

    List<GameObject> objs = new List<GameObject>();

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
            if(Input.GetKeyDown(keyToPress)){
                resultScreen.SetActive(false);
                Time.timeScale = 1f;

                score = 0f;
                streak = 0;
                hits = 0;
                misses = 0;

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
        timeText.text = "Time:" + GetAudioSourceTime() + "s\n";
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
        streak += 1;
        hits += 1;

        scoreText.text = "Score: " + score;
        streakText.text = ""+ streak;

        spawnHitMessage(scoretype);

    }
    public void NoteMiss(){
        misses += 1;

        missSound.Play();

        streak = 0;
        streakText.text = "" + streak;

        spawnHitMessage(3);
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

        pHitValue.text = "" + hits;
        mHitValue.text = "" + misses;

        percHit.text = Mathf.Ceil((float)hits/(float)(hits+misses) * 100f) + "%";
        
        rank.text = returnRank(Mathf.Ceil((float)hits/(float)(hits+misses) * 100f));
        fScore.text = "" + score;
    }
    public static string returnRank(float percentage)
    {
        if(percentage == 100) return "SS";
        if(percentage > 95) return "S";
        if(percentage > 90) return "A";
        if(percentage > 80) return "B";
        if(percentage > 70) return "C";
        return "D";
    }
    public static bool playing()
    {
        return instance.theMusic.isPlaying;
    }
    public static double GetAudioSourceTime()
    {
        return (double)instance.theMusic.timeSamples / instance.theMusic.clip.frequency;
    }
}
