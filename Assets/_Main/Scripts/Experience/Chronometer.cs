using UnityEngine;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica {
    public class Chronometer : MonoBehaviour {
        #region Components
        [SerializeField] TMPro.TextMeshProUGUI experienceChronometer;
        [SerializeField] TMPro.TextMeshProUGUI sectionChronometer;
        public string backendChronometer = "";
        float totalTime = 0;
        public float sectionTime = 0;
        string sectionTimeFormat;

        /*public float TotalTime { get { return totalTime; } }
        public float SecionTime { get { return sectionTime; } }
        public string SectionTimeFormat { get { return sectionTimeFormat; } }*/
        #endregion

        #region Unity Functions
        private void Update () {
            SectionChronometer ();
            ExperienceChronometer ();
        }

        #endregion

        #region Functions
        public void SectionChronometer () {
            if (PauseButton.isPause && ScenesManager.instance.isLoadingScene || ExperienceUI.instance.stopChronometer) {
                return;
            }
            
            if(PauseButton.isPause)
                return;

            //sectionTime += Time.deltaTime;

            string seconds = Mathf.FloorToInt (sectionTime % 60).ToString ("00");
            string minutes = Mathf.FloorToInt (sectionTime / 60).ToString ("00");
            
            float sseconds = float.Parse(seconds);
            sseconds = sseconds / 60f;
            sseconds = sseconds * 100;

            sectionTimeFormat = string.Format ("{0}:{1}", minutes, seconds);
            backendChronometer = string.Format ("{0}.{1}", minutes, sseconds);

            sectionChronometer.text = sectionTimeFormat;
        }
        public void ExperienceChronometer () {
            totalTime += Time.deltaTime;

            string seconds = Mathf.FloorToInt (totalTime % 60).ToString ("00");
            string minutes = Mathf.FloorToInt (totalTime / 60).ToString ("00");

            if (experienceChronometer)
                experienceChronometer.text = string.Format ("{0}:{1}", minutes, seconds);
        }

        public void ResetSetionChronometer ()
        {
            //sectionTime = 0;
            sectionTimeFormat = "0";
            backendChronometer = "0";
        }

        #endregion
    }
}