using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

    public class color_mod : MonoBehaviour
    {
        public int previousCharacter = 0;
        public int currentCharacter = 0;
        private TMP_Text m_TextComponent;
        public bool charChanged = false;

        void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }


        void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }

        int FindPivot(string word) {
            int pivNum = 0;
            if (word.Length <= 0) {
                // do nothing
            } else if (word.Length % 2 == 0) {
                pivNum = (word.Length/2) - 1;
            } else {
                pivNum = (word.Length - 1)/2;
            }
            //Debug.Log("piv num of " + word + " is " + pivNum);
            return pivNum;
        }

        /// <summary>
        /// Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateVertexColors()
        {
            // Force the text object to update right away so we can have geometry to modify right from the start.
            m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            
            //int currentCharacter = FindPivot(m_TextComponent.text);
            Color32[] newVertexColors;
            //Color32 c0 = m_TextComponent.color;
            Color32 c0 = new Color32(222, 0, 0, 255);
            Color32 origColor = new Color32(102, 102, 102, 255);

            while (true)
            {
                int characterCount = textInfo.characterCount;

                // If No Characters then just yield and wait for some text to be added
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int newVertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
                int prevVertexIndex = textInfo.characterInfo[previousCharacter].vertexIndex;

                currentCharacter = FindPivot(m_TextComponent.text);
                if (currentCharacter != previousCharacter) {
                    charChanged = true;
                }
                

                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    //c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                    //c0 = Color.blue;
                    //Debug.Log("applying color to character " + currentCharacter + " in word " + m_TextComponent.text);
                    newVertexColors[newVertexIndex + 0] = c0;
                    newVertexColors[newVertexIndex + 1] = c0;
                    newVertexColors[newVertexIndex + 2] = c0;
                    newVertexColors[newVertexIndex + 3] = c0;

                    if (charChanged) {
                        newVertexColors[prevVertexIndex + 0] = origColor;
                        newVertexColors[prevVertexIndex + 1] = origColor;
                        newVertexColors[prevVertexIndex + 2] = origColor;
                        newVertexColors[prevVertexIndex + 3] = origColor;
                        charChanged = false;
                    }
                    
                    
                    // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    
                    // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                    // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
                }

                //currentCharacter = (currentCharacter + 1) % characterCount;
                
                previousCharacter = currentCharacter;
                yield return new WaitForSeconds(0.05f);
            }
        }

    }
}
