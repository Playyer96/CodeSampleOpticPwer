using System;
using System.Collections;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BotonCambio : MonoBehaviour
{
    public Method actualMethod;
    public string s_SceneName;
    public int i_SceneIndex;
    public float f_TimeToChange;
    public StudioEventEmitter soundFX;
    public SpectraUISettings s_Settings;
    public UnityEvent OnStartChangeSecene;

    Transform target; //Cabeza del jugador

    private void Awake()
    {
        if (OnStartChangeSecene == null)
        {
            OnStartChangeSecene = new UnityEvent();
        }
        
        if (s_Settings.experienMode == ExperienMode.Entrenamiento)
        {
            gameObject.SetActive(false);
        }

        target = FindObjectOfType<Player>().hmdTransforms[0];
    }

    private void Update()
    {
        transform.rotation = DesireRotation(transform);
    }


    [ContextMenu("End Module")]
    public void ButtonBH()
    {
        OnStartChangeSecene.Invoke();
        PlaySound(soundFX);
        StopAllCoroutines();
        //StartCoroutine(WaitToChange());
        ScenesManager.instance.StartLoadScene(s_SceneName);
        GetComponent<BoxCollider>().enabled = false;
    }

    public IEnumerator WaitToChange()
    {
        //Espacio para llamar funciones que se necesiten antes de cambiar de escena como ejemplo, mandar los datos a la base de datos.
        yield return new WaitForSeconds(f_TimeToChange);
        OnChangeScene();
    }

    public void OnChangeScene()
    {
        switch (actualMethod)
        {
            case Method.String:
                SceneManager.LoadScene(s_SceneName);
                break;
            case Method.Int:
                SceneManager.LoadScene(i_SceneIndex);
                break;
        }
    }

    public void PlaySound(StudioEventEmitter soundFx)
    {
        if (soundFx)
        {
            if (soundFx.IsPlaying())
                soundFx.Stop();

            soundFx.Play();
        }
    }

    Quaternion DesireRotation(Transform tr)
    {
        Vector3 relativePos = (tr.position - target.position);
        relativePos.y = 0;

        Quaternion desireRotation = Quaternion.LookRotation(relativePos);
        return desireRotation;
    }
}

public enum Method
{
    String,
    Int
}