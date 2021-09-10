using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ProgressOnWorldCanvas : MonoBehaviour
    {
        [SerializeField] SpectraUISettings settings;
        [SerializeField] TMPro.TextMeshProUGUI progressText;
        [SerializeField] Image progressFiller;

        private void Start()
        {
            switch (settings.experienMode)
            {
                case ExperienMode.Evaluacion:
                    gameObject.SetActive(false);
                    break;
                case ExperienMode.Entrenamiento:
                    gameObject.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            if(ExperienceUI.instance)
            {
                switch (settings.startPoint)
                {
                    case StartPoint.Uniforme:
                        progressText.text = ExperienceUI.instance.eppProgress.ToString("f2") + "%";
                        progressFiller.fillAmount = ExperienceUI.instance.eppProgress / 100f;
                        break;
                    case StartPoint.Recepcion:
                        progressText.text = ExperienceUI.instance.receptionProgress.ToString("f2") + "%";
                        progressFiller.fillAmount = ExperienceUI.instance.receptionProgress / 100f;
                        break;
                    case StartPoint.Ubicacion:
                        progressText.text = ExperienceUI.instance.locationProgress.ToString("f2") + "%";
                        progressFiller.fillAmount = ExperienceUI.instance.locationProgress / 100f;
                        break;
                    case StartPoint.Picking:
                        progressText.text = ExperienceUI.instance.pickingProgress.ToString("f2") + "%";
                        progressFiller.fillAmount = ExperienceUI.instance.pickingProgress / 100f;
                        break;
                    case StartPoint.Packing:
                        progressText.text = ExperienceUI.instance.packingProgress.ToString("f2") + "%";
                        progressFiller.fillAmount = ExperienceUI.instance.packingProgress / 100f;
                        break;
                }
            }
        }
    }
}
