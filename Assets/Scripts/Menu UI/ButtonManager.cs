using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public Image fade;

    private float _fadeDelay = 1f;

    public void Start()
    {
        var startButton = transform.Find("Buttons/Start").gameObject.GetComponent<Button>();
        startButton.onClick.AddListener(() => StartCoroutine(StartFade()));

        var quitButton = transform.Find("Buttons/Quit").gameObject.GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);
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

        StartGame();
    }

    private static void StartGame()
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
