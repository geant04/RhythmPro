using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    private string[] keys = {"A","S","J","K"};
    private string[] noteRestrictions = {"F4", "G4", "A4","B4"};

    public MenuController pause;
    public GameObject hitFX;
    public GameObject preFabs;
    List<Note> notes = new List<Note>();
    List<GameObject> noteObjects = new List<GameObject>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int hitIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void resetNotes()
    {
        notes = new List<Note>();
        timeStamps = new List<double>();
        noteObjects = new List<GameObject>();
    }


    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach(var note in array)
        {
            if(note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, GameManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            } //ripped off that dude from youtube thx
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count && !(pause.isPaused))
        {
            if (GameManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - GameManager.instance.noteTime)
            {
                var note = Instantiate(preFabs, transform);
                notes.Add(note.GetComponent<Note>());
                noteObjects.Add(note);
                note.GetComponent<Note>().timing = (float)timeStamps[spawnIndex];
                
                Debug.Log("spawned at time: " + timeStamps[spawnIndex] + " for audiosource time: " + GameManager.GetAudioSourceTime());

                spawnIndex++;
            }
        }
        
        if(hitIndex < timeStamps.Count){
            if(Input.GetKeyDown(input))
            {
                if( Mathf.Abs((float)timeStamps[hitIndex] - (float)GameManager.GetAudioSourceTime()) < 0.15f )
                {
                    //print("hit at: " + Mathf.Abs(timing-(float)GameManager.GetAudioSourceTime()));
                    GameManager.instance.NoteHit(Mathf.Abs( (float)timeStamps[hitIndex] - (float)GameManager.GetAudioSourceTime() ) );
                    Destroy(noteObjects[hitIndex]);
                    hitIndex++;
                }
                var fx = Instantiate(hitFX, transform);
            }
        }
        if(hitIndex < timeStamps.Count)
        {
            if((float)GameManager.GetAudioSourceTime() > (float)timeStamps[hitIndex] + 0.15f)
            {
                print("missed");
                GameManager.instance.NoteMiss();
                Destroy(noteObjects[hitIndex]);
                hitIndex++;
            }
        }
        if(!GameManager.playing() && (spawnIndex >= timeStamps.Count)){
            noteObjects = new List<GameObject>();
            spawnIndex = 0;
            hitIndex = 0;
        }
    }
}
