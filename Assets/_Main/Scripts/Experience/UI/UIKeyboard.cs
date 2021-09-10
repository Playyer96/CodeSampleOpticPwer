using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica {
    public class UIKeyboard : MonoBehaviour {
        #region Components
        public static UIKeyboard instance;
        public InputField actualInputField;

        public string word = null;
        int wordIndex = 0;
        string alpha;

        bool isMayus;
        #endregion

        #region Unity Functions
        private void Awake () {
            if (instance != null && instance != this) {
                Destroy (this.gameObject);
            }

            instance = this;
        }

        private void Start () {
            word = null;
        }
        #endregion

        #region Functions
        public void AlphabetFunction (string alphabet) {
            wordIndex++;
            if (isMayus) {
                word = word + alphabet.ToUpper ();
            } else {
                word = word + alphabet;
            }

            if (actualInputField) {
                actualInputField.text = word.ToString ();
                actualInputField.ActivateInputField ();
            } else {
                word = null;
                Debug.LogError ("Don't Have a text reference");
            }
        }

        public void MayusActivated () {
            if (isMayus)
                isMayus = false;
            else
                isMayus = true;
        }

        public void DelKey () {
            if (wordIndex > 0) {
                wordIndex--;
                word = word.Substring (0, word.Length - 1);

                if (actualInputField) {
                    actualInputField.text = word;
                    actualInputField.ActivateInputField ();
                } else {
                    Debug.Log (word);
                    Debug.LogError ("Don't Have a text reference");
                }
            }
        }

        public void EnterKey () {
            Debug.Log ("Enter Key Pressed");
        }

        public void GetText (InputField inputField) {
            StartCoroutine (GetReference (inputField));
        }

        IEnumerator GetReference (InputField inputField) {
            yield return new WaitForEndOfFrame ();
            actualInputField = inputField;
            Debug.Log (actualInputField);
            word = null;
            yield return new WaitForEndOfFrame ();
        }
        #endregion
    }
}