using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float timetoGo = 1f;
    private float max = 0.23f;
    private float sc = 0.1f;
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
        if(transform.localScale.x <= max)
        {
            transform.localScale = new Vector3(sc * Time.deltaTime, sc * Time.deltaTime, 0);
            sc *= 1.3f; 
        }
        
        Destroy(gameObject, timetoGo);
    }
}
