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

        var sunnyland1Button = transform.Find("Buttons/Sunnyland 1").gameObject.GetComponent<Button>();
        sunnyland1Button.onClick.AddListener(() => StartCoroutine(StartFade(6)));

        var sunnyland2Button = transform.Find("Buttons/Sunnyland 2").gameObject.GetComponent<Button>();
        sunnyland2Button.onClick.AddListener(() => StartCoroutine(StartFade(7)));
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
