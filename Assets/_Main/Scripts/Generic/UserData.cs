using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UserData", menuName = "SOFASA_Logistica_Tenjo/UserData", order = 0)]
public class UserData : ScriptableObject
{
    [Space(10f), Header("URLs")] public string localUrl = "localhost:3000";
    public string serverUrl = "https://backendsofasa.herokuapp.com";

    public DreamHouseStudios.SofasaLogistica.SpectraUISettings settings;

    [Space(10f), Header("Experience Data")]
    public string trainer;

    public string trainerId;
    public string sede;
    public string fullName;
    public string id;
    public string mail;
    public string phone;
    public string address;
    public string date;
    public string finalizationDate;
    public float eppsProgress;
    public string eppTime;
    public float receptionProgress;
    public string receptionTime;
    public float locationProgress;
    public string locationTime;
    public float pickingProgress;
    public string pickingTime;
    public float packingProgress;
    public string packingTime;
    public string totalProgress;
    public float grade;

    [HideInInspector] public CreateUser createUser;
    public ModuleAdvance moduleAdvance;
    public ModuleGrade moduleGrade;
    [HideInInspector] public ModuleReset moduleReset;
    public ReportUser reportUser;

    public void Init()
    {
        fullName = "";
        id = "";
        mail = "";
        phone = "";
        address = "";
        date = "";
        finalizationDate = "";
        eppsProgress = 0;
        eppTime = "0";
        receptionProgress = 0;
        receptionTime = "0";
        locationProgress = 0;
        locationTime = "0";
        pickingProgress = 0;
        pickingTime = "0";
        packingProgress = 0;
        packingTime = "0";
        totalProgress = "";
        grade = 0;
    }


    public void CreateUserData()
    {
        createUser.name = fullName;
        createUser.cedula = id;
        createUser.email = mail;
        createUser.phone = phone;
        createUser.sede = sede;
        createUser.address = address;
        createUser.formador.name = trainer;
        createUser.formador.cedula = trainerId;
    }

    public void SendModuleAdvance()
    {
        moduleAdvance.cedula = id;
        switch (settings.startPoint)
        {
            case DreamHouseStudios.SofasaLogistica.StartPoint.Uniforme:
                moduleAdvance.module = "1";
                moduleAdvance.advance = float.Parse(eppsProgress.ToString("f2"));
                ;
                moduleAdvance.tiempoQueDemoro = eppTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Recepcion:
                moduleAdvance.module = "2";
                moduleAdvance.advance = float.Parse(receptionProgress.ToString("f2"));
                ;
                moduleAdvance.tiempoQueDemoro = receptionTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Ubicacion:
                moduleAdvance.module = "3";
                moduleAdvance.advance = float.Parse(locationProgress.ToString("f2"));
                ;
                moduleAdvance.tiempoQueDemoro = locationTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Picking:
                moduleAdvance.module = "4";
                moduleAdvance.advance = float.Parse(pickingProgress.ToString("f2"));
                ;
                moduleAdvance.tiempoQueDemoro = pickingTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Packing:
                moduleAdvance.module = "5";
                moduleAdvance.advance = float.Parse(packingProgress.ToString("f2"));
                ;
                moduleAdvance.tiempoQueDemoro = packingTime;
                break;
        }
    }

    public void SendGrade()
    {
        moduleGrade.cedula = id;
        switch (settings.startPoint)
        {
            case DreamHouseStudios.SofasaLogistica.StartPoint.Uniforme:
                moduleGrade.module = "1";
                moduleGrade.tiempoQueDemoro = eppTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Recepcion:
                moduleGrade.module = "2";
                moduleGrade.tiempoQueDemoro = receptionTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Ubicacion:
                moduleGrade.module = "3";
                moduleGrade.tiempoQueDemoro = locationTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Picking:
                moduleGrade.module = "4";
                moduleGrade.tiempoQueDemoro = pickingTime;
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Packing:
                moduleGrade.module = "5";
                moduleGrade.tiempoQueDemoro = packingTime;
                break;
        }
        moduleGrade.grade = float.Parse((grade/10f).ToString("f2"));
    }

    public void ResetModule()
    {
        moduleReset.cedula = id;
        switch (settings.startPoint)
        {
            case DreamHouseStudios.SofasaLogistica.StartPoint.Uniforme:
                moduleReset.module = "1";
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Recepcion:
                moduleReset.module = "2";
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Ubicacion:
                moduleReset.module = "3";
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Picking:
                moduleReset.module = "4";
                break;
            case DreamHouseStudios.SofasaLogistica.StartPoint.Packing:
                moduleReset.module = "5";
                break;
        }
    }

    public void CreateReport()
    {
        
        
        
        if (SendReportBackEnd.Instance != null)
        {
            SendReportBackEnd.Instance.CreateReport();
            reportUser.cedula = id;
            switch (settings.startPoint)
            {
                case DreamHouseStudios.SofasaLogistica.StartPoint.Uniforme:
                    reportUser.module = "1";
                    break;
                case DreamHouseStudios.SofasaLogistica.StartPoint.Recepcion:
                    reportUser.module = "2";
                    break;
                case DreamHouseStudios.SofasaLogistica.StartPoint.Ubicacion:
                    reportUser.module = "3";
                    break;
                case DreamHouseStudios.SofasaLogistica.StartPoint.Picking:
                    reportUser.module = "4";
                    break;
                case DreamHouseStudios.SofasaLogistica.StartPoint.Packing:
                    reportUser.module = "5";
                    break;
            }

            reportUser.aciertos = SendReportBackEnd.Instance._GoodReport();
            reportUser.fallos = SendReportBackEnd.Instance._BadReport();
        }
    }

    [Serializable]
    public class CreateUser
    {
        public string name;
        public string cedula;
        public string email;
        public string phone;
        public string sede;
        public string address;
        public Formador formador;
    }

    [Serializable]
    public class Formador
    {
        public string name;
        public string cedula;
    }

    [Serializable]
    public class ModuleAdvance
    {
        public string cedula;
        public string module;
        public float advance;
        public string tiempoQueDemoro;
        public string fechaInicialEntrenamiento;
        public string fechaFinalEntrenamiento;
    }

    [Serializable]
    public class ModuleGrade
    {
        public string cedula;
        public string module;
        public float grade;
        public string tiempoQueDemoro;
        public string fechaInicialEvaluacion;
        public string fechaFinalEvaluacion;
    }

    [Serializable]
    public class ModuleReset
    {
        public string cedula;
        public string module;
    }
    
    [Serializable]
    public class ReportUser
    {
        public string cedula;
        public string module;
        public string[] aciertos;
        public string[] fallos;
    }
}