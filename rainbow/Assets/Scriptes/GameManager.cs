using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Numerics;


public class GameManager : MonoBehaviour
{
    #region game manager variables

    public AudioSource music;

    public bool startplaying;

 

    public static GameManager instance;

    public int currentscore;
    public int scorepernote = 100;
    public int scorepergoodnote = 125;
    public int scoreperperfectnote = 150;
    
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThreshholds;
    
    public Text scoreText;
    public Text multiText;

    public GameObject mt;//multiplier text

    
    public float normalhits;
    public float goodhits;
    public float missedhits;
    public float perfecthits;

    public GameObject resultsscreen;
    public Text percenthittext, normalstext, goodstext, perfectstext, missestext, ranktext, finaltext;

    #endregion
    
    #region song manager variables



    public Lane[] lanes;
    public float songDelayInSeconds = 0;
    public float noteDelayInSeconds = 2;

    //public int inputDelayInMilliseconds;


    public string fileLocation;
    public float noteTime = 5;
    public float noteSpawnZ=150 ;
    public float noteTapZ;
    public float noteDespawnZ
    {
        get
        {
            return noteTapZ - (noteSpawnZ - noteTapZ);
        }
    }

    public static MidiFile midiFile;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "0";
        currentMultiplier = 1 ;
        ReadFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!startplaying)
        {
            //Debug.Log("not started");
            if (Input.anyKeyDown)
            {
                //Debug.Log("started");
                startplaying = true;
                music.Play();

            }

        }
        
        
        else
        {
            
            if (!music.isPlaying && !resultsscreen.activeInHierarchy)
            {
                resultsscreen.SetActive(true);

                normalstext.text = normalhits.ToString();
                goodstext.text = goodhits.ToString();
                perfectstext.text = perfecthits.ToString();
                missestext.text = missedhits.ToString();

                float totalnotes=0;
                foreach (Lane lane in lanes)
                {
                    totalnotes += lane.spawnIndex;
                }

                float totalhits = normalhits + goodhits + perfecthits;
                float percenthit = totalhits / totalnotes * 100f;
                percenthittext.text = percenthit.ToString("F1") + "%";

                string rankval = "F";
                if (percenthit > 40)
                {
                    rankval = "D";
                    if (percenthit > 55 )
                    {
                        rankval = "C";
                        if (percenthit > 70)
                        {
                            rankval = "B";
                            if (percenthit > 85)
                            {
                                rankval = "A";
                                if (percenthit > 95)
                                    rankval = "S";
                            }
                        }
                    }
                }
                ranktext.text = rankval;
                finaltext.text = currentscore.ToString();
            }
             
        }
        
    }
    #region score manager functions
    public void NoteHit()
    {   
        if (currentMultiplier -1 < multiplierThreshholds.Length)
        {
            multiplierTracker++;

            if (multiplierThreshholds[currentMultiplier-1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
        
        mt.SetActive(true);//multiplier text active
        multiText.text = "X" + currentMultiplier;

        scoreText.text = currentscore.ToString();
    }

    public void normalhit()
    {
        currentscore += scorepernote * currentMultiplier;
        NoteHit();

        normalhits++;
    }

    public void goodhit()
    {
        currentscore += scorepergoodnote * currentMultiplier;
        NoteHit();

        goodhits++;
    }

    public void perfecthit()
    {
        currentscore += scoreperperfectnote * currentMultiplier;
        NoteHit();

        perfecthits++;
    }

    
    public void NoteMissed()
    { 
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "X" + currentMultiplier;
        
        missedhits++;
    }
    #endregion

    #region song manager

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);

        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) 
            lane.SetTimeStamps(array);
         
    }

    
    
    public static double GetAudioSourceTime()
    {
        
        return (double)instance.music.timeSamples / instance.music.clip.frequency;
    }

    #endregion
}
