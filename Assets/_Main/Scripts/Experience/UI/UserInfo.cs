using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    [CreateAssetMenu (fileName = "UserInfo", menuName = "SOFASA_Logistica_Tenjo/UserInfo", order = 0)]
    public class UserInfo : ScriptableObject {
        public UserData[] userData;

        [System.Serializable]
        public class UserData {
            public string mandante;
            public string username;
            public string password;
        }
    }
}