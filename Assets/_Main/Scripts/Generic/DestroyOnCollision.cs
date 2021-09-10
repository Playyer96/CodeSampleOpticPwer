using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Serialization;

namespace DreamHouseStudios.SofasaLogistica
{
    public class DestroyOnCollision : MonoBehaviour
    {
        #region Components

        [SerializeField] string nameTag;
        [SerializeField] string animParameter;

        [SerializeField] Animator trashcanAnimator;
        [SerializeField] StudioEventEmitter trashSound;
        public TrashcanType trashcanType;

        IEnumerator playAnimator;

        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds oneSecond;
        public Transform targetPos;

        #endregion

        #region Unity Functions

        private void Start()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
            oneSecond = new WaitForSeconds(1f);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponent<ItemID>().isSet)
            {
                if (collision.gameObject.CompareTag(nameTag))
                {
                    
                    if (collision.GetComponent<VR.Interactable>() != null)
                    {
                        VR.Interactable i_Interactable = collision.GetComponent<VR.Interactable>();
                        i_Interactable.onRelease.Invoke(i_Interactable);
                        i_Interactable.transform.parent = null;
                        i_Interactable.GetComponent<Rigidbody>().isKinematic = true;
                        collision.transform.position = targetPos.position;
                                
                        collision.gameObject.GetComponent<ItemID>().isSet = true;
                        StartAnimator(collision.gameObject.GetComponent<Animator>(), collision.gameObject);
                    }
                    trashcanAnimator.SetBool("Open", true);
                }
            }
        }

        #endregion

        #region Functions

        private void StartAnimator(Animator animator, GameObject obj)
        {
            if (playAnimator != null)
                StopCoroutine(playAnimator);

            playAnimator = PlayAnimator(animator, obj);
            StartCoroutine(playAnimator);
        }

        IEnumerator PlayAnimator(Animator animator, GameObject obj)
        {
            yield return waitForEndOfFrame;
            animator.SetBool(animParameter, true);
            yield return oneSecond;
            trashcanAnimator.SetBool("Open", false);
            obj.SetActive(false);

            if (trashSound)
            {
                if (trashSound.IsPlaying())
                    trashSound.Stop();

                trashSound.Play();
            }
        }

        #endregion
    }
}

public enum TrashcanType
{
    Green,
    Blue,
    Grey
}