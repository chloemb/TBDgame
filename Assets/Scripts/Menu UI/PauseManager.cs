using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Image fade;
    private float _fadeDelay = 1f;
    private GameObject Pause;
    private GameObject Player1;
    private GameObject Player2;

    // Start is called before the first frame update
    void Start()
    {
        Pause = transform.Find("Pause").gameObject;

        var levelsButton = transform.Find("Pause/Buttons/Levels").gameObject.GetComponent<Button>();
        levelsButton.onClick.AddListener(LoadGame);

        var continueButton = transform.Find("Pause/Buttons/Continue").gameObject.GetComponent<Button>();
        continueButton.onClick.AddListener(Unpause);

        var quitButton = transform.Find("Pause/Buttons/Quit").gameObject.GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);

        Pause.SetActive(false);
    }

    private IEnumerator StartFade()
    {
        var color = Color.black;
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= .5f * Time.deltaTime;

            color.a = 1f - _fadeDelay;
            fade.color = color;
            yield return null;
        }

        LoadGame();
    }

    private void Unpause()
    {
        Player1 = GameObject.Find("Player 1");
        Player2 = GameObject.Find("Player 2");
        
        Player1.GetComponent<PlayerController>().Paused = false;
        Player2.GetComponent<PlayerController>().Paused = false;
        Pause.SetActive(false);
        Time.timeScale = 1f;
    }

    private static void LoadGame()
    {
        SceneManager.LoadScene(4);
    }

    private static void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}