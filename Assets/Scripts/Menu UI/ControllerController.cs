﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ControllerController : MonoBehaviour
{
    private bool hasBeenCalled;
    private PlatformType _platform;
    private GameObject Players;

    // Start is called before the first frame update
    void Start()
    {
        _platform = Platform.GetPlatform();
        hasBeenCalled = GlobalControl.Instance.seenControls;
        if (hasBeenCalled)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GlobalControl.Instance.seenControls = true;
            Time.timeScale = 0f;
            GlobalControl.Instance.GetComponent<AudioSource>().Stop();
        }
            
    }

    void Update()
    {
        if (_platform.ToString() == "Windows")
        {
            if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 1 button 7"))
            {
            }
        } else if (_platform.ToString() == "Mac")
        {
            if (Input.GetKeyDown("joystick 1 button 16") || Input.GetKeyDown("joystick 1 button 9"))
            {
            }
        }

    }
}