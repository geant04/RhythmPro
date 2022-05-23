using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float timetoGo = 1f;
    private float max = 0.23f;
    private float sc = 0.0f;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 3.5f, 0);
        transform.localScale = new Vector3(0,0,0);

        if(type == 1)
        {
            max = 0.34f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transformFunction() < max)
        {
            float newV = transformFunction();
            transform.localScale = new Vector3(newV, newV, 0);
            sc += 0.01f; 
        }
        
        Destroy(gameObject, timetoGo);
    }

    float transformFunction()
    {
        if(type == 1) return -1*(sc-0.6f)*(sc-0.6f) + 0.36f;
        return -1f * (sc-0.4895f)*(sc-0.4895f) + 0.24f;
    }
}
