using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TMPro.Examples
{
    public class RSVP : MonoBehaviour
    {
        public enum objectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

        public objectType ObjectType; 
        public bool isStatic;

        private TMP_Text rsvp_text;
        private TMP_TextInfo rsvp_info;
        public bool timerRunning = false;

        //private const string newText = "will it work?  idk";
        //public string[] wordArr = {"this", "is", "a", "string"};
        //public string[] wordArr = {"one", "three", "seven", "ooooooooooo"};
        public string test = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec elementum leo sodales sem vehicula ullamcorper. Morbi velit tortor, congue eget auctor ac, porta sit amet elit. Vivamus eu augue vel nisl iaculis tristique. Duis molestie, lacus in pulvinar commodo, risus nulla pretium diam, eget commodo massa ante sit amet nunc. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vivamus lacus arcu, cursus scelerisque ex vel, aliquet eleifend orci. Fusce vel rhoncus ante. Sed volutpat, magna non imperdiet finibus, nulla est condimentum elit, sit amet mollis lacus urna id lectus. Integer condimentum dui nec est commodo, quis tristique mi egestas.";
        public string[] wordArr;
        public int i = 0;
        

        void Start() {
            timerRunning = true;
            InvokeRepeating("Interval", 1.0f, .2f); // Third parameter controls the intervals at which the words appear
            //InvokeRepeating("Interval", 1.0f, 3f);
            wordArr = test.Split(' ');
        }

        void Interval() {
            if (timerRunning) {
                if (i < wordArr.Length) {
                    if (!isStatic) {
                        rsvp_text.SetText(wordArr[i]);
                        // FindPivot(wordArr[i]);
                        i++;
                    }
                } else {
                    timerRunning = false;
                }
            }
        }

        /* int FindPivot(string word) {
            int pivNum = 0;
            if (word.Length <= 0) {
                // do nothing
            } else if (word.Length % 2 == 0) {
                pivNum = (word.Length/2) - 1;
            } else {
                pivNum = (word.Length - 1)/2;
            }
            Debug.Log("piv num of " + word + " is " + pivNum);
            return pivNum;
        } */

        void Awake() {
            rsvp_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();
            rsvp_info = rsvp_text.textInfo;
            Vector2 size = rsvp_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);
            rsvp_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        }
    }
}
