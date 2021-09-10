using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    [CreateAssetMenu (fileName = "SpectraUISettings", menuName = "SOFASA_Logistica_Tenjo/SpectraUISettings", order = 0)]
    public class SpectraUISettings : ScriptableObject {

        public StartPoint startPoint = StartPoint.Recepcion;
        public ExperienMode experienMode = ExperienMode.Evaluacion;
        public List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> pickingProducts;
        public int boxType = 0;

        public void Init()
        {
            startPoint = StartPoint.Uniforme;
            experienMode = ExperienMode.Entrenamiento;
        }
    }
    public enum StartPoint {
        Uniforme,
        Recepcion,
        Ubicacion,
        Picking,
        Packing
    }

    public enum ExperienMode {
        Evaluacion,
        Entrenamiento
    }
}