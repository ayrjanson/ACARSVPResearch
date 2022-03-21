using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*  Names: Adrian Muth and Chase Ohmstede
    Date: 3-7-2022

    Description: Gets input from text file, splits it by word, and displays each word for a set amount of time.  
    Built using TMP_ExampleScript_01 from the MRTKTutorial as a template.
*/

namespace TMPro.Examples
{
    public class RSVP_text : MonoBehaviour
    {
        public enum objectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

        public objectType ObjectType;
        private TMP_Text rsvp_text;
        private TMP_TextInfo rsvp_info;
        private Button WPMButton;
        private Text WPMText;
        private Text startText;

        public int i; // Used to iterate through word array
        public string countdown = "3 2 1";
        public string test = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec elementum leo sodales sem vehicula ullamcorper. Morbi velit tortor, congue eget auctor ac, porta sit amet elit. Vivamus eu augue vel nisl iaculis tristique. Duis molestie, lacus in pulvinar commodo, risus nulla pretium diam, eget commodo massa ante sit amet nunc. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vivamus lacus arcu, cursus scelerisque ex vel, aliquet eleifend orci. Fusce vel rhoncus ante. Sed volutpat, magna non imperdiet finibus, nulla est condimentum elit, sit amet mollis lacus urna id lectus. Integer condimentum dui nec est commodo, quis tristique mi egestas.";
        public string[] wordArr;
        public bool isStatic;
        public bool timerRunning = false;

        public float wpm;
        public float countdown_wpm = 1.0f;

        public void StartTrigger() 
        {
            CancelInvoke(); // Gets rid of any possible functions still running
            i = 0; // Value initialized here in cause of restarting
            startText.text = "Reset";
            //Set words per minute based on button input
            string currentWPM = WPMText.text;
            switch (currentWPM) {
                case "200 words per minute":
                    wpm = 0.300003f;
                    break;
                case "300 words per minute":
                    wpm = 0.2f;
                    break;
                case "400 words per minute":
                default:
                    wpm = 0.15f;
                    break;
            }
            WPMButton.interactable = false;

            // If file exists, read and parse it.  Else, notify user and use sample text instead
            try {
                StreamReader reader = new StreamReader("titanic.txt");
                string line = "";

                while (reader.Peek() > -1) {
                    line = line + reader.ReadLine();
                }

                wordArr = line.Split(' '); 
            } catch (Exception e) {
                Debug.Log(e.Message);
                wordArr = test.Split(' ');
            }
            timerRunning = true;
            InvokeRepeating("IntervalCountdown", 0.0f, countdown_wpm); // Show one word each .2 seconds
        }

        public void Interval() 
        {
            // Continue as long as the timer is running, the end of the array isn't reached, and the input isn't static
            if (timerRunning) {
                if (i < wordArr.Length) {
                    if (!isStatic) {
                        // Update the displayed text and increment
                        rsvp_text.SetText(wordArr[i]);
                        i++;
                    }
                } else {
                    // End of word array reached - stop timer
                    timerRunning = false;
                    CancelInvoke("Interval");
                    startText.text = "Start";
                    WPMButton.interactable = true;
                }
            }
        }

        public void IntervalCountdown() {
            string[] countdownArr = countdown.Split(' ');
            if (timerRunning)
            {
                if (i < countdownArr.Length)
                {
                    if (!isStatic)
                    {
                        // Update the displayed text and increment
                        rsvp_text.SetText(countdownArr[i]);
                        i++;
                    }
                } else {
                    // End of countdown reached - go to interval
                    InvokeRepeating("Interval", 0.0f, wpm);
                    i = 0;
                    CancelInvoke("IntervalCountdown");
                }
            }
        }

        public void Awake() 
        {
            startText = GameObject.Find("StartText").GetComponent<Text>();
            WPMButton = GameObject.Find("WPMButton").GetComponent<Button>();
            WPMText = GameObject.Find("WPMText").GetComponent<Text>();
            
            // Get text component, adds component if nonexistent
            rsvp_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>(); 
            // Gets the text info
            rsvp_info = rsvp_text.textInfo;
            // Update mesh to reflect new text
            // Ensure the size lines up with the prefered values - came with method (don't touch)
            Vector2 size = rsvp_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);
            rsvp_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        }
    }
}