using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContinueButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button myButton;
    public Text theText;

    void OnEnable()
    {
        myButton = this.GetComponent<Button>();
        // Select the button
        myButton.Select(); // Or EventSystem.current.SetSelectedGameObject(myButton.gameObject)
        // Highlight the button
        myButton.OnSelect(null); // Or myButton.OnSelect(new BaseEventData(EventSystem.current))
        theText.color = new Color(255, 255, 255, 255);
    }

    public void OnSelect(BaseEventData eventData)
    {
        theText.color = new Color(255, 255, 255, 255);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        theText.color = new Color32(18, 255, 0, 255);
    }
}
