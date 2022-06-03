using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float color = 1f;
    public float sc = 0.0f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x,2f,0);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.67f, 0.97f, 1f, 0.6f);
        Color toColor = new Color(0.67f, 0.97f, 1f, 0f);

        LeanTween.color(gameObject, toColor, 0.2f);

    }
 
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 0.6f);
    }
  
}
