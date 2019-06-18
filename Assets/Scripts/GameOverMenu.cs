using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI textObject;

    public string loseText;
    public Color loseColor;

    public string winText;
    public Color winColor;

    public void Won()
    {
        textObject.text = winText;
        textObject.color = winColor;
    }

    public void Lost()
    {
        textObject.text = loseText;
        textObject.color = loseColor;
    }
}
