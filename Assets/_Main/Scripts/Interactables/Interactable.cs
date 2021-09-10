using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.VR
{
    [RequireComponent(typeof(Rigidbody))]
    // [CanEditMultipleObjects]
    public class Interactable : MonoBehaviour
    {
        public bool snappable;
        public bool parentable = true;
        //public Transform snapFrom;
        public int snapTo;
        public bool beingGrabbed;
        public bool updateVelocity;
        public GrabReleaseEvent onGrab, onRelease, onGrip, onGripRelease, onPoint, onPointRelease;
        public GrabInteractables currentGrabber;

        private Transform parent = null;

        Rigidbody rb;

        bool useGravity, kinematic;
        RigidbodyConstraints constraints;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {

        }
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                useGravity = rb.useGravity;
                kinematic = rb.isKinematic;
                constraints = rb.constraints;
            }
        }

        public void Grab(GrabInteractables grabber)
        {
            // if (beingGrabbed) return;
            if (!enabled) return;

            currentGrabber = grabber;


            beingGrabbed = true;

            parent = transform.parent != null ? (transform.parent.GetComponent<GrabInteractables>() == null ? transform.parent : parent) : parent;

            if (rb != null)
            {
                useGravity = rb.useGravity;
                kinematic = rb.isKinematic;
                constraints = rb.constraints;
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.None;
                //Collider c = GetComponent<Collider>();
                //if (c != null)
                //    c.enabled = false;
                //grabber.hovered.RemoveAll(h => h == this);
            }

            Transform snap = snapTo >= 0 && grabber.snapDestination.Length > snapTo ? grabber.snapDestination[snapTo] : null;

            if (snappable && snap != null)
            {
                //transform.position = Vector3.zero + (transform.position - snapFrom.position);
                // transform.position = grabber.snapDestination.position + (transform.position - snapFrom.position);
                transform.position = snap.position;
                transform.rotation = snap.rotation;
            }
            if (parentable)
            {
                transform.SetParent(grabber.transform);
            }
            onGrab.Invoke(this);
        }

        public void Release(GrabInteractables releaser)
        {
            if (!beingGrabbed) return;
            if (!enabled) return;

            beingGrabbed = false;
            currentGrabber = null;
            if (rb != null)
            {
                rb.isKinematic = kinematic;

                Vector3 vel = rb.velocity;
                rb.useGravity = useGravity;

                if (updateVelocity)
                    rb.velocity = vel;

                rb.constraints = constraints;
                //Collider c = GetComponent<Collider>();
                //if (c != null)
                //    c.enabled = true;
            }

            transform.SetParent(parent);

            onRelease.Invoke(this);
        }

        public void Grip(GrabInteractables grabber)
        {
            if (!enabled) return;
            Debug.Log("GRIP");
            // if (beingGrabbed) return;
            onGrip.Invoke(this);
        }

        public void GripRelease(GrabInteractables releaser)
        {
            if (!enabled) return;
            Debug.Log("GRIPRELEASE");
            // if (!beingGrabbed) return;
            onGripRelease.Invoke(this);
        }

        [System.Serializable]
        public class GrabReleaseEvent : UnityEvent<Interactable> { }
    }
}
