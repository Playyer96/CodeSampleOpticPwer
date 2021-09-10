using FMODUnity;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class FxOnGrab : MonoBehaviour {
        #region Components
        public StudioEventEmitter eventEmitter;
        public StudioEventEmitter fxShelf;
        [SerializeField] bool other;

        private Surface surface;

        public enum Surface { Box, Shelf }
        #endregion

        #region Unity Functions
        private void Awake () {
            if (!eventEmitter)
                eventEmitter = GetComponent<StudioEventEmitter> ();

            Mute();
        }

        private void OnDestroy () {
            Mute ();
        }
        #endregion

        #region Functions
        public void PlaySound (float value) {
            if (!other) {
                switch (surface) {
                    case Surface.Box:
                        if (eventEmitter) {
                            if (eventEmitter.IsPlaying ()) eventEmitter.Stop ();

                            eventEmitter.EventInstance.setParameterValue ("GrabNDrop", value);
                            eventEmitter.Play ();
                        }
                        break;
                    case Surface.Shelf:
                        if (fxShelf) {
                            if (fxShelf.IsPlaying ()) fxShelf.Stop ();

                            fxShelf.EventInstance.setParameterValue ("GrabNDrop", value);
                            fxShelf.Play ();
                        }
                        break;
                    default:
                        if (eventEmitter) {
                            if (eventEmitter.IsPlaying ()) eventEmitter.Stop ();

                            eventEmitter.Play ();
                        }
                        break;
                }
            } else {
                if (eventEmitter) {
                    if (eventEmitter.IsPlaying ()) eventEmitter.Stop ();

                    eventEmitter.EventInstance.setParameterValue ("GrabNDrop", value);
                    eventEmitter.Play ();
                }
            }
        }

        public void Mute () {
            if (eventEmitter)
                eventEmitter.Stop ();
            if (fxShelf)
                fxShelf.Stop ();
        }
        #endregion
    }
}