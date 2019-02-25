using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    public int KillsRequired;
    private int _killsHappened;
    // Start is called before the first frame update
    void Start()
    {
        _killsHappened = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_killsHappened >= KillsRequired)
        {
            //End to quit game screen
            SceneManager.LoadScene(4);
        }
        
    }

    void Killed()
    {
        _killsHappened++;
    }
}
