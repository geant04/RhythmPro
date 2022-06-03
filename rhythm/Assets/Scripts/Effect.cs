using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float timetoGo = 1f;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 3.5f, 0);
        transform.localScale = new Vector3(0,0,0);

        if(type == 1)
        {
            LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0), 0.1f);
        }else{
            LeanTween.scale(gameObject, new Vector3(0.24f, 0.24f, 0), 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timetoGo);
    }
}
