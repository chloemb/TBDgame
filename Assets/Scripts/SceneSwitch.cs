﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneSwitch : MonoBehaviour
{
    public int KillsRequired;
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;
    private int _kills1Happened;
    private int _kills2Happened;

    private AudioSource Music;
    public AudioClip StageTheme;

    // Start is called before the first frame update
    void Start()
    {
        Music = GameObject.Find("GlobalControl").GetComponent<AudioSource>();
        Music.volume = 1f;
        Music.clip = StageTheme;
        Music.Play();
        _kills1Happened = KillsRequired;
        _kills2Happened = KillsRequired;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Score1.text = _kills1Happened.ToString();
        Score2.text = _kills2Happened.ToString();
        if (_kills1Happened <= 0)
        {
            //End to quit game screen
            SceneManager.LoadScene(3);
            SaveData("Player2");
        }
        else if (_kills2Happened <= 0)
        {
            SceneManager.LoadScene(3);
            SaveData("Player1");
        }
    }

    void Player1Killed()
    {
        _kills1Happened--;
    }

    void Player2Killed()
    {
        _kills2Happened--;
    }

    void SaveData(string player)
    {
        GlobalControl.Instance.winner = player;
    }

}
