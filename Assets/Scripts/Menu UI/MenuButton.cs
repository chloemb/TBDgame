using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text theText;

    private Color original;

    public void Start()
    {
        original = theText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = new Color(255, 255, 255, 255); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = original;
    }
}