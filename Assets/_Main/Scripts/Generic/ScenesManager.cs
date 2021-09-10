using System;
using System.Collections;
using DreamHouseSpectra.Networking;
using DreamHouseSpectra.Networking.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ScenesManager : MonoBehaviour
    {
        #region Components

        public static ScenesManager instance;

        [Space(10f), Header("Components")] [SerializeField]
        private KeyCode reloadKey = KeyCode.Escape;

        [SerializeField] private KeyCode reloadSceneKey = KeyCode.R;
        [SerializeField] private SpectraUISettings settings = null;
        [SerializeField] public UserData userData = null;
        [SerializeField] private GameObject ui = null;
        [SerializeField] private Chronometer chronometer = null;
        [SerializeField] private GameObject LoadingBarObj;
        [SerializeField] private TMPro.TextMeshProUGUI loadingText;
        [SerializeField] private Image LoadingBar;

        [Space(10f), Header("Escenas")] public string menuScene = "Intro";
        public string tutorialScene = "TutorialControls";
        public SectionInfo eppScene = null;
        public SectionInfo receptionScene = null;
        public SectionInfo locationScene = null;
        public SectionInfo pickingScene = null;
        public SectionInfo packingScene = null;

        public string emptyScene = "Empty Scene";
        public string endScene = "Cierre";

        [Space(10f), Header("Buttons")] [SerializeField]
        private Section section = null;

        [SerializeField] private Color normalColor = Color.black;

        [Space(10f), Header("Menus")] [SerializeField]
        private GameObject secitonButtons;

        [SerializeField] private GameObject progressUi;
        [SerializeField] private GameObject continueButton;

        private Color disabledColor = Color.white;

        private string mainMenu = "Main Menu";
        private IEnumerator loadScene;
        private IEnumerator reloadScene;
        private IEnumerator sendProgress;

        private WaitForEndOfFrame waitForEndOfFrame;
        private WaitForSeconds waitForScene;
        private WaitForSeconds twentySeconds;

        private bool loadingScene = false;
        private bool killOnDestroy = true;

        public GameObject buttonPause;
        public bool canSendData;

        [System.Serializable]
        public class SectionInfo
        {
            public string sceneName;
            public bool visited = false;
        }

        public bool isLoadingScene
        {
            get { return loadingScene; }
        }

        #endregion Components

        #region Unity Functions

        private void OnDestroy()
        {
            killOnDestroy = false;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            waitForScene = new WaitForSeconds(2f);
            waitForEndOfFrame = new WaitForEndOfFrame();
            twentySeconds = new WaitForSeconds(20f);
            canSendData = false;
        }

        private void Start()
        {
            Init();
            userData.Init();
            ExperienceUI.instance.HardResetProgress();
            chronometer.ResetSetionChronometer();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == eppScene.sceneName)
                settings.startPoint = StartPoint.Uniforme;
            else if (SceneManager.GetActiveScene().name == receptionScene.sceneName)
                settings.startPoint = StartPoint.Recepcion;
            else if (SceneManager.GetActiveScene().name == locationScene.sceneName)
                settings.startPoint = StartPoint.Ubicacion;
            else if (SceneManager.GetActiveScene().name == pickingScene.sceneName)
                settings.startPoint = StartPoint.Picking;
            else if (SceneManager.GetActiveScene().name == packingScene.sceneName)
                settings.startPoint = StartPoint.Packing;

            if (Input.GetKeyDown(reloadKey) && SceneManager.GetActiveScene().name != mainMenu)
            {
                if (loadingScene)
                {
                    return;
                }

                StartLoadScene(menuScene);
            }

            if (Input.GetKeyDown(reloadSceneKey) && SceneManager.GetActiveScene().name != mainMenu)
            {
                if (loadingScene)
                {
                    return;
                }

                StartReloadScene();
            }

            CheckScene();

            if (loadingScene)
            {
                LoadingBarObj.SetActive(true);
                ui.SetActive(false);
                if (asyncLoad != null)
                {
                    float progress = Mathf.Lerp(LoadingBar.fillAmount, asyncLoad.progress, Time.deltaTime * 8f);
                    float textProgress = progress * 100f;
                    LoadingBar.fillAmount = progress;
                    loadingText.text = textProgress.ToString("f0") + "%";
                }
            }
            else
            {
                //LoadingBar.fillAmount = 0;
                LoadingBarObj.SetActive(false);

                if (SceneManager.GetActiveScene().name == menuScene || SceneManager.GetActiveScene().name == mainMenu)
                    ui.SetActive(false);
                else
                    ui.SetActive(true);
            }
        }

        #endregion Unity Functions

        #region Functions

        public void Init()
        {
            eppScene.visited = false;
            receptionScene.visited = false;
            locationScene.visited = false;
            pickingScene.visited = false;
            packingScene.visited = false;

            if (SceneManager.GetActiveScene().name == mainMenu || SceneManager.GetActiveScene().name == menuScene)
                ui.SetActive(false);
            else
                ui.SetActive(true);

            for (int i = 0; i < section.uniform.Length; i++)
            {
                section.uniform[i].interactable = true;
                section.reception[i].interactable = true;
                section.ubication[i].interactable = true;
                section.picking[i].interactable = true;
                section.packing[i].interactable = true;

                section.uniform[2].image.color = normalColor;
                section.reception[2].image.color = normalColor;
                section.ubication[2].image.color = normalColor;
                section.picking[2].image.color = normalColor;
                section.packing[2].image.color = normalColor;
            }
        }

        public void StartReloadScene()
        {
            if (reloadScene != null)
                StopCoroutine(reloadScene);

            reloadScene = ReloadScene(SceneManager.GetActiveScene().name);
            StartCoroutine(reloadScene);
        }

        public void ReloadExperience()
        {
            //userData.Init();
            StartLoadScene(menuScene);
        }

        public void CheckScene()
        {
            userData.eppsProgress = ExperienceUI.instance.eppProgress;
            userData.receptionProgress = ExperienceUI.instance.receptionProgress;
            userData.locationProgress = ExperienceUI.instance.locationProgress;
            userData.pickingProgress = ExperienceUI.instance.pickingProgress;
            userData.packingProgress = ExperienceUI.instance.packingProgress;

            SendTime();

            if (!eppScene.visited && SceneManager.GetActiveScene().name == eppScene.sceneName)
            {
                settings.startPoint = StartPoint.Uniforme;
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
                for (int i = 0; i < section.uniform.Length; i++)
                {
                    section.uniform[i].interactable = false;

                    section.uniform[2].image.color = disabledColor;
                }

                eppScene.visited = true;
                chronometer.ResetSetionChronometer();
            }
            else if (!receptionScene.visited && SceneManager.GetActiveScene().name == receptionScene.sceneName)
            {
                settings.startPoint = StartPoint.Recepcion;
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
                for (int i = 0; i < section.reception.Length; i++)
                {
                    section.reception[i].interactable = false;

                    section.reception[2].image.color = disabledColor;
                }

                receptionScene.visited = true;
                chronometer.ResetSetionChronometer();
            }
            else if (!locationScene.visited && SceneManager.GetActiveScene().name == locationScene.sceneName)
            {
                settings.startPoint = StartPoint.Ubicacion;
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
                for (int i = 0; i < section.ubication.Length; i++)
                {
                    section.ubication[i].interactable = false;

                    section.ubication[2].image.color = disabledColor;
                }

                locationScene.visited = true;
                chronometer.ResetSetionChronometer();
            }
            else if (!pickingScene.visited && SceneManager.GetActiveScene().name == pickingScene.sceneName)
            {
                settings.startPoint = StartPoint.Picking;
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
                for (int i = 0; i < section.picking.Length; i++)
                {
                    section.picking[i].interactable = false;

                    section.picking[2].image.color = disabledColor;
                }

                pickingScene.visited = true;
                chronometer.ResetSetionChronometer();
            }
            else if (!packingScene.visited && SceneManager.GetActiveScene().name == packingScene.sceneName)
            {
                settings.startPoint = StartPoint.Packing;
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
                for (int i = 0; i < section.packing.Length; i++)
                {
                    section.packing[i].interactable = false;

                    section.packing[2].image.color = disabledColor;
                }

                packingScene.visited = true;
                chronometer.ResetSetionChronometer();
            }

            if (SceneManager.GetActiveScene().name == tutorialScene)
            {
                secitonButtons.SetActive(false);
                progressUi.SetActive(false);
                continueButton.SetActive(true);
            }
            else
            {
                secitonButtons.SetActive(true);
                progressUi.SetActive(true);
                continueButton.SetActive(false);
            }
        }

        private void onFailed(Error obj)
        {
        }

        private void onComplete(UserData.ModuleAdvance obj)
        {
        }

        private void onComplete(UserData.ReportUser obj)
        {
        }

        private void onComplete(UserData.ModuleGrade obj)
        {
        }

        private void onComplete(UserData.ModuleReset obj)
        {
        }

        public void SendTime()
        {
            switch (settings.startPoint)
            {
                case StartPoint.Uniforme:
                    userData.eppTime = chronometer.backendChronometer;
                    break;

                case StartPoint.Recepcion:
                    userData.receptionTime = chronometer.backendChronometer;
                    break;

                case StartPoint.Ubicacion:
                    userData.locationTime = chronometer.backendChronometer;
                    break;

                case StartPoint.Picking:
                    userData.pickingTime = chronometer.backendChronometer;
                    break;

                case StartPoint.Packing:
                    userData.packingTime = chronometer.backendChronometer;
                    break;
            }
        }

        public void StartLoadScene(string sceneName)
        {
            if (GenericReportBackend.Instance != null)
            {
                GenericReportBackend.Instance.LaunchCreateReport();
            }
            if (loadScene != null)
                StopCoroutine(loadScene);
            loadScene = LoadScene(sceneName);
            StartCoroutine(loadScene);
        }

        /*private void StartSendProgress()
        {
            if (sendProgress != null)
                StopCoroutine(sendProgress);

            sendProgress = SendProgress();
            StartCoroutine(sendProgress);
        }

        private IEnumerator SendProgress()
        {
            yield return waitForEndOfFrame;

            while (killOnDestroy)
            {
                yield return twentySeconds;

                if (SceneManager.GetActiveScene().name == eppScene.sceneName ||
                    SceneManager.GetActiveScene().name == receptionScene.sceneName ||
                    SceneManager.GetActiveScene().name == locationScene.sceneName ||
                    SceneManager.GetActiveScene().name == pickingScene.sceneName ||
                    SceneManager.GetActiveScene().name == packingScene.sceneName)
                {
                    if (canSendData)
                    {
                        Debug.Break();
                        canSendData = false;
                        if (settings.experienMode == ExperienMode.Evaluacion)
                        {
                            userData.moduleGrade.fechaFinalEvaluacion = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                            userData.SendGrade();
                            WebServiceManager.SendJsonData<UserData.ModuleAdvance>(userData.serverUrl + "/api/informes/sendGrade", userData.moduleGrade, string.Empty, onComplete, onFailed);
                        }
                        else 
                        {
                            userData.moduleAdvance.fechaFinalEntrenamiento = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                            userData.SendModuleAdvance();
                            WebServiceManager.SendJsonData<UserData.ModuleAdvance>(userData.serverUrl + "/api/informes/sendAdvance", userData.moduleAdvance, string.Empty, onComplete, onFailed);
                        }
                    }
                }
            }
        }*/

        private AsyncOperation asyncUnload;
        private AsyncOperation asyncEmptySceneLoad;
        private AsyncOperation asyncEmptySceneUnload;
        private AsyncOperation asyncLoad;
        private float totalLoadProgress;

        private IEnumerator LoadScene(string sceneName)
        {
            if (SendReportBackEnd.Instance != null &&
                settings.experienMode == ExperienMode.Evaluacion)
            {
                yield return waitForEndOfFrame;
                userData.CreateReport();
                WebServiceManager.SendJsonData<UserData.ReportUser>(userData.serverUrl + "/api/informes/fallosUsuario",
                    userData.reportUser, string.Empty, onComplete, onFailed);
            }

            asyncLoad = null;
            LoadingBar.fillAmount = 0;
            loadingText.text = "";
            yield return waitForEndOfFrame;

            loadingScene = true;

            yield return waitForEndOfFrame;

            if ( /*settings.experienMode == ExperienMode.Evaluacion &&*/ SceneManager.GetActiveScene().name != endScene)
            {
                //userData.ResetModule();
                yield return waitForEndOfFrame;
                //ResetModule();
                yield return new WaitForSeconds(1f);

                if (canSendData)
                {
                    canSendData = false;
                    if (settings.experienMode == ExperienMode.Entrenamiento)
                    {
                        userData.moduleAdvance.fechaFinalEntrenamiento = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                        userData.SendModuleAdvance();
                        WebServiceManager.SendJsonData<UserData.ModuleAdvance>(
                            userData.serverUrl + "/api/informes/sendAdvance", userData.moduleAdvance, string.Empty,
                            onComplete, onFailed);
                    }
                    else
                    {
                        userData.moduleGrade.fechaFinalEvaluacion = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                        userData.SendGrade();
                        WebServiceManager.SendJsonData<UserData.ModuleGrade>(
                            userData.serverUrl + "/api/informes/sendGrade", userData.moduleGrade, String.Empty,
                            onComplete, onFailed);
                    }

                    //yield return new WaitForSeconds(1f);
                }

                yield return new WaitForSeconds(3f);
            }

            asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            yield return asyncUnload;

            asyncEmptySceneLoad = SceneManager.LoadSceneAsync(emptyScene);
            yield return asyncEmptySceneLoad;

            asyncEmptySceneUnload = SceneManager.UnloadSceneAsync(emptyScene);
            yield return asyncEmptySceneUnload;

            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            yield return asyncLoad;

            if (SceneManager.GetActiveScene().name == menuScene || SceneManager.GetActiveScene().name == mainMenu)
            {
                yield return new WaitForEndOfFrame();
                userData.Init();
                Init();
                //ExperienceUI.instance.finalProgress = 0;
                yield return new WaitForSeconds(1f);
                ExperienceUI.instance.HardResetProgress();
                chronometer.ResetSetionChronometer();
                yield return waitForEndOfFrame;
            }

            if (SceneManager.GetActiveScene().name == menuScene || SceneManager.GetActiveScene().name == mainMenu)
                ui.SetActive(false);
            else
                ui.SetActive(true);

            if (SceneManager.GetActiveScene().name == tutorialScene)
            {
                buttonPause.SetActive(false);
                ExperienceUI.instance.HardResetProgress();
                chronometer.ResetSetionChronometer();
            }
            else
            {
                buttonPause.SetActive(true);
            }

            yield return waitForScene;
            loadingScene = false;

            yield return waitForEndOfFrame;
        }

        private IEnumerator ReloadScene(string sceneName)
        {
            asyncLoad = null;
            LoadingBar.fillAmount = 0;
            loadingText.text = "";
            yield return waitForEndOfFrame;

            loadingScene = true;

            yield return waitForEndOfFrame;
            asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            yield return asyncUnload;

            asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            yield return asyncLoad;

            loadingScene = false;

            //if (settings.experienMode == ExperienMode.Evaluacion)
            //{
            //userData.ResetModule();
            yield return waitForEndOfFrame;
            //ResetModule();
            //}

            yield return waitForEndOfFrame;
        }

        /*void ResetModule()
        {
            
            if (settings.experienMode == ExperienMode.Entrenamiento)
            {
                WebServiceManager.SendJsonData<UserData.ModuleReset>(userData.serverUrl + "/api/informes/resetModuleEntrenamiento", userData.moduleReset, String.Empty, onComplete, onFailed);

            }
            else
            {
                WebServiceManager.SendJsonData<UserData.ModuleReset>(userData.serverUrl + "/api/informes/resetModuleEvaluacion", userData.moduleReset, String.Empty, onComplete, onFailed);
            }
        }*/

        #endregion Functions

        #region Section Buttons

        public void ContinueButton()
        {
            if (loadingScene) return;

            if (settings.startPoint == StartPoint.Uniforme)
                StartLoadScene(eppScene.sceneName);
            else if (settings.startPoint == StartPoint.Recepcion)
                StartLoadScene(receptionScene.sceneName);
            else if (settings.startPoint == StartPoint.Ubicacion)
                StartLoadScene(locationScene.sceneName);
            else if (settings.startPoint == StartPoint.Picking)
                StartLoadScene(pickingScene.sceneName);
            else if (settings.startPoint == StartPoint.Packing)
                StartLoadScene(packingScene.sceneName);
        }

        public void EppButton()
        {
            if (GenericReportBackend.Instance != null)
            {
                GenericReportBackend.Instance.LaunchCreateReport();
            }
            if (loadingScene) return;
            //settings.startPoint = StartPoint.Uniforme;
            StartLoadScene(eppScene.sceneName);
        }

        public void Receptionutton()
        {
           
            if (loadingScene) return;
            StartLoadScene(receptionScene.sceneName);
        }

        public void LocationButton()
        {
           
            if (loadingScene) return;
            StartLoadScene(locationScene.sceneName);
        }

        public void PickingButton()
        {
            
            if (loadingScene) return;
            StartLoadScene(pickingScene.sceneName);
        }

        public void PackingButton()
        {
           
            if (loadingScene) return;
            StartLoadScene(packingScene.sceneName);
        }

        #endregion Section Buttons
    }
}