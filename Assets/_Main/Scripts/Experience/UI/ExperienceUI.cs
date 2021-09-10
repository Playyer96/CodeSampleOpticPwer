using System;
using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseSpectra.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica {
    public class ExperienceUI : MonoBehaviour {
        #region Components

        public static ExperienceUI instance;

        [SerializeField] UserData userData;
        [SerializeField] SpectraUISettings settings;

        [Space (10f), Header ("UI")][SerializeField]
        TMPro.TextMeshProUGUI sectionTitle;

        [SerializeField] TMPro.TextMeshProUGUI sectionProgress;
        [SerializeField] Image sectionProgressFiller;
        [SerializeField] TMPro.TextMeshProUGUI totalProgress;
        [SerializeField] Image totalProgressFiller;
        [SerializeField] private TMPro.TextMeshProUGUI modeText;

        [HideInInspector] public float eppProgress;
        [HideInInspector] public float receptionProgress;
        [HideInInspector] public float locationProgress;
        [HideInInspector] public float pickingProgress;
        [HideInInspector] public float packingProgress;
        [HideInInspector] public float finalProgress;
        [HideInInspector] public bool stopChronometer = false;

        #endregion

        #region Unity Functions

        private void Awake () {
            if (instance != null && instance != this) {
                Destroy (this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad (this.gameObject);
        }

        private void Update () {
            UpdateTotalProgress ();

            switch (settings.startPoint) {
                case StartPoint.Uniforme:
                    sectionTitle.text = "EPPs";
                    sectionProgress.text = eppProgress.ToString ("f2") + "%";
                    sectionProgressFiller.fillAmount = eppProgress / 100f;
                    userData.grade = float.Parse (eppProgress.ToString ("f2"));
                    break;
                case StartPoint.Recepcion:
                    sectionTitle.text = " Recepción";
                    sectionProgress.text = receptionProgress.ToString ("f2") + "%";
                    sectionProgressFiller.fillAmount = receptionProgress / 100f;
                    userData.grade = float.Parse (receptionProgress.ToString ("f2"));
                    break;
                case StartPoint.Ubicacion:
                    sectionTitle.text = "Ubicación";
                    sectionProgress.text = locationProgress.ToString ("f2") + "%";
                    sectionProgressFiller.fillAmount = locationProgress / 100f;
                    userData.grade = float.Parse (locationProgress.ToString ("f2"));
                    break;
                case StartPoint.Picking:
                    sectionTitle.text = "Picking";
                    sectionProgress.text = pickingProgress.ToString ("f2") + "%";
                    sectionProgressFiller.fillAmount = pickingProgress / 100f;
                    userData.grade = float.Parse (pickingProgress.ToString ("f2"));
                    break;
                case StartPoint.Packing:
                    sectionTitle.text = "Packing";
                    sectionProgress.text = packingProgress.ToString ("f2") + "%";
                    sectionProgressFiller.fillAmount = packingProgress / 100f;
                    userData.grade = float.Parse (packingProgress.ToString ("f2"));
                    break;
            }

            if (sectionProgress.text == "100.00%") {
                stopChronometer = true;
            } else {
                stopChronometer = false;
            }

            if (modeText) {
                switch (settings.experienMode) {
                    case ExperienMode.Entrenamiento:
                        modeText.text = "Modo Entrenamiento";
                        break;
                    case ExperienMode.Evaluacion:
                        modeText.text = "Modo Evaluación";
                        break;
                }
            }
        }

        #endregion

        #region Functions

        public void UpdateTotalProgress () {
            finalProgress = (eppProgress + receptionProgress + locationProgress + pickingProgress + packingProgress) / 5f;
            totalProgress.text = finalProgress.ToString ("f2") + "%";
            totalProgressFiller.fillAmount = finalProgress / 100f;
        }

        public void HardResetProgress () {
            eppProgress = 0;
            receptionProgress = 0;
            locationProgress = 0;
            pickingProgress = 0;
            packingProgress = 0;
            finalProgress = 0;
            totalProgress.text = "0.00%";
            totalProgressFiller.fillAmount = 0;

        }
        #endregion
    }
}