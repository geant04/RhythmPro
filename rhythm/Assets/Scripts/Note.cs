using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool canBePressed;

    public KeyCode keyToPress;

    public float timing;
    public float currentTime;
    // Start is called before the first frame update

    void Start()
    {
        transform.position = new Vector3(transform.position.x, (float)GameManager.instance.noteTime * (GameManager.instance.BPM/60f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(keyToPress)){
            
            if( Mathf.Abs(timing - (float)GameManager.GetAudioSourceTime()) < 0.15f )
            {
                //print("hit at: " + Mathf.Abs(timing-(float)GameManager.GetAudioSourceTime()));
                GameManager.instance.NoteHit(Mathf.Abs(timing - (float)GameManager.GetAudioSourceTime()));
                Destroy(gameObject);
            }

        }
        if(GameManager.instance.startPlaying){

            if((float)GameManager.GetAudioSourceTime() > timing + 0.15f)
            {
                print("missed");
                GameManager.instance.NoteMiss();
                Destroy(gameObject);
            }

        }
        */
        transform.position -= new Vector3(0f, (float)(GameManager.instance.BPM/60f) * Time.deltaTime, 0);   
    }

}
