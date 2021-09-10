using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class FxOncollision : MonoBehaviour {
        #region Components
        public StudioEventEmitter eventEmitter;
        public StudioEventEmitter fxDropEstanteria;
        public StudioEventEmitter fxDropEnCaja;
        [Range (0, 1f), SerializeField] float value;

        public bool hasParams = true;
        #endregion

        #region Unity Functions
        private void Awake () {
            if (!eventEmitter)
                eventEmitter = GetComponent<StudioEventEmitter> ();
        }

        private void Start () {
            eventEmitter.EventInstance.setParameterValue ("TipoElemento", value);
        }

        private void OnDestroy () {
            Mute ();
        }

        private void OnCollisionEnter (Collision other) {
            switch (other.gameObject.tag) {
                case "Floor":
                    if (eventEmitter) {
                        if (eventEmitter.IsPlaying ())
                            eventEmitter.Stop ();

                        if (hasParams)
                            eventEmitter.EventInstance.setParameterValue ("TipoElemento", value);
                        eventEmitter.Play ();
                    }
                    break;
                case "Estanteria":
                    if (fxDropEstanteria) {
                        if (fxDropEstanteria.IsPlaying ())
                            fxDropEstanteria.Stop ();

                        if (hasParams)
                            fxDropEstanteria.EventInstance.setParameterValue ("TipoElemento", value);
                        fxDropEstanteria.Play ();
                    }
                    break;
                case "Box":
                    if (fxDropEnCaja) {
                        if (fxDropEnCaja.IsPlaying ())
                            fxDropEnCaja.Stop ();

                        fxDropEnCaja.Play ();
                        if (hasParams)
                            fxDropEnCaja.EventInstance.setParameterValue ("TipoElemento", value);
                    }
                    break;
                default:
                    if (eventEmitter) {
                        if (eventEmitter.IsPlaying ()) eventEmitter.Stop ();
                        eventEmitter.Play ();
                    }
                    break;
            }
        }
        #endregion

        #region Functions
        public void Mute () {
            if (eventEmitter)
                eventEmitter.Stop ();
            if (fxDropEstanteria)
                fxDropEstanteria.Stop ();
            if (fxDropEnCaja)
                fxDropEnCaja.Stop ();
        }
        #endregion
    }
}