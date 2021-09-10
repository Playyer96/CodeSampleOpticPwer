using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.VR;
using UnityEngine;
using System.Linq;

public class BoxesHolder : MonoBehaviour
{
    public Interactable cajonInteractable;
    public GameObject highlightCajon;

    public HandInteractor handInteractor;

    public List<GameObject> cajitas = null;
    public List<DreamHouseStudios.SofasaLogistica.ReferenceState> cajonReferenceStates = null;

    private IEnumerator checkForBoxes;
    private WaitForEndOfFrame _waitForEndOfFrame;
    private WaitForSeconds _quaterSecond;

    private bool killWhenComplete = true;
    private bool dispencer;

    private void Start()
    {
        Init();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Cajita" || other.name == "dispensadoraCinta" || other.name == "Calculator")
        {
            if (other.GetComponent<Interactable>())
            {
                if (!other.GetComponent<Interactable>().beingGrabbed)
                {
                    other.transform.SetParent(transform);
                    if (cajitas.Contains(other.gameObject)) return;
                    cajitas.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Cajita" || other.name == "dispensadoraCinta" || other.name == "Calculator")
        {
            if (cajitas.Contains(other.gameObject))
                cajitas.Remove(other.gameObject);

            //other.transform.parent = null;

            if (cajitas.Count < 6 && !handInteractor.b_Snap)
            {
                StartCheckForBoxes();

                cajonInteractable.enabled = false;
                highlightCajon.SetActive(false);
            }
        }
    }

    private void Init()
    {
        _waitForEndOfFrame = new WaitForEndOfFrame();
        _quaterSecond = new WaitForSeconds(0.25f);
        dispencer = false;

        StartCheckForBoxes();

        cajonInteractable.enabled = false;
        highlightCajon.SetActive(false);
    }

    private void StartCheckForBoxes()
    {
        if (checkForBoxes != null)
            StopCoroutine(checkForBoxes);

        checkForBoxes = CheckForBoxes();
        StartCoroutine(checkForBoxes);
    }

    private IEnumerator CheckForBoxes()
    {
        killWhenComplete = true;
        yield return _waitForEndOfFrame;

        while (killWhenComplete)
        {
            yield return _quaterSecond;

            if (cajitas.Count == 6 && cajonReferenceStates.All(item => item.IsInOrder))
            {
                cajonInteractable.enabled = true;
                highlightCajon.SetActive(true);

                killWhenComplete = false;
            }
        }
    }

    public void DisableBoxesPhysics()
    {
        highlightCajon.SetActive(false);
        cajonInteractable.enabled = false;

        for (int i = 0; i < cajitas.Count; i++)
        {
            cajitas[i].GetComponent<Rigidbody>().isKinematic = true;
            cajitas[i].GetComponent<Interactable>().enabled = false;
        }
    }
}