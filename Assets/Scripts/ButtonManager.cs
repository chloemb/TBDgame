using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void onClick()
    {
        SceneManager.LoadScene("Environment Experiments");
    }
}
