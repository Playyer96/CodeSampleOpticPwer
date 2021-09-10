using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFirstInteraction : MonoBehaviour
{
    [SerializeField] DreamHouseStudios.VR.Interactable interactable = null;
    [SerializeField] string checklist;
    [SerializeField] string item;

    bool alreadyCheck;

    private void OnEnable()
    {
        alreadyCheck = false;
    }

    private void Start()
    {
        if (!interactable)
            interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
    }

    private void Update()
    {
        if (interactable)
        {
            if (interactable.beingGrabbed && !alreadyCheck)
            {
                alreadyCheck = true;
                ChecklistSet(checklist, item, true);
            }
        }
    }

    [ContextMenu("Send To Checklist")]
    public void TestCHecklist()
    {
        ChecklistSet(checklist, item, true);
    }

    public void ChecklistSet(string checklist, string item, bool value)
    {
        Checklist.Set(checklist, item, value);
    }
}