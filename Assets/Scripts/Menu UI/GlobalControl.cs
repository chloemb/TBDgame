using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    
    public AudioSource Music;

    public bool CurrentlyFading;

    public string winner;

    public string PrevScene;

    public bool seenControls = false;
    
    void Awake()
    {
        Music = GetComponent<AudioSource>();
        Music.volume = 1f;
        Music.Play();
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level Select")
            Time.timeScale = 1;
            if (SceneManager.GetActiveScene().name != PrevScene)
            {
                IEnumerator Fade = FadeOutMusic();
                // GetComponent<AudioSource>().volume = 0;
                if (SceneManager.GetActiveScene().name == "Level Select" ||
                    SceneManager.GetActiveScene().name == "End Game")
                {
                    Time.timeScale = 1f;
                    if (!CurrentlyFading)
                    {
                        StartCoroutine(Fade);
                    }
                }
                else
                {
                    StopCoroutine(Fade);
                }
               
            }

//        if (SceneManager.GetActiveScene().name != "Level Select" && GetComponent<AudioSource>().isPlaying)
//            GetComponent<AudioSource>().volume = 1f;

        PrevScene = SceneManager.GetActiveScene().name;
    }

    private IEnumerator FadeOutMusic()
    {
        //Debug.Log("fading");
        CurrentlyFading = true;
        float StartVolume = Music.volume;
        
        //Debug.Log(Music.volume);

        while (Music.volume > 0)
        {
            //Debug.Log("volume is " + Music.volume);
            //Debug.Log("decreasing volume by " + StartVolume * (Time.deltaTime / 3));
            //Debug.Log(Time.deltaTime);
            Music.volume -= StartVolume * (Time.deltaTime / 1.5f);
            yield return null;
        }

        Music.Stop();
        Music.volume = StartVolume;
        CurrentlyFading = false;
    }
}