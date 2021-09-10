using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DreamHouseStudios.SofasaLogistica {
    public class SceneLoader : MonoBehaviour {

        #region Components
        [SerializeField] string sceneName;

        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds waitForScene;
        IEnumerator loadScene;
        #endregion

        #region Unity Functions
        private void Awake () {
            waitForScene = new WaitForSeconds (2f);
            waitForEndOfFrame = new WaitForEndOfFrame ();
        }
        #endregion

        #region Functions
        public void StartSceneLoad () {
            if (loadScene != null)
                StopCoroutine (loadScene);

            loadScene = LoadScene ();
            StartCoroutine (loadScene);
        }

        IEnumerator LoadScene () {
            yield return waitForEndOfFrame;
            SceneManager.LoadScene (sceneName);
            yield return waitForScene;
        }
        #endregion
    }
}