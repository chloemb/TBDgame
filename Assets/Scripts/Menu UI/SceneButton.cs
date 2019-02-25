using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public Image fade;

    private float _fadeDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        var scene2Button = transform.Find("Buttons/Scene 2").gameObject.GetComponent<Button>();
        scene2Button.onClick.AddListener(() => StartCoroutine(StartFade(2)));

        var scene3Button = transform.Find("Buttons/Scene 3").gameObject.GetComponent<Button>();
        scene3Button.onClick.AddListener(() => StartCoroutine(StartFade(3)));
    }

    private IEnumerator StartFade(int stage)
    {
        var color = Color.black;
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= .5f * Time.deltaTime;

            color.a = 1f - _fadeDelay;
            fade.color = color;
            yield return null;
        }

        LoadGame(stage);
    }

    private static void LoadGame(int stage)
    {
        SceneManager.LoadScene(stage);
    }
}
