using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ObjectReset : MonoBehaviour
    {
        #region Components
        [SerializeField] Transform objTransform;
        public Transform resetPos;
        [System.NonSerialized]
        public Vector3 resetPos_; //Dalir
        [System.NonSerialized]
        public Quaternion resetRotation_; //Dalir
        #endregion
        public bool b_IsGrab;
        public Rigidbody rb;
        public DreamHouseStudios.VR.Interactable i_Interactable;
        #region Unity Functions
        private void Start()
        {
            objTransform = GetComponent<Transform>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Floor")
            {
                transform.parent = null;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                //objTransform.rotation = Quaternion.identity;
                //objTransform.position = resetPos.position;
                objTransform.rotation = resetPos == null ? resetRotation_ : Quaternion.identity; //Dalir
                objTransform.position = resetPos == null ? resetPos_ : resetPos.position; //Dalir
            }
        }
        #endregion

        public void SetBoolIsGrab(bool b_Val)
        {
            b_IsGrab = b_Val;
        }

        public void SetKinematic()
        {
            StartCoroutine(KinematicAccion());
        }

        public IEnumerator KinematicAccion()
        {
            yield return new WaitForSeconds(.01f);
            i_Interactable.enabled = false;
            yield return new WaitForSeconds(1f);
            rb.isKinematic = true;
        }
    }
}