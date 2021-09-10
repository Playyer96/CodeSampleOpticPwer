using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class UnpackBox : MonoBehaviour
    {
        #region Components
        [SerializeField] GameObject top;
        [SerializeField] GameObject interactableTop;
        [SerializeField] AnimatorTrigger tapeOne;
        [SerializeField] AnimatorTrigger tapeTwo;
        [SerializeField] Collider tapeOneCollider, tapeTwoCollider;
        public UnityEvent e_OnRemoveTapes;
        bool b_RemoveTapes = false;

        [HideInInspector] public bool progressSet = false;
        bool isBoxOpen = false;

        UnpackBox unpackBox;

        float time;
        float nextUpdate = 0.25f;

        public bool IsBoxOpen { get { return isBoxOpen; } }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            unpackBox = GetComponent<UnpackBox>();
        }

        void Start()
        {
            if (e_OnRemoveTapes == null)
            {
                e_OnRemoveTapes = new UnityEvent();
                e_OnRemoveTapes.AddListener(OnRemoveTapes);
            }
        }

        private void OnEnable()
        {
            interactableTop.SetActive(false);

            if (top)
                top.SetActive(true);
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time >= nextUpdate)
            {
                time = time - nextUpdate;
                if (tapeOne.Value && tapeTwo.Value)
                {
                    if (top)
                    {
                        top.SetActive(false);
                    }
                    if (!b_RemoveTapes)
                    {
                        b_RemoveTapes = true;
                        e_OnRemoveTapes.Invoke();
                    }

                    interactableTop.SetActive(true);
                }
                if (interactableTop.activeInHierarchy)
                {
                    isBoxOpen = true;
                }
            }
        }

        void OnRemoveTapes()
        {

        }
        #endregion
    }
}