using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace DreamHouseStudios.SofasaLogistica
{
    public class PlayFMODEvent : MonoBehaviour
    {
        #region Components
        [SerializeField] StudioEventEmitter eventEmitter = null;
        [SerializeField] bool playOnAwake = false;
        #endregion

        #region Unity Functions
        private void OnDestroy()
        {
            StopClip();
        }

        private void Start()
        {
            if (!eventEmitter)
                eventEmitter = GetComponent<StudioEventEmitter>();

            if (playOnAwake)
                PlayClip();
        }
        #endregion

        #region Functions
        public void PlayClip()
        {
            if (eventEmitter)
            {
                if (eventEmitter.IsPlaying())
                    eventEmitter.Stop();

                eventEmitter.Play();
            }
        }

        public void StopClip() {
            if (eventEmitter)
            {
                if (eventEmitter.IsPlaying())
                    eventEmitter.Stop();
            }
        }
        #endregion
    }
}