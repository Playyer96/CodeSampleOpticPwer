using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class AnimatedBox : MonoBehaviour
    {
        #region Components
        public BoxPoses boxPoses;
        [SerializeField] private DreamHouseStudios.VR.Interactable handlerOne;
        [SerializeField] private DreamHouseStudios.VR.Interactable handlerTwo;

        [SerializeField] private List<GameObject> boxHandles = null;

        [SerializeField] private GameObject disassembleBox;
        [SerializeField] private Animator _animatorDisassembleBox;
        [SerializeField] private GameObject assembleBox;
        [SerializeField] private GameObject cinta;

        [SerializeField] private BoxProductsHolder _boxProductsHolder;
        
        public UnityEvent e_OpenBox;
        public UnityEvent e_ClosedBox;
        public UnityEvent e_StapledBox;
        public UnityEvent e_SealedBox;

        private Rigidbody _rigidbody;
        private Transform _transform;

        private float finalPos;
        private bool setOpenBox = false;
        private bool isStapledBox = false;
        private bool boxTaped = false;
        private bool canSealed = false;
        private bool isSealed = false;
        private bool boxEventsDone = false;
        public enum BoxPoses
        {
            disassembled, assemble,stapledSideBox,closed,sealedBox
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();

            boxPoses = BoxPoses.disassembled;
        }

        private void Start()
        {
            SetBoxPoses();

            disassembleBox.SetActive(true);
            assembleBox.SetActive(false);

            if (e_OpenBox == null)
            {
                e_OpenBox = new UnityEvent();
            }
            if (e_ClosedBox == null)
            {
                e_ClosedBox = new UnityEvent();
            }
            if (e_StapledBox == null)
            {
                e_StapledBox = new UnityEvent();
            }
            if (e_SealedBox == null)
            {
                e_SealedBox = new UnityEvent();
            }
        }

        private void Update()
        {
            finalPos = Vector3.Distance(handlerOne.transform.position, handlerTwo.transform.position);

            var scaledValue = Mathf.Clamp(finalPos / 1.3f, 0f, 1.3f);

            _animatorDisassembleBox.SetFloat("assemble", scaledValue);

            if (scaledValue >= .9f && !setOpenBox)
            {
                boxPoses = BoxPoses.assemble;
                SetBoxPoses();
                setOpenBox = true;
            }

            if (boxHandles.All(handle => !handle.activeInHierarchy) && !canSealed && setOpenBox && isStapledBox)
            {
                boxPoses = BoxPoses.closed;
                SetBoxPoses();
                canSealed = true;
                _boxProductsHolder.OnClosedBox();
            }

            if(boxPoses == BoxPoses.stapledSideBox && !isStapledBox)
            {
                SetBoxPoses();
                isStapledBox = true;
            }

            if(boxPoses == BoxPoses.sealedBox && canSealed && !boxEventsDone) 
            {
                print("Sealed Box");
                SetBoxPoses();
                boxEventsDone = true;
                isSealed = true;
            }
        }

        #endregion

        #region Functions
        public void SetBoxPoses()
        {
            switch (boxPoses)
            {
                case BoxPoses.disassembled:
                    disassembleBox.SetActive(true);
                    assembleBox.SetActive(false);
                    cinta.SetActive(false);
                    break;
                case BoxPoses.assemble:
                    assembleBox.transform.SetParent(null);
                    disassembleBox.SetActive(false);
                    assembleBox.SetActive(true);
                    e_OpenBox.Invoke();
                    break;
                case BoxPoses.stapledSideBox:
                    e_StapledBox.Invoke();
                    break;
                case BoxPoses.closed:
                    e_OpenBox.Invoke();
                    break;
                case BoxPoses.sealedBox:
                    cinta.SetActive(true);
                    e_SealedBox.Invoke();
                    break;
            }
        }
        #endregion
    }
}
