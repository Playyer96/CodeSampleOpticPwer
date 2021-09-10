using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    [CreateAssetMenu (fileName = "PocketData", menuName = "SOFASA_Logistica_Tenjo/PocketData", order = 0)]
    public class PocketData : ScriptableObject {
        public List<BoxData> boxes = new List<BoxData> ();
        public List<ProductData> products = new List<ProductData> ();
    }
}