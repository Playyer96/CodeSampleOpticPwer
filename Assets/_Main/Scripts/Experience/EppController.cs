using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class EppController : MonoBehaviour
    {
        #region Components

        [Space(10f), Header("Settings")] [SerializeField]
        SpectraUISettings settings = null;

        [Space(10f), Header("Player Components")] [SerializeField]
        Transform player = null;

        [SerializeField] Transform head = null;
        [SerializeField] Vector3 offset = new Vector3(0, 0, 0);
        [SerializeField] CapsuleCollider capsuleCollider = null;

        [Space(10f), Header("Player Positions")] [SerializeField]
        Transform initialPos = null;

        [SerializeField] Transform uniformPos = null;

        [Space(10f), Header("Epp Items")] [SerializeField]
        int totalItemsToTrack = 0;

        [SerializeField] WorkItems workItems = null;
        [SerializeField] ItemID[] uniform = null;
        [SerializeField] ToolsetEquipment toolsetEquipment = null;
        [SerializeField] Toolset toolsetController = null;
        [SerializeField] List<GameObject> eppColliders = new List<GameObject>();

        [Space(10f), Header("Sound FX")] [SerializeField]
        private AudioManager _audioManager;

        [SerializeField] private StudioEventEmitter teleportSound;

        public FMOD.Studio.ParameterInstance parameterEvent;

        [Space(10f), Header("Extra Components")] [SerializeField]
        UnityEvent swapGloves;

        [SerializeField] GameObject lockedShelf;

        bool glovesActive = false;

        [System.Serializable]
        public class WorkItems
        {
            public ItemID pocket;
            public ItemID gloves;
            public ItemID id;
            public ItemID printer;
        }

        [System.Serializable]
        public class Uniform
        {
            public ItemID Hat;
            public ItemID shirt;
            public ItemID jeans;
            public ItemID boots;
        }

        float progress = 0f;
        float totalProgress = 0f;
        bool alredyVisited = false;
        bool toolsReady = false;
        bool uniformIsOn = false;
        bool loadReception = false;

        IEnumerator checkIfCompleted;
        IEnumerator allToolsetPick;
        IEnumerator changeSceneOnTrainingMode;

        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds twoSeconds;
        WaitForSeconds twentySeconds;

        public float Progress
        {
            get { return progress; }
        }

        public bool visited
        {
            get { return alredyVisited; }
        }

        public bool uniformReady
        {
            get { return uniformIsOn; }
        }

        public bool toolsSet
        {
            get { return toolsReady; }
        }

        public float TotalProgress
        {
            get { return totalProgress; }
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
            twoSeconds = new WaitForSeconds(2f);
            twentySeconds = new WaitForSeconds(20f);
            capsuleCollider = GetComponent<CapsuleCollider>();

            if (toolsetEquipment) toolsetEquipment.CheckForToolSet();
        }

        private void Start()
        {
            PlayerTeleport(initialPos);

            alredyVisited = true;
            CheckingIfComplete();
            ToolsSetChecking();

            if (settings.experienMode == ExperienMode.Entrenamiento)
            {
                if (lockedShelf)
                    lockedShelf.SetActive(true);

                for (int i = 0; i < eppColliders.Count; i++)
                {
                    eppColliders[i].GetComponent<Rigidbody>().useGravity = false;
                    eppColliders[i].GetComponent<Rigidbody>().isKinematic = true;
                    eppColliders[i].GetComponent<Collider>().enabled = false;
                }
            }
            else
            {
                if (lockedShelf)
                    lockedShelf.SetActive(false);
            }

            if (!toolsetEquipment.IsSet)
            {
                toolsetController.toolset.SetActive(false);
                toolsetController.splitter.SetActive(false);
                toolsetController.marker.SetActive(false);
                toolsetController.pen.SetActive(false);
                toolsetController.pen.SetActive(false);
                toolsetController.pocket.SetActive(false);
            }

            if (!workItems.id.isSet)
                toolsetController.id.SetActive(false);

            if (settings.experienMode == ExperienMode.Entrenamiento)
            {
                for (int i = 0; i < uniform.Length; i++)
                {
                    uniform[i].gameObject.GetComponent<Collider>().enabled = false;
                }
            }

            ExperienceUI.instance.eppProgress = 0;
        }


        private void OnDestroy()
        {
            StopCheckers();
        }

        void FixedUpdate()
        {
            FollowHead();
            totalProgress = progress;
            
            ExperienceUI.instance.eppProgress = totalProgress;

            if (settings.experienMode == ExperienMode.Entrenamiento && Checklist.Get("Epps", "Can Interact"))
            {
                for (int i = 0; i < uniform.Length; i++)
                {
                    uniform[i].gameObject.GetComponent<Collider>().enabled = true;
                }
            }

            
            if (totalProgress >= 99.5f && settings.experienMode == ExperienMode.Entrenamiento)
                Checklist.Set("Epps", "Complete", true);

            if (totalProgress >= 99.5f && !ScenesManager.instance.receptionScene.visited)
            {
                totalProgress = Mathf.Round(progress);
            }

            /*
            if (settings.experienMode == ExperienMode.Evaluacion && !loadReception)
            { 
                    ScenesManager.instance.StartLoadScene(ScenesManager.instance.receptionScene.sceneName);
                    loadReception = true;
                }
            }
            */
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("EPP"))
            {
                if (other.GetComponent<ItemID>())
                {
                    if (other.GetComponent<ItemID>().progressSet == false)
                    {
                        ProgressCounter();
                        other.GetComponent<ItemID>().progressSet = true;
                        switch (other.GetComponent<ItemID>().id)
                        {
                            case "Casco":
                                _audioManager.SetAudioEvent(0, "GrabNDrop", 2);
                                break;
                            case "Botas":
                                _audioManager.SetAudioEvent(1, "GrabNDrop", 2);
                                break;
                            case "Camisa":
                                _audioManager.SetAudioEvent(2, "GrabNDrop", 2);
                                break;
                            case "Jeans":
                                _audioManager.SetAudioEvent(3, "GrabNDrop", 2);
                                break;
                            case "Gloves":
                                _audioManager.SetAudio("GrabNDrop", 0, 2);
                                break;
                            case "Printer":
                                _audioManager.SetAudio("GrabNDrop", 1, 2);
                                break;
                            case "Pocket":
                                _audioManager.SetAudio("GrabNDrop", 2, 2);
                                break;
                            case "ID":
                                _audioManager.SetAudio("GrabNDrop", 3, 2);
                                break;
                        }
                    }

                    other.GetComponent<ItemID>().isSet = true;

                    if (other.GetComponent<MeshRenderer>())
                    {
                        other.GetComponent<MeshRenderer>().enabled = false;
                        other.GetComponent<ItemID>().isSet = true;
                    }
                    else
                    {
                        MeshRenderer[] meshRenderers = other.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < meshRenderers.Length; i++)
                        {
                            meshRenderers[i].enabled = false;
                        }
                    }
                }
                else if (other.GetComponent<ToolsetEquipment>())
                {
                    if (other.GetComponent<ToolsetEquipment>().IsSet)
                    {
                        if (other.GetComponent<ToolsetEquipment>().progressIsSet == false)
                        {
                            if (settings.experienMode == ExperienMode.Entrenamiento)
                            {
                                if (!workItems.gloves.isSet || !workItems.id.isSet ||
                                    !toolsetEquipment.IsSet || !workItems.pocket)
                                {
                                    Checklist.Set("Cartuchera", "FaltanElementos", true);
                                }else
                                    Checklist.Set("Cartuchera", "Adhiere", true);
                            }

                            _audioManager.SetAudio("GrabNDrop", 4, 2);
                            other.GetComponent<ToolsetEquipment>().progressIsSet = true;
                            ProgressCounter();
                        }

                        MeshRenderer[] meshRenderers = other.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < meshRenderers.Length; i++)
                        {
                            meshRenderers[i].enabled = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Functions

        private void PlayerTeleport(Transform _transform)
        {
            PlayTeleportSound();
            
            player.SetPositionAndRotation(
                new Vector3(_transform.position.x, _transform.position.y, _transform.position.z),
                new Quaternion(player.rotation.x, player.rotation.y, player.rotation.z, 1));
        }

        private void PlayTeleportSound()
        {
            if (teleportSound)
            {
                if (teleportSound.IsPlaying())
                    teleportSound.Stop();

                teleportSound.Play();
            }
        }

        private void ProgressCounter()
        {
            progress += (100f / totalItemsToTrack);
        }

        private void FollowHead()
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                head.transform.position.x + offset.x,
                head.transform.position.y + offset.y, head.transform.position.z + offset.z), 5f);
        }

        private void CheckingIfComplete()
        {
            if (checkIfCompleted != null)
                StopCoroutine(checkIfCompleted);

            checkIfCompleted = CheckIfCompleted();
            StartCoroutine(checkIfCompleted);
        }

        private void ToolsSetChecking()
        {
            if (allToolsetPick != null)
                StopCoroutine(allToolsetPick);

            allToolsetPick = AllToolsetPick();
            StartCoroutine(allToolsetPick);
        }

        private void StopCheckers()
        {
            if (checkIfCompleted != null)
                StopCoroutine(checkIfCompleted);

            if (allToolsetPick != null)
                StopCoroutine(allToolsetPick);
        }

        public void StartChangeSceneOnTrainingMode()
        {
            if (changeSceneOnTrainingMode != null)
                StopCoroutine(changeSceneOnTrainingMode);

            changeSceneOnTrainingMode = ChangeSceneOnTrainingMode();
            StartCoroutine(changeSceneOnTrainingMode);
        }

        IEnumerator ChangeSceneOnTrainingMode()
        {
            yield return waitForEndOfFrame;
            if (totalProgress >= 99.5f && !ScenesManager.instance.receptionScene.visited)
            {
                totalProgress = Mathf.Round(progress);
                yield return new WaitForSeconds(12f);
                ScenesManager.instance.StartLoadScene(ScenesManager.instance.receptionScene.sceneName);
            }
        }

        IEnumerator CheckIfCompleted()
        {
            yield return waitForEndOfFrame;
            bool killOnCompleted = true;
            while (killOnCompleted)
            {
                yield return twoSeconds;
                if (uniform.All(parts => parts.GetComponent<ItemID>().isSet))
                {
                    uniformIsOn = true;

                    lockedShelf.SetActive(false);

                    for (int i = 0; i < eppColliders.Count; i++)
                    {
                        eppColliders[i].GetComponent<Rigidbody>().useGravity = true;
                        eppColliders[i].GetComponent<Rigidbody>().isKinematic = false;
                        eppColliders[i].GetComponent<Collider>().enabled = true;
                    }

                    if (settings.experienMode == ExperienMode.Entrenamiento)
                        Checklist.Set("Epps", "Uniforme", true);
                    PlayerTeleport(uniformPos);
                    if (uniformIsOn) killOnCompleted = false;
                }
            }
        }

        IEnumerator AllToolsetPick()
        {
            yield return waitForEndOfFrame;
            bool killOnCompleted = true;
            while (killOnCompleted)
            {
                yield return waitForEndOfFrame;

                yield return twoSeconds;

                if (workItems.gloves.isSet)
                {
                    if (glovesActive == false)
                    {
                        swapGloves.Invoke();
                        glovesActive = true;
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (toolsetEquipment.IsSet)
                {
                    toolsetController.toolset.SetActive(true);
                    toolsetController.splitter.SetActive(true);
                    toolsetController.marker.SetActive(true);
                    toolsetController.pen.SetActive(true);
                }

                if (workItems.pocket.isSet)
                    toolsetController.pocket.SetActive(true);

                if (workItems.id.isSet)
                    toolsetController.id.SetActive(true);


                if (workItems.gloves.isSet && workItems.id.isSet &&
                    toolsetEquipment.IsSet && workItems.pocket)
                {
                    killOnCompleted = false;
                    toolsReady = true;
                }
            }

            #endregion
        }
    }
}