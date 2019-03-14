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
    private PlayerController _pc1;
    private PlayerController _pc2;

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

        _pc1 = Player1.GetComponent<PlayerController>();
        _pc2 = Player2.GetComponent<PlayerController>();
        
        _pc1.Paused = false;
        _pc2.Paused = false;
        Pause.SetActive(false);
        _pc1.DisableControl();
        _pc2.DisableControl();
        
        Time.timeScale = 1f;
        
        Invoke("GiveBackControls", .15f);
    }

    private void GiveBackControls()
    {
        _pc1.GiveBackControl();
        _pc2.GiveBackControl();
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