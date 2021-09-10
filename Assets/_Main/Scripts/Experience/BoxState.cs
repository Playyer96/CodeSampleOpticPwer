using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class BoxState : MonoBehaviour {
        #region Components
        [HideInInspector] public BoxStates boxStates;
        [SerializeField] GameObject openBox;
        public BoxTransforms[] boxTransforms;

        bool boxIsSealed = false;

        public bool BoxIsSealed { get { return boxIsSealed; } }

        public enum BoxStates { FlatBox, OpenBox, SealedBox }

        [System.Serializable]
        public class BoxTransforms {
            public Transform[] transforms;
        }
        #endregion

        #region Functions
        public void BoxStateChanger () {
            switch (boxStates) {
                case BoxStates.FlatBox:
                    FlatBox ();
                    break;
                case BoxStates.OpenBox:
                    OpenBox ();
                    break;
                case BoxStates.SealedBox:
                    SealedBox ();
                    break;
            }
        }

        private void FlatBox () {
            openBox.SetActive (false);
            foreach (Transform t in boxTransforms[1].transforms) {
                t.gameObject.SetActive (false);
            }

            if (boxTransforms[0] != null) {
                if (boxTransforms[0].transforms[0] != null) {
                    boxTransforms[0].transforms[0].gameObject.SetActive (true);
                }
            }
        }

        private void OpenBox () {
            openBox.SetActive (true);
            foreach (Transform t in boxTransforms[1].transforms) {
                t.gameObject.SetActive (true);
            }

            //boxTransforms[1].transforms[0].eulerAngles = new Vector3 (0, 0, 0);
            //boxTransforms[1].transforms[1].eulerAngles = new Vector3 (0, 0, 0);
            //boxTransforms[1].transforms[2].eulerAngles = new Vector3 (0, 0, 0);
            //boxTransforms[1].transforms[3].eulerAngles = new Vector3 (0, 0, 0);

            if (boxTransforms[0] != null) {
                if (boxTransforms[0].transforms[0] != null) {
                    boxTransforms[0].transforms[0].gameObject.SetActive (false);
                }
            }
        }

        private void SealedBox () {
            foreach (Transform t in boxTransforms[1].transforms) {
                t.gameObject.SetActive (true);
            }

            openBox.SetActive (true);
            boxTransforms[1].transforms[0].eulerAngles = new Vector3 (-90, 0, 0);
            boxTransforms[1].transforms[1].eulerAngles = new Vector3 (90, 0, 0);
            boxTransforms[1].transforms[2].eulerAngles = new Vector3 (0, 0, -90);
            boxTransforms[1].transforms[3].eulerAngles = new Vector3 (0, 0, 90);

            if (boxTransforms[0] != null) {
                if (boxTransforms[0].transforms[0] != null) {
                    boxTransforms[0].transforms[0].gameObject.SetActive (false);
                }
            }

            boxIsSealed = true;
        }
        #endregion
    }
}