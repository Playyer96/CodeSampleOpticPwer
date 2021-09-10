using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseSpectra.Networking;
using DreamHouseSpectra.Networking.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica
{
    public class MainMenu : MonoBehaviour
    {
        #region Components

        [Space(10f), Header("Seleccion de Modo")] [SerializeField]
        Button[] trainingButton = null;

        [SerializeField] Button[] evaluationButton = null;
        [SerializeField] Section section = null;

        [Space(10f), Header("Ingreso de Datos Formador")] [SerializeField]
        TMPro.TMP_InputField nombreFormador = null;
        [SerializeField] TMPro.TMP_InputField cedulaFormador = null;

        [SerializeField] TMPro.TMP_Dropdown sedeDropdown = null;

        [Space(10f), Header("Ingreso de Datos")] [SerializeField]
        TMPro.TMP_InputField nombre = null;

        [SerializeField] TMPro.TMP_InputField cedula = null;
        [SerializeField] TMPro.TMP_InputField mail = null;
        [SerializeField] TMPro.TMP_InputField phone = null;
        [SerializeField] TMPro.TMP_InputField address = null;

        [Space(10f), Header("Textos de Boton")] [SerializeField]
        TMPro.TextMeshProUGUI zonaSeleccionada = null;

        [SerializeField] TMPro.TextMeshProUGUI modoSeleccionado = null;

        [Space(10f), Header("Menus")] [SerializeField]
        GameObject ingresoDatosFormador = null;

        [SerializeField] GameObject ingresoDeDatos = null;
        [SerializeField] GameObject seleccionDeModo = null;

        [Space(10f), Header("Settings")] [SerializeField]
        SpectraUISettings settings = null;

        [Space(10f), Header("Tab Inputs")]
        [SerializeField] private GameObject menuDatosFormador;
        [SerializeField] private GameObject menuDatosUsuario;

        [SerializeField] private GameObject[] t_DatosFormador = null;
        [SerializeField] private GameObject[] t_DatosUsuario = null;

        [SerializeField] UserData userData = null;

        [SerializeField] Color normalColor = Color.black;

        Color disabledColor = Color.white;

        public Sedes sedes = null;


        #endregion

        #region Unity Functions

        private void Awake()
        {
            WebServiceManager.GetDataFromEndpoint<Sedes>(userData.serverUrl + "/api/sedes/find",
                string.Empty, ResponseSucceded, ResponsedFailed);
        }

        private void Start()
        {
            if (ingresoDatosFormador)
                ingresoDatosFormador.SetActive(true);
            if (ingresoDeDatos)
                ingresoDeDatos.SetActive(false);
            if (seleccionDeModo)
                seleccionDeModo.SetActive(false);

            userData.Init();
            //settings.Init();

            sedeDropdown.ClearOptions();
        }

        private void ResponsedFailed(Error obj)
        {
            Debug.LogError("Failed");
        }

        private void ResponseSucceded(Sedes obj)
        {
            sedes = obj;
            Debug.Log("Succeed");

            if(sedes != null)
            {
                StartCoroutine(FillDropDownOptions());
            }
        }

        private void Update()
        {
            if (settings.experienMode == ExperienMode.Entrenamiento)
            {
                for (int i = 0; i < evaluationButton.Length; i++)
                {
                    trainingButton[i].interactable = false;
                    evaluationButton[i].interactable = true;

                    trainingButton[2].image.color = disabledColor;
                    evaluationButton[2].image.color = normalColor;

                    modoSeleccionado.text = "Entrenamiento";
                }

                /* if (sedes.sedes.Count > 0)
                 {
                     foreach (var s in sedes.sedes)
                     {
                         sedeDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData { text = s.name });
                     }
                 }*/
            }
            else
            {
                for (int i = 0; i < trainingButton.Length; i++)
                {
                    trainingButton[i].interactable = true;
                    evaluationButton[i].interactable = false;

                    evaluationButton[2].image.color = disabledColor;
                    trainingButton[2].image.color = normalColor;

                    modoSeleccionado.text = "Evaluación";
                }
            }

            switch (settings.startPoint)
            {
                case StartPoint.Uniforme:
                    for (int i = 0; i < section.uniform.Length; i++)
                    {
                        section.uniform[i].interactable = false;
                        section.reception[i].interactable = true;
                        section.ubication[i].interactable = true;
                        section.picking[i].interactable = true;
                        section.packing[i].interactable = true;

                        section.uniform[2].image.color = disabledColor;
                        section.reception[2].image.color = normalColor;
                        section.ubication[2].image.color = normalColor;
                        section.picking[2].image.color = normalColor;
                        section.packing[2].image.color = normalColor;

                        zonaSeleccionada.text = "Elementos de Protección Personal (EPP)";
                    }

                    break;
                case StartPoint.Recepcion:
                    for (int i = 0; i < section.reception.Length; i++)
                    {
                        section.uniform[i].interactable = true;
                        section.reception[i].interactable = false;
                        section.ubication[i].interactable = true;
                        section.picking[i].interactable = true;
                        section.packing[i].interactable = true;

                        section.reception[2].image.color = disabledColor;
                        section.uniform[2].image.color = normalColor;
                        section.ubication[2].image.color = normalColor;
                        section.picking[2].image.color = normalColor;
                        section.packing[2].image.color = normalColor;

                        zonaSeleccionada.text = "Recepción De Mercancías";
                    }

                    break;
                case StartPoint.Ubicacion:
                    for (int i = 0; i < section.ubication.Length; i++)
                    {
                        section.uniform[i].interactable = true;
                        section.reception[i].interactable = true;
                        section.ubication[i].interactable = false;
                        section.picking[i].interactable = true;
                        section.packing[i].interactable = true;

                        section.ubication[2].image.color = disabledColor;
                        section.reception[2].image.color = normalColor;
                        section.uniform[2].image.color = normalColor;
                        section.picking[2].image.color = normalColor;
                        section.packing[2].image.color = normalColor;

                        zonaSeleccionada.text = "Ubicación";
                    }

                    break;
                case StartPoint.Picking:
                    for (int i = 0; i < section.picking.Length; i++)
                    {
                        section.uniform[i].interactable = true;
                        section.reception[i].interactable = true;
                        section.ubication[i].interactable = true;
                        section.picking[i].interactable = false;
                        section.packing[i].interactable = true;

                        section.picking[2].image.color = disabledColor;
                        section.reception[2].image.color = normalColor;
                        section.ubication[2].image.color = normalColor;
                        section.uniform[2].image.color = normalColor;
                        section.packing[2].image.color = normalColor;

                        zonaSeleccionada.text = "Picking";
                    }

                    break;
                case StartPoint.Packing:
                    for (int i = 0; i < section.packing.Length; i++)
                    {
                        section.uniform[i].interactable = true;
                        section.reception[i].interactable = true;
                        section.ubication[i].interactable = true;
                        section.picking[i].interactable = true;
                        section.packing[i].interactable = false;

                        section.packing[2].image.color = disabledColor;
                        section.reception[2].image.color = normalColor;
                        section.ubication[2].image.color = normalColor;
                        section.picking[2].image.color = normalColor;
                        section.uniform[2].image.color = normalColor;

                        zonaSeleccionada.text = "Packing";
                    }

                    break;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (menuDatosFormador.activeInHierarchy)
                {
                    indexFormador++;
                    hentliste(t_DatosFormador, indexFormador);
                    if (indexFormador < (t_DatosFormador.Length))
                    {
                        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(t_DatosFormador[indexFormador]);
                    }else  if(indexFormador >= t_DatosFormador.Length)
                    {
                        indexFormador = 0;
                        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(t_DatosFormador[indexFormador]);
                    }
                }
                else
                {
                    indexFormador = -1;
                }
                
                if (menuDatosUsuario.activeInHierarchy)
                {
                    indexUsuario++;
                    hentliste(t_DatosUsuario, indexUsuario);
                    if (indexUsuario < (t_DatosUsuario.Length))
                    {
                        print(" 2 " + (t_DatosUsuario.Length));
                        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(t_DatosUsuario[indexUsuario]);
                    }
                    else if (indexUsuario >= t_DatosUsuario.Length)
                    {
                        indexUsuario = 0;
                        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(t_DatosUsuario[indexUsuario]);
                    }
                }
                else
                {
                    indexUsuario = -1;
                }
            }
        }

        int indexFormador;
        int indexUsuario;

        public void hentliste(GameObject[] menuItems, int index)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject.name == menuItems[i].name)
                {
                    index = i;
                }
            }
        }

        #endregion

        #region Functions
        IEnumerator FillDropDownOptions()
        {
            yield return new WaitForEndOfFrame();
            sedeDropdown.ClearOptions();
            if (sedes.sedes.Count > 0)
            {
                foreach (var s in sedes.sedes)
                {
                    sedeDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData { text = s.name });
                }
            }
            yield return new WaitForSeconds(0.125f);
                sedeDropdown.RefreshShownValue();
        }

        public void TrainingMode()
        {
            settings.experienMode = ExperienMode.Entrenamiento;
        }

        public void EvaluationMode()
        {
            settings.experienMode = ExperienMode.Evaluacion;
        }

        public void Epp()
        {
            settings.startPoint = StartPoint.Uniforme;
        }

        public void Reception()
        {
            settings.startPoint = StartPoint.Recepcion;
        }

        public void Ubication()
        {
            settings.startPoint = StartPoint.Ubicacion;
        }

        public void Picking()
        {
            settings.startPoint = StartPoint.Picking;
        }

        public void Packing()
        {
            settings.startPoint = StartPoint.Packing;
        }

        public void TrainerData()
        {
            if (nombreFormador && nombreFormador.text != null)
            {
                userData.trainer = nombreFormador.text;
            }

            if(cedulaFormador && cedulaFormador.text != null)
            {
                userData.trainerId = cedulaFormador.text;
            }

            if (ingresoDatosFormador)
            {
                ingresoDatosFormador.SetActive(false);
            }

            if (ingresoDeDatos)
            {
                ingresoDeDatos.SetActive(true);
            }
        }

        public void SetData()
        {
            if (nombre && nombre.text != null)
            {
                userData.fullName = nombre.text;
            }

            if (cedula && cedula.text != null)
            {
                userData.id = cedula.text;
            }

            if(mail && mail.text != null)
            {
                userData.mail = mail.text;
            }

            if(phone && phone.text!= null)
            {
                userData.phone = phone.text;
            }

            if(address && address.text != null)
            {
                userData.address = address.text;
            }

            if (ingresoDeDatos)
            {
                ingresoDeDatos.SetActive(false);
            }

            if (seleccionDeModo)
            {
                seleccionDeModo.SetActive(true);
            }

            userData.sede = sedeDropdown.options[sedeDropdown.value].text;
        }

        private void onFailed(Error obj)
        {
            print("Failed");
        }

        private void onComplete(UserData.CreateUser obj)
        {
                print("Succed");
        }

        public void StartExperience()
        {
            //CreateUser
            //if (settings.experienMode == ExperienMode.Evaluacion)
            //{
                userData.CreateUserData();
                WebServiceManager.SendJsonData<UserData.CreateUser>(userData.serverUrl + "/api/user/create", userData.createUser, string.Empty, onComplete, onFailed);
            //}
            
            ScenesManager.instance.StartLoadScene(ScenesManager.instance.tutorialScene);
        }

        #endregion
    }

    [Serializable]
    public class Sedes
    {
        public List<Sede> sedes = null;
    }

    [Serializable]
    public class Sede
    {
        public string name;
    }

    [Serializable]
    public class Section
    {
        public Button[] uniform;
        public Button[] reception;
        public Button[] ubication;
        public Button[] picking;
        public Button[] packing;
    }
}