using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class Lane : MonoBehaviour
{
    
    public GameObject notePrefab;

    public List<double> timeStamps = new List<double>();

    public int spawnIndex = 0;

    

    public static Lane Instance;

    void Start()
    {
        
        Instance = this;
    }

    
}
