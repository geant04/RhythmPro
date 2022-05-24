using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorLoop : MonoBehaviour
{
    private float[] colors = {0.0f,0.0f,1.0f};
    private int indx = 0;
    private float multi = 1;
    public bool stat;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!stat) 
        {
            if( (multi == 1 && colors[indx] < 1) || (multi == -1 && colors[indx] > 0))
            {
                colors[indx] += 0.5f * multi * Time.deltaTime;
            } else
            {
                indx = (indx + 1) % 3;
                multi *= -1;
            }
            gameObject.GetComponent<SpriteRenderer>().color =  new Color(colors[0], colors[1], colors[2], 0.60f);
        }
    }
}
