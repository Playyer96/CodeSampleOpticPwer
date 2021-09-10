using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class SettingsManager : MonoBehaviour {
        #region Components
        public SpectraUISettings settings;
        [SerializeField] Transform uniformPosition;
        [SerializeField] List<Transform> position;
        #endregion

        public Transform GetInitialPos {
            get { return uniformPosition; }
        }

        public Transform GetUbication {
            get { return position[(int) settings.startPoint]; }
        }
    }
}