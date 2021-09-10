using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PocketScaner : MonoBehaviour
{
    public Transform[] t_DataReciber;
    public bool canScan = false;
    public GameObject g_FlashLight;
    public GameObject g_pocketSound;
    public GameObject g_decalLaser;
    [SerializeField] public StudioEventEmitter fxScan;
    private IEnumerator StartTurnOffLaser;

 
    private void OnTriggerEnter(Collider other)
    {
        if (canScan)
        {
            for (int i = 0; i < t_DataReciber.Length; i++)
            {
                if (t_DataReciber[i].gameObject.activeInHierarchy)
                {
                    t_DataReciber[i].GetComponent<PocketFunctions>().GetData(other.transform);
                    g_pocketSound.SetActive(false);

                    if (fxScan)
                    {
                        if (fxScan.IsPlaying())
                            fxScan.Stop();

                        fxScan.Play();
                    }
                }
            }
        }
        else
        {
            return;
        }
    }

    public void ActiveFlash()
    {
        g_FlashLight.SetActive(true);
        g_decalLaser.SetActive(true);
    }

    public void DeactiveFlash()
    {
        g_FlashLight.SetActive(false);
        g_decalLaser.SetActive(false);
    }
    
    public void CanScan()
    {
        canScan = true;
    }

    public void NotScan()
    {
        canScan = false;
        g_FlashLight.SetActive(false);
        g_decalLaser.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }
}
