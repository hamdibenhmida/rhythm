using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class Lane : MonoBehaviour
{
   

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    
    public List<double> timeStamps = new List<double>();//list ta3 wa9teh tespawni kol note

    public int spawnIndex = 0;

    

    public List <GameObject> pool = new List<GameObject>();
    public int amouttopool;

    public static Lane Instance;

   
    

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amouttopool; i++)
        {
           GameObject note = Instantiate(notePrefab,transform);
            note.SetActive(false);
            pool.Add(note);
        }
        Instance = this;    
    }


    #region  set notes time in array in each lane
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, GameManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f +GameManager.instance.noteDelayInSeconds );
            }
        }
    }
    #endregion


    // Update is called once per frame
    void Update()
    {    
        if (spawnIndex < timeStamps.Count)
        {
            if (GameManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - GameManager.instance.noteTime)
            {
                var note = getnote();            
                
                //note.transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.instance.noteSpawnZ);    
                note.SetActive(true);
                spawnIndex++;
            }
        }
    }
  
  public GameObject getnote ()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy )
            {
                return pool[i];
            }
        }
        GameObject note = Instantiate(notePrefab, transform);
        note.SetActive(false);
        pool.Add(note);
        return note;
    }
}
    
