using FMODUnity;
using UnityEngine;
using Valve.VR.InteractionSystem;
namespace DreamHouseStudios.SofasaLogistica {
    public class FxOnAttached : MonoBehaviour {
        #region Components
        [SerializeField] Interactable interactable;
        [SerializeField] StudioEventEmitter eventEmitter;
        #endregion

        #region Unity Functions
        private void Start () {
            interactable = GetComponent<Interactable> ();
        }

        private void Update () {
            PlayOnAttached ();
        }
        #endregion

        #region Functions
        private void PlayOnAttached () {
            if (interactable.isAttachedToHand == true) {
                Debug.Log ("attached");
                if (eventEmitter) {
                    if (eventEmitter.IsPlaying ())
                        eventEmitter.Stop ();

                    eventEmitter.Play ();
                }
            }
        }
        #endregion
    }
}