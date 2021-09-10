using System;
using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace DreamHouseStudios.SofasaLogistica {
    public class Toolset : MonoBehaviour {

        #region Components
        [Space(10f), Header("Components")]
        [SerializeField] Transform headTransform;
        [SerializeField] Transform ReferenceUp;
        [SerializeField] float HeadOffset = -0.35f;

        [Space(10f), Header("Objects")]
        public GameObject toolset;
        public GameObject pen;
        public GameObject marker;
        public GameObject splitter;
        public GameObject pocket;
        public GameObject id;

        [Space(10f), Header("Positions")]
        public Transform toolsetPos;
        public Transform splitterPos;
        public Transform pocketPos;

        [HideInInspector] public bool follow = false;

        bool OnToolset;
        #endregion

        #region Unity Functions
        private void OnEnable () {
            follow = true;
            ShowHideTools (true);
            OnSplitterAttach ();
            splitter.transform.SetParent (splitterPos);
        }
        
        private void Update () {
            FollowHead ();
            OnSplitterAttach ();
            OnPocketAttach();

        }

        public void LateUpdate () {
            if (headTransform == null) {
                return;
            }
            Vector3 up = Vector3.up;
            if (ReferenceUp != null) {
                up = ReferenceUp.up;
            }

            transform.position = headTransform.position + (HeadOffset * up);

            Vector3 forward = headTransform.forward;
            Vector3 forwardLeveld1 = forward;
            forwardLeveld1.y = 0;
            forwardLeveld1.Normalize ();
            Vector3 mixedInLocalForward = headTransform.up;
            if (forward.y > 0) {
                mixedInLocalForward = -headTransform.up;
            }
            mixedInLocalForward.y = 0;
            mixedInLocalForward.Normalize ();

            float dot = Mathf.Clamp (Vector3.Dot (forwardLeveld1, forward), 0f, 1f);
            Vector3 finalForward = Vector3.Lerp (mixedInLocalForward, forwardLeveld1, dot * dot);
            transform.rotation = Quaternion.LookRotation (finalForward, up);
        }

        #endregion

        #region Funcitons
        private void OnSplitterAttach () {
            if (!splitter.GetComponent<DreamHouseStudios.VR.Interactable> ().beingGrabbed) {
                splitter.GetComponent<Rigidbody> ().useGravity = false;
                splitter.transform.position = Vector3.MoveTowards (splitter.transform.position, splitterPos.position, 100f);
                splitter.transform.rotation = splitterPos.transform.rotation;
            }
        }

        public void OnPocketAttach () {
            if (!pocket.GetComponent<DreamHouseStudios.VR.Interactable> ().beingGrabbed) {
                pocket.GetComponent<Rigidbody> ().useGravity = false;
                pocket.transform.parent = pocketPos.transform;
                pocket.transform.localPosition = Vector3.zero;
                pocket.transform.localRotation = Quaternion.identity;
            }
        }
        
        private void FollowHead () {
            // toolsetPos.position = Vector3.MoveTowards (toolsetPos.position, new Vector3 (headTransform.transform.position.x + offset.x,
            //     headTransform.transform.position.y + offset.y, headTransform.transform.position.z + offset.z), 5f);
            toolsetPos.rotation = Quaternion.LookRotation (new Vector3 (headTransform.transform.forward.x, 0f, headTransform.transform.forward.z), Vector3.up);
        }

        public void ShowHideTools (bool value) {
            toolset.SetActive (value);
            pen.SetActive(value);
            marker.SetActive(value);
            splitter.SetActive (value);
            id.SetActive (value);
        }
        #endregion
    }
}