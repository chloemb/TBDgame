using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, ISelectHandler
{
    public Sprite border;
    private float _fadeDelay;

    void Start()
    {
        _fadeDelay = .1f;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine("FadeIn");

    }

    private IEnumerator FadeIn()
    {
        var color = Color.white;
        //var spRenderer = border.
        while (_fadeDelay > 0f)
        {
            _fadeDelay -= Time.deltaTime;

            color.a = 1f - _fadeDelay;
        //    border.color = color;
            yield return null;
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("Deselected");
    }
}
