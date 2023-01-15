using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Numerics;
using NAudio.Midi;
using System.Linq;

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



    // Path to the MIDI file
    public string filePath = "Crazy-Frog.mid";

    // BPM (tempo) of the track
    int bpm = 120;

    // List to store the extracted note information
    public List<float> notes = new List<float>();

    public GameObject noteprefab;



    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Load the MIDI file
        var midi = new MidiFile(filePath);
        // Get the ticks per quarter note (needed for timing calculations)
        int ticksPerQuarterNote = midi.DeltaTicksPerQuarterNote;
        // Iterate through the events in the first track
        foreach (var evt in midi.Events[0])
        {
            // Look for note on events
            if (evt.CommandCode == MidiCommandCode.NoteOn)
            {
                var noteOn = (NoteOnEvent)evt;
                // Calculate the time at which the note starts (in seconds)
                float time = (60f * noteOn.AbsoluteTime) / (bpm * ticksPerQuarterNote);
                // Get the note name (A, B, C, etc.)
                
                // Add the note name and time to the list
                notes.Add((time));
            }
        }



        instance = this;
        scoreText.text = "0";
        currentMultiplier = 1 ;
        
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
                //foreach (Lane lane in lanes)
                //{
                //    totalnotes += lane.spawnIndex;
                //}

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
        foreach (var note in notes)
        {
            if (music.time == note)
                Instantiate(noteprefab);
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

  
    #endregion

}
