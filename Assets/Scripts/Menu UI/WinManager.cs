using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class WinManager : MonoBehaviour
{

    public TextMeshProUGUI Player1Text;
    public TextMeshProUGUI Player2Text;

    public void Start()
    {
        switch (GlobalControl.Instance.winner)
        {
            case "Player1":
                Player1Wins();
                break;
            case "Player2":
                Player2Wins();
                break;
        }
    }

    void Player1Wins()
    {
        Player1Text.color = new Color(Player1Text.color.r, Player1Text.color.g, Player1Text.color.b, 255);
    }

    void Player2Wins()
    {
        Player2Text.color = new Color(Player2Text.color.r, Player2Text.color.g, Player2Text.color.b, 255);
    }
}
