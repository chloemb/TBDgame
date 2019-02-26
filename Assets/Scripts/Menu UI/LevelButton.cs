using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image border;
    private float _fadeDelay;

    void Start()
    {
        _fadeDelay = 1f;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine("FadeIn");

    }

    private IEnumerator FadeIn()
    {
        StopCoroutine("FadeOut");
        var color = Color.white;
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= 5 * Time.deltaTime;

            color.a = 1f - _fadeDelay;
            border.color = color;
            yield return null;
        }

        _fadeDelay = 1f;
    }

    public void OnDeselect(BaseEventData data)
    {
        StartCoroutine("FadeOut");
    }

    private IEnumerator FadeOut()
    {
        StopCoroutine("FadeIn");
        var color = Color.white;
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= 5 * Time.deltaTime;

            color.a = _fadeDelay;
            border.color = color;
            yield return null;
        }

        _fadeDelay = 1f;
    }
}
