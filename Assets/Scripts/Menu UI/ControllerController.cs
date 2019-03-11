using System.Collections;
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

}
