using FMODUnity;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class FMODEventPlayer : MonoBehaviour {
        #region Components
        public StudioEventEmitter eventEmitter;
        #endregion

        #region Unity Functions
        private void Awake () {
            eventEmitter = GetComponent<StudioEventEmitter> ();
        }

        private void OnDestroy () {
            Mute ();
        }
        #endregion

        #region Functions
        public void PlaySound (float parameter, string parmName = "") {

            if (eventEmitter.IsPlaying ()) {
                TrySetSoundParams (eventEmitter, parmName, parameter);

                Debug.LogWarningFormat ("Sound '{0}' is already playing", eventEmitter);
                return;
            }
            TrySetSoundParams (eventEmitter, parmName, parameter);
            eventEmitter.Play ();
        }

        public void TrySetSoundParams (StudioEventEmitter sound, string parmName, float parameter) {
            if (sound.Params.Length == 0) {
                Debug.LogWarningFormat ("Params of event '{0}' are empty", sound.name);
                return;
            }
            string realParam = sound.Params[FindIndex (sound.Params, parmName)].Name;

            if (!string.IsNullOrEmpty (realParam))
                sound.SetParameter (realParam, parameter);
            else
                Debug.LogWarningFormat ("Param '{0}' does not exist for {1}", sound.name, parmName);
        }

        private int FindIndex (ParamRef[] array, string name) {
            if (string.IsNullOrEmpty (name))
                return 0;
            else {
                for (int i = 0; i < array.Length; i++) {
                    if (array[i].Name == name)
                        return i;
                }
            }
            return 0;
        }

        public void Mute () {
            eventEmitter.Stop ();
        }
        #endregion
    }
}