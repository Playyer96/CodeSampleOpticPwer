using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class CameraShooter : MonoBehaviour
    {

        #region Components
        public event Action OnAttachedToHand;

        [SerializeField] KeyCode key;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Image photo;

        [SerializeField] Interactable interactable;
        [SerializeField] SteamVR_Action_Boolean shootAction;
        [SerializeField] StudioEventEmitter eventEmitter;

        IEnumerator takePhoto;
        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds waitForSeconds;

        /*public Transform t_Out;
        public Transform t_Interior;
        public UnityEvent e_OnPhotoOut;
        public UnityEvent e_OnPhotoInterior;*/

        private bool isOnHand;
        #endregion

        #region Unity Functions
        private void OnDestroy()
        {
            eventEmitter.Stop();
        }

        private void Start()
        {
            /*if (e_OnPhotoOut == null)
            {
                e_OnPhotoOut = new UnityEvent();
                e_OnPhotoOut.AddListener(OnPhotoOut);
            }
            if (e_OnPhotoInterior == null)
            {
                e_OnPhotoInterior = new UnityEvent();
                e_OnPhotoInterior.AddListener(OnPhotoInterior);
            }*/
            Init();

        }

        void Update()
        {
#if UNITY_EDITOR
            DebugInput();
#endif
        }
        #endregion

        #region Functions
        void Init()
        {
            shootAction.AddOnStateDownListener(StateDown, SteamVR_Input_Sources.LeftHand);
            shootAction.AddOnStateDownListener(StateDown, SteamVR_Input_Sources.RightHand);

            shootAction.AddOnStateUpListener(StateUp, SteamVR_Input_Sources.LeftHand);
            shootAction.AddOnStateUpListener(StateUp, SteamVR_Input_Sources.RightHand);

            waitForEndOfFrame = new WaitForEndOfFrame();
            waitForSeconds = new WaitForSeconds(1f);
        }

        public void OnPhotoInterior()
        {

        }

        public void OnPhotoOut()
        {

        }

        private void StateDown(SteamVR_Action_Boolean fromAciton, SteamVR_Input_Sources fromSource)
        {
            if (!isOnHand || fromSource != interactable.attachedToHand.handType)
                return;

        }
        private void StateUp(SteamVR_Action_Boolean fromAciton, SteamVR_Input_Sources fromSource)
        {
            if (!isOnHand || fromSource != interactable.attachedToHand.handType)
                return;

            StartTakePhoto();
        }

        public void SetOnHand()
        {
            if (OnAttachedToHand != null) OnAttachedToHand();

            isOnHand = true;
        }

        public void SetDetachHand()
        {
            isOnHand = false;
        }

        private void DebugInput()
        {
            if (Input.GetKeyDown(key))
            {
                StartTakePhoto();
            }
        }

        public void StartTakePhoto()
        {
            if (takePhoto != null)
                StopCoroutine(takePhoto);

            takePhoto = TakePhoto();
            StartCoroutine(takePhoto);
        }

        IEnumerator TakePhoto()
        {
            yield return waitForEndOfFrame;
            Debug.Log("<color=blue>Taking the picture</color>");
            eventEmitter.Play();

            /*if (CameraPhotoBH.Instance.GetTarget() == t_Out)
            {
                e_OnPhotoOut.Invoke();
            }
            if (CameraPhotoBH.Instance.GetTarget() == t_Interior)
            {
                e_OnPhotoInterior.Invoke();
            }*/

            photo.sprite = CameraPhotoBH.Instance.TakePicture(cameraTransform);
            yield return waitForSeconds;
        }
        #endregion
    }
}