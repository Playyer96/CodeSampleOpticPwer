using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using Valve.VR;

namespace DreamHouseStudios.SofasaLogistica
{
    public class Stapler : MonoBehaviour
    {
        [SerializeField] private AnimatedBox animatedBox;
        [SerializeField] private Animator animator;
        [SerializeField] private StudioEventEmitter fx_Grapar;
        [SerializeField] private StudioEventEmitter fx_sellar_caja;
        public UnityEvent e_OnGrapBox;

        private void Start()
        {
            if (e_OnGrapBox == null)
            {
                e_OnGrapBox = new UnityEvent();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AssembleBox"))
            {
                if (animatedBox.boxPoses == AnimatedBox.BoxPoses.assemble)
                {
                    animatedBox.boxPoses = AnimatedBox.BoxPoses.stapledSideBox;
                    animator.SetTrigger("Grapar");
                    e_OnGrapBox.Invoke();

                    if (fx_Grapar)
                    {
                        if (fx_Grapar.IsPlaying()) fx_Grapar.Stop();

                        fx_Grapar.SetParameter("GrabNDrop", 2);
                        fx_Grapar.Play();
                    }

                    if (fx_sellar_caja)
                    {
                        if (fx_sellar_caja.IsPlaying()) fx_sellar_caja.Stop();

                        fx_sellar_caja.Play();
                    }
                }
            }
        }
    }
}