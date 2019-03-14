using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public Text theText;

    private Color original;

    public void Start()
    {
        original = theText.color;
    }

    public void OnSelect(BaseEventData eventData)
    {
        theText.color = new Color(255, 255, 255, 255); 
    }

    public void OnDeselect(BaseEventData eventData)
    { 
        theText.color = original;
    }
}