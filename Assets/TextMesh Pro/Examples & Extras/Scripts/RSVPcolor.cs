using UnityEngine;
using System.Collections;

/*  Names: Adrian Muth, Anna Yrjanson, and Chase Ohmstede
    Date: 3-7-2022

    Description: Gets character count of current word and selects middle character as the pivot.  If the given word is even, 
    an extra space is placed at the beginning to make it odd.  The pivot is then colored red.
    Built using VertexColorCycler from the MRTKTutorial as a template.
*/

namespace TMPro.Examples
{

    public class RSVPcolor : MonoBehaviour
    {
        public int previousCharacter = 0; // Previous colored red character index
        public int currentCharacter = 0; // Current colored red character index
        public bool charChanged = false; // Has pivot character changed
        private TMP_Text m_TextComponent; // The text mesh

        void Awake()
        {
            // Gets the Text Mesh Component to display
            m_TextComponent = GetComponent<TMP_Text>();
        }

        void Start()
        {
            StartCoroutine(AnimateVertexColors()); // Initiates color changing procedure
        }

        
        // Adds space at the front of even length strings to make them odd
        string MakeOdd(string word) {
            word = " " + word;
            return word;
        }
        

        // Returns pivot number, which is in the middle of the string
        int FindPivot(string word) {
            int pivNum = (word.Length - 1)/2;
            return pivNum;
        }

        // Changes color of the indicated character
        IEnumerator AnimateVertexColors()
        {
            // Ensures that there are values to modify
            m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = m_TextComponent.textInfo; // Get text information
            
            Color32[] newVertexColors; // Allows direct color modification

            Color32 pivotColor = new Color32(216, 54, 54, 255); // Red RGBP
            Color32 origColor = new Color32(102, 102, 102, 255); // Gray RGBP
            
            while (true)
            {
                textInfo = m_TextComponent.textInfo; // Update text information
                int characterCount = textInfo.characterCount;

                // Make sure string is always odd
                if (characterCount % 2 == 0) {
                    m_TextComponent.text = MakeOdd(m_TextComponent.text);
                }

                // If no characters present, wait
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.05f);
                    continue;
                }

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

                // Get the vertex colors of the mesh used by text element.
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the previous vertex used by this text element.
                int prevVertexIndex = textInfo.characterInfo[previousCharacter].vertexIndex;

                currentCharacter = FindPivot(m_TextComponent.text); // Get pivot number

                // Keep track of if the word changed
                if (currentCharacter != previousCharacter)
                {
                    charChanged = true;
                }

                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    // Color the pivot character
                    newVertexColors[prevVertexIndex + 0] = pivotColor;
                    newVertexColors[prevVertexIndex + 1] = pivotColor;
                    newVertexColors[prevVertexIndex + 2] = pivotColor;
                    newVertexColors[prevVertexIndex + 3] = pivotColor;

                    // If on a new word, color the old pivot back to normal and indicate that the character is no longer changed
                    if (charChanged) {
                        newVertexColors[prevVertexIndex + 0] = origColor;
                        newVertexColors[prevVertexIndex + 1] = origColor;
                        newVertexColors[prevVertexIndex + 2] = origColor;
                        newVertexColors[prevVertexIndex + 3] = origColor;
                        
                        charChanged = false;
                    }
                                       
                    // Update vertex data - came with method (don't touch)
                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    
                }
                
                // Update character after each iteration and wait
                previousCharacter = currentCharacter;
                yield return new WaitForSeconds(0.02f);

            }
        }
    }
}
