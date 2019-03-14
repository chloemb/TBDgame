using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    public Image fade;

    private float _fadeDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        var startButton = transform.Find("Buttons/Back").gameObject.GetComponent<Button>();
        startButton.onClick.AddListener(() => StartCoroutine(StartFade(0)));
    }

    private IEnumerator StartFade(int sceneNum)
    {
        var color = Color.black;
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= .5f * Time.deltaTime;

            color.a = 1f - _fadeDelay;
            fade.color = color;
            yield return null;
        }

        StartGame(sceneNum);
    }

    public void StartGame(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

}
