using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    private PlatformType _platform;

    private StandaloneInputModule _saim;

    // Start is called before the first frame update
    void Start()
    {
        _platform = Platform.GetPlatform();
        _saim = GetComponent<StandaloneInputModule>();
        if (_platform.ToString() == "Windows")
        {
            _saim.horizontalAxis = "P1LHorizontal_Windows";
            _saim.verticalAxis = "P1LVertical_Windows";
            _saim.submitButton = "P1Jump_Windows";
            _saim.cancelButton = "P1Dash_Windows";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
