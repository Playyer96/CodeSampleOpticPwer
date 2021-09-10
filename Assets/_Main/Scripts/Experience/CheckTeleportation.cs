using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class CheckTeleportation : MonoBehaviour
    {
        [Space(10f),Header("Teleport 1")]
        [SerializeField] string checklist;
        [SerializeField] string item;

        [Space(10f), Header("Teleport 2")]
        [SerializeField] string checklistTwo;
        [SerializeField] string itemTwo;


        private Vector3 _originalPos = Vector3.zero;
        private Vector3 _teleportPos = Vector3.zero;


        public bool alreadyTeleported = false;
        public bool alreadyTeleportedAgain = false;
        private Transform _transform;

        private void OnEnable()
        {
            _transform = GetComponent<Transform>();
            _originalPos = _transform.position;
        }


        private void Update()
        {
            if (_transform.position != _originalPos && !alreadyTeleported)
            {
                alreadyTeleported = true;
                ChecklistSet(checklist, item, true);
                _teleportPos = _transform.position;
            }

            if(alreadyTeleported)
            {
                if ( _transform.position != _teleportPos && !alreadyTeleportedAgain)
                {
                    alreadyTeleportedAgain = true;
                    ChecklistSet(checklistTwo, itemTwo, true);
                }
            }
        }

        [ContextMenu("First Teleport")]
        public void TPOne()
        {
            ChecklistSet(checklist, item, true);
        }

        [ContextMenu("Second Teleport")]
        public void TPTwo()
        {
            ChecklistSet(checklistTwo, itemTwo, true);
        }

        public void ChecklistSet(string checklist, string item, bool value)
        {
            Checklist.Set(checklist, item, value);
        }
    }
}
