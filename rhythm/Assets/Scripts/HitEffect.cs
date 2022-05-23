using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float color = 1f;
    public float sc = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x,2f,0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0.67f,0.97f,1f, returnColor() );

        Destroy(gameObject, 0.6f);
    }

    float returnColor()
    {
        if(gameObject.GetComponent<SpriteRenderer> ().material.color.a > 0) 
        {
            sc += 0.01f;
            return -1f * (1f*sc-0.6f) * (1f*sc-0.6f) + 0.5f;
        }
        return 0;
    }
}
