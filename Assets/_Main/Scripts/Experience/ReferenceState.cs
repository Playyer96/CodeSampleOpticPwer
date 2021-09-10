using System;
using FMOD;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ReferenceState : MonoBehaviour
    {
        #region Components
        [SerializeField] SpectraUISettings settings;
        [SerializeField] string identifer;

        public bool progressSet = false;
        public bool isInOrder = false;
        public bool b_NeedTrackBool = false;

        public bool IsInOrder { get { return isInOrder; } }
        public Vector3 v_Position;
        public Vector3 v_Rotation;
        public bool b_Event;
        public UnityEvent e_OnSnap;
        private bool called = false;
        public bool b_CanCall = true;

        public bool needReport;
        public ReportBackend rb;
        #endregion

        private void Start()
        {
            if (needReport && rb == null)
            {
                rb = GetComponent<ReportBackend>();
            }
            if (b_Event)
            {
                if (e_OnSnap == null)
                {
                    e_OnSnap = new UnityEvent();
                }
            }
        }

        #region Unity Functions
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == identifer)
            {
                isInOrder = true;
                if (needReport)
                {
                    rb.isReported = true;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == identifer)
            {
                isInOrder = true;
                if (b_NeedTrackBool)
                {
                    if (!other.GetComponent<ObjectReset>().b_IsGrab)
                    {
                        if (settings.experienMode == ExperienMode.Entrenamiento)
                        {
                            if (GetComponent<ReportToCheckList>() != null)
                            {
                                GetComponent<ReportToCheckList>().SetCheckList(true);
                            }

                            if (!called && b_CanCall)
                            {
                                called = true;
                                e_OnSnap.Invoke();
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == identifer)
            {
                isInOrder = false;
                called = false;
                if (needReport)
                {
                    rb.isReported = false;
                }
            }
        }

        public void LaunchEvent()
        {
            e_OnSnap.Invoke();
        }
        #endregion

        public void SetBool(bool value)
        {
            isInOrder = value;
        }
    }
}