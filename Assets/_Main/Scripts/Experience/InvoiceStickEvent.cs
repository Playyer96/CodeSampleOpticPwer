using System;
using FMODUnity;
using UnityEditor;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class InvoiceStickEvent : MonoBehaviour
    {
        new Collider collider;
        new Rigidbody rigidbody;
        new Transform transform;
        public bool b_Stick;
        public UnityEvent e_OnStick;
        public bool b_CanStick;
        public string s_Tag;
        public TypeStick stickTarget;
        public DreamHouseStudios.VR.Interactable i_Interactable;

        [SerializeField] StudioEventEmitter eventEmitter;

        private void Start()
        {
            collider = GetComponent<Collider>();
            rigidbody = GetComponent<Rigidbody>();
            transform = GetComponent<Transform>();
            i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
            if (e_OnStick == null)
            {
                e_OnStick = new UnityEvent();
            }

            b_CanStick = true;
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (b_CanStick)
            {
                if (collision.transform.CompareTag("StickBox"))
                {
                    collider.isTrigger = true;
                    rigidbody.isKinematic = true;
                    transform.SetParent(collision.transform);
                    TryPlaySound(0);
                }

                if (collision.transform.CompareTag("StickPoint"))
                {
                    if (collision.transform.GetComponent<Bag_Shelf>().transform.GetComponentInChildren<ProductInvoice>().Product.productId == GetComponent<ReceptionInvoice>().Product.productId)
                    {
                        collider.isTrigger = true;
                        TryPlaySound(0);
                        rigidbody.isKinematic = true;
                        transform.SetParent(collision.transform);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("StickBox"))
            {
                e_OnStick.Invoke();
                if (!GetComponent<DreamHouseStudios.VR.Interactable>().beingGrabbed && !b_Stick)
                {
                    b_Stick = true;
                    GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                }
                else
                {
                    if (!GetComponent<DreamHouseStudios.VR.Interactable>().beingGrabbed)
                    {
                        b_CanStick = false;
                        transform.parent = null;
                        collider.isTrigger = false;
                        rigidbody.isKinematic = false;
                        transform.parent = null;
                        Invoke("ActiveCollision", 1f);
                    }
                }
            }

            if (other.CompareTag("StickPoint"))
            {
                if (!GetComponent<DreamHouseStudios.VR.Interactable>().beingGrabbed && !b_Stick)
                {
                    b_Stick = true;
                    GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                    if (other.GetComponent<Bag_Shelf>() != null)
                    {
                        if (other.transform.GetComponent<Bag_Shelf>().transform.GetComponentInChildren<ProductInvoice>()
                            .Product.productId == GetComponent<ReceptionInvoice>().Product.productId)
                        {
                            other.GetComponent<Bag_Shelf>().SetReceptionInvoice(transform);
                        }
                        else
                        {
                            b_CanStick = false;
                            transform.parent = null;
                            collider.isTrigger = false;
                            rigidbody.isKinematic = false;
                            transform.parent = null;
                            Invoke("ActiveCollision", 1f);
                        }
                    }

                    e_OnStick.Invoke();
                }
            }
        }
        */

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag(s_Tag))
            {
                switch (stickTarget)
                {
                    case TypeStick.Product:
                        if (other.transform.GetComponentInChildren<ProductInvoice>() != null)
                        {
                            ProductInvoice pi = other.transform.GetComponentInChildren<ProductInvoice>();
                            if (pi.Product.productId == GetComponent<ReceptionInvoice>().Product.productId)
                            {
                                other.transform.GetComponent<Bag_Shelf>().SetReceptionInvoice(transform);
                                Stick_BH();
                            }
                        }
                        break;
                    case TypeStick.Box:
                        if (other.transform.GetComponent<BoxPacking_BH>() != null)
                        {
                            other.transform.GetComponent<BoxPacking_BH>().SetInvoice(transform);
                            Stick_BH();
                        }
                        break;
                }
            }
        }

        void Stick_BH()
        {
            i_Interactable.onRelease.Invoke(i_Interactable);
            rigidbody.isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
            i_Interactable.enabled = false;
            e_OnStick.Invoke();
            TryPlaySound(0);
        }

        private void Update()
        {
            if (transform.parent == null)
            {
                rigidbody.isKinematic = false;
                collider.isTrigger = false;
            }
        }

        public void TryPlaySound(float value)
        {
            if (eventEmitter.IsPlaying())
            {
                return;
            }

            eventEmitter.EventInstance.setParameterValue("GrapNDrop", value);
            eventEmitter.Play();
        }

        public void ActiveCollision()
        {
            b_CanStick = true;
        }
    }
}

public enum TypeStick
{
    Product,
    Box
}