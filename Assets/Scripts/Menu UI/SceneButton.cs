using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class SceneButton : MonoBehaviour
{
    public Image fade;

    private float _fadeDelay = 1f;

    private int RandNum;

    // Start is called before the first frame update
    void Start()
    {
        RandNum = Random.Range(1, 7);
        if (RandNum == 3) //make it so numbers line up correctly with actual stages
        {
            RandNum = 7;
        }
        else if (RandNum == 4)
        {
            RandNum = 8;
        }
        
        var scene2Button = transform.Find("Buttons/Scene 2").gameObject.GetComponent<Button>();
        scene2Button.onClick.AddListener(() => StartCoroutine(StartFade(1)));

        var scene3Button = transform.Find("Buttons/Scene 3").gameObject.GetComponent<Button>();
        scene3Button.onClick.AddListener(() => StartCoroutine(StartFade(2)));

        var sunnyland1Button = transform.Find("Buttons/Sunnyland 1").gameObject.GetComponent<Button>();
        sunnyland1Button.onClick.AddListener(() => StartCoroutine(StartFade(5)));

        var sunnyland2Button = transform.Find("Buttons/Sunnyland 2").gameObject.GetComponent<Button>();
        sunnyland2Button.onClick.AddListener(() => StartCoroutine(StartFade(6)));

        var boxlevelButton = transform.Find("Buttons/Box Level").gameObject.GetComponent<Button>();
        boxlevelButton.onClick.AddListener(() => StartCoroutine(StartFade(7)));

        var blocklevelButton = transform.Find("Buttons/Block Level").gameObject.GetComponent<Button>();
        blocklevelButton.onClick.AddListener(() => StartCoroutine(StartFade(8)));

        var randomButton = transform.Find("Buttons/Random").gameObject.GetComponent<Button>();
        randomButton.onClick.AddListener(() => StartCoroutine(StartFade(RandNum)));

        var backButton = transform.Find("Buttons/Back").gameObject.GetComponent<Button>();
        backButton.onClick.AddListener(() => StartCoroutine(StartFade(0)));
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
