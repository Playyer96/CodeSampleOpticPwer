using DreamHouseStudios.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomProductsAndsShelfsSlots;

namespace Picking
{
    public class MovableShelf : MonoBehaviour
    {
        public static MovableShelf instance;
        public bool isMoving;
        //private Vector3 lastPos;

        public List<ProductTrigger> productTriggers;

        public Vector3 v;

        private void Awake()
        {
            instance = this;
        }

        //private void Update()
        //{
        //    if (!isMoving) return;
        //    foreach (ProductTrigger pt in productTriggers)
        //    {
        //        //if (pt.released)
        //        //pt.localPosition = transform.InverseTransformPoint(pt.transform.position);
        //        pt.diff = pt.transform.position - transform.position;
        //        //if (pt.rb != null)
        //        //    pt.rb.isKinematic = true;
        //    }
        //}

        //private void LateUpdate()
        //{
        //    if (isMoving)
        //    {
        //        //Debug.Break();
        //        foreach (ProductTrigger pt in productTriggers)
        //        {
        //            //if (pt.released)
        //            //Vector3 local = transform.InverseTransformPoint(pt.transform.position);

        //            //pt.transform.position = transform.position + (transform.position - pt.transform.position);

        //            pt.transform.position = v = transform.position + pt.diff;
        //            //if (pt.rb != null)
        //            //    pt.rb.isKinematic = false;

        //            //pt.rb.constraints = RigidbodyConstraints.None;
        //            //pt.transform.position = transform.position;

        //            //v = pt.transform.position - transform.position;
        //        }
        //    }
        //    isMoving = transform.position != lastPos;
        //    lastPos = transform.position;
        //}

        private void OnTriggerEnter(Collider other)
        {
            ProductTrigger pt = other.GetComponent<ProductTrigger>();

            if (pt == null) return;

            productTriggers.Add(pt);
        }

        private void OnTriggerExit(Collider other)
        {
            ProductTrigger pt = other.GetComponent<ProductTrigger>();

            if (pt == null) return;

            productTriggers.Remove(pt);
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(v, .5f);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(transform.position, .5f);
        //}
    }
}
