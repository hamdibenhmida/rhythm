using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public double timeInstantiated;
    public static Note instance;
    
    void Start()
    {

        instance = this;
        timeInstantiated = GameManager.GetAudioSourceTime();
        
    }

    // Update is called once per frame
    void Update()
    {

        double timeSinceInstantiated = GameManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (GameManager.instance.noteTime *2));

        if (t > 1 )
        {
             gameObject.SetActive(false);
            timeInstantiated = GameManager.GetAudioSourceTime();
            t = 0;

        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * GameManager.instance.noteSpawnZ, Vector3.forward * GameManager.instance.noteDespawnZ, t);
            //GetComponent<SpriteRenderer>().enabled = true;
        }
                              
    }

}