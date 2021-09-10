using System.Collections;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class AnimatorTrigger : MonoBehaviour
    {
        #region Components
        [SerializeField] Animator animator;
        [SerializeField] StudioEventEmitter eventEmitter;
        [SerializeField] string parameter;
        [SerializeField] bool playAnimOnTrigger = false;
        [SerializeField] bool OnlySetTrue = false;
        [SerializeField] string colliderTag;

        [SerializeField] bool activateInteractable;
        [SerializeField] GameObject interactable;
        public UnityEvent e_OnCutTape;

        bool value = false;

        IEnumerator startAnimator;

        public bool Value { get { return value; } }
        #endregion

        #region Unity Functions
        private void Start()
        {
            if (e_OnCutTape == null)
            {
                e_OnCutTape = new UnityEvent();
            }
            if (!animator)
                animator = GetComponent<Animator>();

            if (interactable)
                interactable.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (playAnimOnTrigger)
            {
                if (other.gameObject.tag == colliderTag)
                {
                    if (OnlySetTrue)
                    {
                        value = true;
                        SetBool(value);
                        e_OnCutTape.Invoke();
                    }
                    else
                    {
                        if (value == true)
                            value = false;
                        else
                            value = true;
                        SetBool(value);
                    }
                }
            }
        }
        #endregion

        #region Functions
        public void SetBool(bool value)
        {
            if (startAnimator != null)
                StopCoroutine(startAnimator);

            startAnimator = StartAnimator(value);
            if (transform.gameObject.activeInHierarchy)
            {
                StartCoroutine(startAnimator);
            }
        }

        IEnumerator StartAnimator(bool value)
        {
            yield return new WaitForEndOfFrame();
            animator.SetBool(parameter, value);

            if (eventEmitter)
            {
                if (eventEmitter.IsPlaying())
                    eventEmitter.Stop();

                eventEmitter.Play();
            }
            yield return new WaitForSeconds(0.5f);
            if (activateInteractable)
            {
                if (interactable)
                {
                    interactable.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion
}