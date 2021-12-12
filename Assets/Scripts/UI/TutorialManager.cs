using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject[] popUps;
    public GameObject endscreen;
    public int popUpIndex;
    public PlayerController player;
    public float waittime = 1f;
    public float pausetime = 1f;
    public GameMenuManager menuManager;
    public bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //show the current popup, hide all others
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i==popUpIndex)
                popUps[i].SetActive(true);
            else
                popUps[i].SetActive(false);

        }

        if (popUpIndex==0)
        {
            //wait for move input
            if (player.horizontal !=0 || player.vertical!=0)
                popUpIndex+=1;
        }
        if (popUpIndex==1)
        {
            //wait for shooting input
            if (Input.GetButton("Fire1") )
                popUpIndex += 1;
        }
        if (popUpIndex == 2)
        {
            //wait some time and check if they have all their quarks now
            if (pausetime <= 0)
            {
                if (GameManager.instance.player.GetComponent<PlayerAmmo>().shots == 3)
                    popUpIndex += 1;
            }
            else
                pausetime -= Time.deltaTime;
        }
        if (popUpIndex == 3)
        {
            //wait for them to dash
            if (Input.GetButton("Jump"))
                popUpIndex += 1;
        }
        if (popUpIndex == 4)
        {
            //after some waiting they have finished - show the finished screen
            if (waittime <= 0 && !finished)
            {
                finished = true;
                menuManager.ShowWinScreen();
            }
            else
            {
                waittime -= Time.deltaTime;
            }
        }
    }
}
