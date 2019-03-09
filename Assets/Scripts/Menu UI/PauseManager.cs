using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Image fade;
    private float _fadeDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        var levelsButton = transform.Find("Pause/Buttons/Levels").gameObject.GetComponent<Button>();
        levelsButton.onClick.AddListener(LoadGame);

        var continueButton = transform.Find("Pause/Buttons/Continue").gameObject.GetComponent<Button>();
        continueButton.onClick.AddListener(Unpause);

        var quitButton = transform.Find("Pause/Buttons/Quit").gameObject.GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);

        this.gameObject.SetActive(false);
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
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private static void LoadGame()
    {
        SceneManager.LoadScene(5);
    }

    private static void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}