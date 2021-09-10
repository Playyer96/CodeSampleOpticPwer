using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class TapeDispencer : MonoBehaviour
    {
        [SerializeField] private AnimatedBox animatedBox;
        [SerializeField] private StudioEventEmitter tapeSound;
        public UnityEvent e_OnTapeBox;

        private void Start()
        {
            if(e_OnTapeBox == null)
                e_OnTapeBox = new UnityEvent();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tape"))
            {
                if (animatedBox.boxPoses == AnimatedBox.BoxPoses.closed)
                {
                    animatedBox.boxPoses = AnimatedBox.BoxPoses.sealedBox;
                    if (tapeSound)
                    {
                        if (tapeSound.IsPlaying())
                            tapeSound.Stop();
                        
                        tapeSound.SetParameter("GrabNDrop",2);
                        tapeSound.Play();
                        e_OnTapeBox.Invoke();
                    }
                }
            }
        }
    }
}