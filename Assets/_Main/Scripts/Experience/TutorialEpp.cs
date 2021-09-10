using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using FMODUnity;

namespace DreamHouseStudios.SofasaLogistica
{
    public class TutorialEpp : Tutorials
    {
        #region Components

        [SerializeField] private EppStepToStep eppStepToStep;
        [SerializeField] private AudioManager aAudio;

        [SerializeField] private CheckFirstInteraction checkFirstInteraction;

        private IEnumerator _setEpps;
        private IEnumerator _stuffLeft;
        private IEnumerator _endOfModule;
        private IEnumerator _beginingOfEpp;
        private IEnumerator _beginingOfEppEvaluation;

        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _waitTenSeconds;
        private WaitForSeconds _waitFiveSeconds;
        private WaitForSeconds _waitSixSeconds;
        private WaitForSeconds _waitFifteenSeconds;

        #endregion Components

        #region Unity Functions

        public override void Start()
        {
            base.Start();
            _waitForEndOfFrame = new WaitForEndOfFrame();
            _waitFiveSeconds = new WaitForSeconds(5f);
            _waitSixSeconds = new WaitForSeconds(6f);
            _waitTenSeconds = new WaitForSeconds(10f);
            _waitFifteenSeconds = new WaitForSeconds(15f);

            checkFirstInteraction.enabled = false;

            switch (settings.experienMode)
            {
                case ExperienMode.Evaluacion:
                    //StartBeginingOfEppEvaluation();
                    StartCoroutine(SetEpps(0));
                    break;

                case ExperienMode.Entrenamiento:
                    StartBeginingOfEpp();
                    break;
            }
        }

        #endregion Unity Functions

        #region Functions

        public void StartEppTutorials(int index)
        {
            if (settings.experienMode == ExperienMode.Evaluacion) return;

            if (_setEpps != null)
                StopCoroutine(_setEpps);

            _setEpps = SetEpps(index);
            StartCoroutine(_setEpps);
        }

        public void StartBeginingOfEpp()
        {
            if (_beginingOfEpp != null)
                StopCoroutine(_beginingOfEpp);

            _beginingOfEpp = BeginingOfEpp();
            StartCoroutine(_beginingOfEpp);
        }

        public void StartBeginingOfEppEvaluation()
        {
            if (_beginingOfEppEvaluation != null)
                StopCoroutine(_beginingOfEppEvaluation);

            _beginingOfEppEvaluation = BeginingOfEppEvaluation();
            StartCoroutine(_beginingOfEppEvaluation);
        }

        public void StartStuffLeft()
        {
            if (_stuffLeft != null)
                StopCoroutine(_stuffLeft);

            _stuffLeft = StuffLeft();
            StartCoroutine(_stuffLeft);
        }

        public void StartModuleEnd()
        {
            if (_endOfModule != null)
                StopCoroutine(_endOfModule);

            _endOfModule = EndOfModule();
            StartCoroutine(_endOfModule);
        }

        private IEnumerator BeginingOfEppEvaluation()
        {
            yield return _waitForEndOfFrame;
            StartEppTutorials(0);
            yield return _waitTenSeconds;
            StartEppTutorials(-1);
        }

        private IEnumerator BeginingOfEpp()
        {
            yield return _waitForEndOfFrame;
            StartEppTutorials(0);
            yield return _waitTenSeconds;
            StartEppTutorials(2);
            yield return _waitSixSeconds;
            StartEppTutorials(3);
            Checklist.Set("Epps", "Can Interact", true);
        }

        private IEnumerator StuffLeft()
        {
            yield return _waitForEndOfFrame;
            StartEppTutorials(5);
        }

        private IEnumerator EndOfModule()
        {
            yield return _waitForEndOfFrame;
            StartEppTutorials(6);
            yield return _waitTenSeconds;
            StartEppTutorials(7);
            yield return _waitFiveSeconds;
            ScenesManager.instance.StartLoadScene(ScenesManager.instance.receptionScene.sceneName);
        }

        private IEnumerator SetEpps(int index)
        {
            canvasManager.isFollow = false;
            canvasManager.isAnim = false;

            switch (index)
            {
                case -1:
                    StartCoroutine(canvasManager.SetPopUp(0, -1, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    break;

                case 0:
                    switch (settings.experienMode)
                    {
                        case ExperienMode.Evaluacion:
                            StartCoroutine(canvasManager.SetPopUp(0, 8, 0));
                            aAudio.SetAudio(2);
                            break;

                        case ExperienMode.Entrenamiento:
                            StartCoroutine(canvasManager.SetPopUp(0, 0, 0));
                            StartCoroutine(canvasManager.SetIcono(-1, 0));
                            aAudio.SetAudio(1);
                            break;
                    }

                    break;

                case 1:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 1, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    aAudio.SetAudio(0, 0);
                    break;

                case 2:
                    StartCoroutine(canvasManager.SetPopUp(0, 2, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    break;

                case 3:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 3, 0));
                    // StartCoroutine(canvasManager.SetIcono(0, 0));
                    break;

                case 4:
                    StartCoroutine(canvasManager.SetPopUp(0, 4, 0));
                    StartCoroutine(canvasManager.SetIcono(1, 0));
                    aAudio.SetAudio(0, 1);
                    break;

                case 5:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 5, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    // aAudio.SetAudio(0, 2);
                    break;

                case 6:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 6, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    aAudio.SetAudio(0, 3);
                    break;

                case 7:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 7, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    aAudio.SetAudio(0, 4);
                    break;

                case 8:
                    yield return StartCoroutine(canvasManager.SetPopUp(0, 9, 0));
                    StartCoroutine(canvasManager.SetIcono(-1, 0));
                    break;
            }
        }

        #endregion Functions
    }
}