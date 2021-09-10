using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.VR
{
    public class GrabInteractables : MonoBehaviour
    {
        public Transform[] snapDestination;

        [SerializeField]
        private GrabQuantity grabQuantity = GrabQuantity.First;

        public List<Interactable> hovered;

        public List<Interactable> grabbed;//hay que referenciar parent

        public void Grabbed(HandGestures handGestures)
        {
            Grabbed();
        }

        public void Grabbed()
        {
            if (hovered.Count > 0)
            {
                switch (grabQuantity)
                {
                    case GrabQuantity.All:
                        grabbed.AddRange(hovered);
                        break;
                    case GrabQuantity.First:
                        grabbed.Add(hovered[0]);
                        break;
                    case GrabQuantity.Last:
                        grabbed.Add(hovered[hovered.Count - 1]);
                        break;
                }

                foreach (Interactable i in grabbed)
                {
                    i.Grab(this);
                    hovered.Remove(i);
                }
            }
        }

        public void Released(HandGestures handGestures)
        {
            Released();
        }

        public void Released()
        {
            foreach (Interactable i in grabbed)
                if (i.currentGrabber == this)
                    i.Release(this);
            grabbed.Clear();
        }

        //public void Grip(HandGestures handGestures)
        //{
        //    Grip();
        //}

        //public void GripRelease(HandGestures handGestures)
        //{
        //    GripRelease();
        //}

        public void Grip()
        {
            foreach (Interactable i in grabbed)
                //if (i.currentGrabber == this)
                if (i.enabled)////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////FIX?
                    i.Grip(this);
        }

        public void GripRelease()
        {
            foreach (Interactable i in grabbed)
                //if (i.currentGrabber == this)
                i.GripRelease(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null && interactable.enabled)
                hovered.Add(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null && !interactable.parentable && interactable.beingGrabbed)
                interactable.Release(this);//AGREGAR GRIP RELEASE?
            if (interactable != null && hovered.Contains(interactable))
                hovered.Remove(interactable);
            //hovered.Find(o => o.name == "cosa");
        }

        [ContextMenu("Remove colliders.")]
        private void RemoveAllColliders()
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
                if (Application.isPlaying)
                    Destroy(c);
                else
                    DestroyImmediate(c);
        }

        [ContextMenu("Remove rigidbodies.")]
        private void RemoveAllRigidbodies()
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
                if (Application.isPlaying)
                    Destroy(rb);
                else
                    DestroyImmediate(rb);
        }

        [ContextMenu("Remove Hi5_Glove_Interaction_Hand.")]
        private void RemoveAllHi5_Glove_Interaction_Hands()
        {
            foreach (Hi5_Interaction_Core.Hi5_Glove_Interaction_Finger hi5gif in GetComponentsInChildren<Hi5_Interaction_Core.Hi5_Glove_Interaction_Finger>())
                if (Application.isPlaying)
                    Destroy(hi5gif);
                else
                    DestroyImmediate(hi5gif);
        }

        private enum GrabQuantity
        {
            First,
            Last,
            All
        }

    }
}
