using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using FMODUnity;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ControlsTutorial : Tutorials
    {
        #region Components

        [SerializeField] private AudioManager aAudio;
        [SerializeField] private Teleport teleports;
        [SerializeField] private ObjectsLogic objectsLogic;
        [SerializeField] private StudioEventEmitter popupSound;
        [SerializeField] private GameObject progressCounter;
        [SerializeField] private GameObject buttonTutorial;
        [SerializeField] private Image progressFill;
        [SerializeField] private TMPro.TextMeshProUGUI progressText;

        private IEnumerator _counterActivation;
        private IEnumerator _tutorial;
        private IEnumerator _firstInteraction;
        private IEnumerator _tutorialEnd;

        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _oneSecond;
        private WaitForSeconds _eightSeconds;
        private WaitForSeconds _fifteenSeconds;

        #endregion

        #region Unity Functions

        public override void Start()
        {
            base.Start();
            progressCounter.SetActive(false);
            buttonTutorial.SetActive(false);

            if (teleports)
                teleports.ActivateTeleports(false);

            _waitForEndOfFrame = new WaitForEndOfFrame();
            _oneSecond = new WaitForSeconds(1f);
            _eightSeconds = new WaitForSeconds(8f);
            _fifteenSeconds = new WaitForSeconds(15f);

            DeactivateInteractors();
            StartFirstInteraction();
        }

        private void Update()
        {
            fakeProgress += (Time.deltaTime * 6f);
            if (fakeProgress >= 100f)
            {
                fakeProgress = 0;
            }

            progressFill.fillAmount = fakeProgress / 100f;
            progressText.text = fakeProgress.ToString("f2") + "%";
        }

        #endregion

        #region Functions

        private void DeactivateInteractors()
        {
            objectsLogic.SetObjects(-1, -1);
        }

        private void CheckFirstActiveInteractors()
        {
            objectsLogic.SetObjects(1, 1);
        }

        public void AcvitateInteractors()
        {
            objectsLogic.SetObjects(0, 3);
        }

        public void StartTutorials(int index)
        {
            if (_tutorial != null)
                StopCoroutine(_tutorial);

            _tutorial = Tutorial(index);
            StartCoroutine(_tutorial);
        }

        public void StartFirstInteraction()
        {
            if (_firstInteraction != null)
                StopCoroutine(_firstInteraction);

            _firstInteraction = FirstInteraction();
            StartCoroutine(_firstInteraction);
        }

        public void StartEndTutorial()
        {
            if (_tutorialEnd != null)
                StopCoroutine(_tutorialEnd);

            _tutorialEnd = TutorialEnd();
            StartCoroutine(_tutorialEnd);
        }

        private void StartCounterActivation()
        {
            if (_counterActivation != null)
                StopCoroutine(_counterActivation);

            _counterActivation = CounterActivation();
            StartCoroutine(_counterActivation);
        }

        private IEnumerator FirstInteraction()
        {
            yield return _waitForEndOfFrame;
            StartTutorials(0);
            yield return _fifteenSeconds;
            StartTutorials(1);
            CheckFirstActiveInteractors();
        }

        private IEnumerator TutorialEnd()
        {
            yield return _waitForEndOfFrame;
            StartTutorials(6);
            yield return _eightSeconds;
            StartTutorials(7);
            yield return _eightSeconds;
            StartTutorials(8);
            yield return _eightSeconds;
            StartTutorials(9);
            yield return _eightSeconds;
            StartTutorials(5);
        }

        float fakeProgress = 0f;

        private IEnumerator CounterActivation()
        {
            fakeProgress += Time.deltaTime * 2f;
            progressCounter.transform.localScale = Vector3.zero;
            yield return _waitForEndOfFrame;
            progressCounter.SetActive(true);
            LeanTween.scale(progressCounter, new Vector3(0.005f, 0.005f, 0.005f), 0.45f)
                .setEase(LeanTweenType.easeOutBounce);

            yield return _eightSeconds;
            LeanTween.scale(progressCounter, new Vector3(0f, 0f, 0f), 0.45f).setEase(LeanTweenType.easeOutBounce);
            yield return _oneSecond;
            progressCounter.SetActive(false);
            yield return _waitForEndOfFrame;
        }

        private IEnumerator ButtonActivation()
        {
            buttonTutorial.transform.localScale = Vector3.zero;
            yield return _waitForEndOfFrame;
            buttonTutorial.SetActive(true);
            LeanTween.scale(buttonTutorial, new Vector3(0.0005f, 0.0005f, 0.01f), 0.45f)
                .setEase(LeanTweenType.easeOutBounce);

            yield return _eightSeconds;
            LeanTween.scale(buttonTutorial, new Vector3(0f, 0f, 0f), 0.45f).setEase(LeanTweenType.easeOutBounce);
            yield return _oneSecond;
            buttonTutorial.SetActive(false);
            yield return _waitForEndOfFrame;
        }

        private IEnumerator Tutorial(int index)
        {
            canvasManager.isFollow = false;
            canvasManager.isAnim = false;
            StartCoroutine(canvasManager.SetPopUpImage(-1));

            if (popupSound)
            {
                if (popupSound.IsPlaying())
                    popupSound.Stop();

                popupSound.Play();
            }

            switch (index)
            {
                case -1:
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    break;
                case 0:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 0, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    aAudio.SetAudio(0, 0);
                    break;
                case 1:
                    canvasManager.isAnim = true;
                    canvasManager.isFollow = true;
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    switch (detectControllers.lastEnabledHandler)
                    {
                        case DetectControllers.HandsHandler.HI5:
                            yield return StartCoroutine(canvasManager.SetPopUp(1, 0, 0));
                            StartCoroutine(canvasManager.SetIcono(4, 0));
                            StartCoroutine(canvasManager.FollowTransform(0));
                            break;
                        case DetectControllers.HandsHandler.Vive_Controller:
                            yield return StartCoroutine(canvasManager.SetPopUp(2, 0, 0));
                            StartCoroutine(canvasManager.SetIcono(4, 0));
                            StartCoroutine(canvasManager.FollowTransform(0));
                            StartCoroutine(canvasManager.SetPopUpImage(0));
                            break;
                        case DetectControllers.HandsHandler.NONE:
                            yield return StartCoroutine(canvasManager.SetPopUp(0, 1, 0));
                            StartCoroutine(canvasManager.FollowTransform(0));
                            break;
                    }

                    aAudio.SetAudio(0, 1);
                    break;
                case 2:
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    switch (detectControllers.lastEnabledHandler)
                    {
                        case DetectControllers.HandsHandler.HI5:
                            yield return StartCoroutine(canvasManager.SetPopUp(1, 1, 1));
                            StartCoroutine(canvasManager.SetIcono(2, 1));
                            break;
                        case DetectControllers.HandsHandler.Vive_Controller:
                            yield return StartCoroutine(canvasManager.SetPopUp(2, 1, 1));
                            StartCoroutine(canvasManager.SetIcono(2, 1));
                            StartCoroutine(canvasManager.SetPopUpImage(1));
                            break;
                        case DetectControllers.HandsHandler.NONE:
                            yield return StartCoroutine(canvasManager.SetPopUp(0, 1, 1));
                            break;
                    }

                    aAudio.SetAudio(0, 2);
                    break;
                case 3:
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    switch (detectControllers.lastEnabledHandler)
                    {
                        case DetectControllers.HandsHandler.HI5:
                            yield return StartCoroutine(canvasManager.SetPopUp(1, 1, 2));
                            StartCoroutine(canvasManager.SetIcono(2, 2));
                            break;
                        case DetectControllers.HandsHandler.Vive_Controller:
                            yield return StartCoroutine(canvasManager.SetPopUp(2, 1, 2));
                            StartCoroutine(canvasManager.SetIcono(2, 2));
                            StartCoroutine(canvasManager.SetPopUpImage(1));
                            break;
                        case DetectControllers.HandsHandler.NONE:
                            yield return StartCoroutine(canvasManager.SetPopUp(0, 1, 2));
                            break;
                    }

                    break;
                case 4:
                    canvasManager.isAnim = true;
                    canvasManager.isFollow = true;
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    switch (detectControllers.lastEnabledHandler)
                    {
                        case DetectControllers.HandsHandler.HI5:
                            yield return StartCoroutine(canvasManager.SetPopUp(1, 2, 0));
                            StartCoroutine(canvasManager.SetIcono(3, 0));
                            StartCoroutine(canvasManager.FollowTransform(2));
                            break;
                        case DetectControllers.HandsHandler.Vive_Controller:
                            yield return StartCoroutine(canvasManager.SetPopUp(2, 2, 0));
                            StartCoroutine(canvasManager.SetPopUpImage(2));
                            StartCoroutine(canvasManager.SetIcono(3, 0));
                            StartCoroutine(canvasManager.FollowTransform(2));
                            break;
                        case DetectControllers.HandsHandler.NONE:
                            yield return StartCoroutine(canvasManager.SetPopUp(0, 1, 0));
                            StartCoroutine(canvasManager.FollowTransform(2));
                            break;
                    }

                    aAudio.SetAudio(0, 3);
                    break;
                case 5:
                    StartCoroutine(canvasManager.SetPopUp(0, 2, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    aAudio.SetAudio(2, 4);
                    break;
                case 6:
                    StartCoroutine(canvasManager.SetPopUp(0, 3, 0));
                    break;
                case 7:
                    StartCoroutine(canvasManager.SetPopUp(0, 4, 0));
                    break;
                case 8:
                    StartCoroutine(canvasManager.SetPopUp(0, 5, 0));
                    StartCounterActivation();
                    break;
                case 9:
                    StartCoroutine(canvasManager.SetPopUp(0, 6, 0));
                    StartCoroutine(ButtonActivation());
                    break;
            }
        }

        #endregion
    }
}