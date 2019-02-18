using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{

    private HealthManager PlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(PlayerHealth.Health);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (PlayerHealth.Health == 0  && SceneManager.GetActiveScene().name == "Environment Experiments")
        {
            Debug.Log("halp");
            //Load next scene on death
            SceneManager.LoadScene(2);
        }
        else if (PlayerHealth.Health == 0 && SceneManager.GetActiveScene().name == "Scene 2")
        {
            //Quit Game if a player loses
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
