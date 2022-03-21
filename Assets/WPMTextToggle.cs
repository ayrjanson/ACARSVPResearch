using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WPMTextToggle : MonoBehaviour
{
    private Button WPMButton;
    private Text WPMText;

    public void Awake() {
        WPMButton = GameObject.Find("WPMButton").GetComponent<Button>();
        WPMText = GameObject.Find("WPMText").GetComponent<Text>();
    }

    // Start is called before the first frame update
    public void changeButtonText() {
        string currentWPM = WPMText.text;
        
        switch (currentWPM) {
            case "200 words per minute":
                WPMText.text = "300 words per minute";
                break;
            case "300 words per minute":
                WPMText.text = "400 words per minute";
                break;
            case "400 words per minute":
            default:
                WPMText.text = "200 words per minute";
                break;
        }
    }
}
