using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class TouchScreenCamera : MonoBehaviour
{
    [SerializeField] DreamHouseStudios.SofasaLogistica.CameraShooter cameraShooter;
    public UnityEvent e_OnTouchScreen;

    private void Start()
    {
        if (e_OnTouchScreen == null)
        {
            e_OnTouchScreen = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cameraShooter)
        {
            if(other.GetComponent<CanvasInteractor>()!= null)
            {
                if (other.GetComponent<CanvasInteractor>().canInterac)
                {
                    other.GetComponent<CanvasInteractor>().canInterac = false;
                    cameraShooter.StartTakePhoto();
                    e_OnTouchScreen.Invoke();
                }
            }
        }
    }
}
