using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttoncontrol : MonoBehaviour
{

    public AudioSource hitSFX;
    public AudioSource missSFX;

    Animator anim;
    public KeyCode keytopress;
    public bool canpressnote = false;
    GameObject note;
    public GameObject hiteffect, goodeffect, perfecteffect, misseffect;

    public static buttoncontrol instance;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        instance = this;
    }


    void Update()
    {
        if (Input.GetKeyDown(keytopress))
        {
            OnMouseDown();
        }
        if (Input.GetKeyUp(keytopress))
        {
            OnMouseUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Note")
        {
            canpressnote = true;
            note = other.gameObject;
        }
            

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Note")
        {
            canpressnote = false;
            GameManager.instance.NoteMissed();
            
            Instantiate(misseffect, transform.position + new Vector3(0f, 32f, 0f), misseffect.transform.rotation);
            GameManager.instance.mt.SetActive(false);
            missSFX.Play();
        }
        
    }
    public void OnMouseDown()
    {anim.SetTrigger("On");
        if (canpressnote)
        {
            hitSFX.Play();
            
            

            canpressnote =false;


            note.SetActive(false);
            
            

            if (Mathf.Abs(note.transform.position.z) > 2.1874205)
            {
                //Debug.Log("normal");
                GameManager.instance.normalhit();
                Instantiate(hiteffect, transform.position + new Vector3(0f, 32f, 0f), hiteffect.transform.rotation);

            }
            else if (Mathf.Abs(note.transform.position.z) > 1)
            {
                //Debug.Log("good");
                GameManager.instance.goodhit();
                Instantiate(goodeffect, transform.position + new Vector3(0f, 32f, 0f), goodeffect.transform.rotation);
            }
            else
            {
                //Debug.Log("perfect");
                GameManager.instance.perfecthit();
                Instantiate(perfecteffect, transform.position + new Vector3(0f, 32f, 0f), perfecteffect.transform.rotation);

            }

        }
    }

    public void OnMouseUp()
    {
        anim.SetTrigger("Off");
    }
}
