using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using DreamHouseStudios.ConfinedSpaces;

public class AudioManager : MonoBehaviour
{
    public SoundEventHandler[] soundEventHandler;
    private SoundEventHandler currentEventHandler;
    public StudioEventEmitter[] soundFX;
    public ParamRef pRef;

    public void SetAudioEvent(int indexSoundFx, string param,float value)
    {
        currentEventHandler = soundEventHandler[indexSoundFx];
        FMOD.RESULT result = currentEventHandler.inst.setParameterValue(param, value);
        if (result == FMOD.RESULT.OK)
        {
            Debug.LogFormat("Succes play: {0}", currentEventHandler.name);
            currentEventHandler.Play();
        }
        else
            Debug.LogWarningFormat("Audio {0} cannot reproduce: {1} - {2} = {3}", currentEventHandler.name, param, value, result);
    }

    public void SetAudio(string param, int indexSoundFX, float index)
    {
        soundFX[indexSoundFX].gameObject.SetActive(false);
        pRef.Name = param;
        pRef.Value = index;
        soundFX[indexSoundFX].Params[0] = pRef;
        //soundFX[indexSoundFX].Play();
        soundFX[indexSoundFX].gameObject.SetActive(true);
    }

    public void SetAudio(int indexSoundFX, float index)
    {
        soundFX[indexSoundFX].gameObject.SetActive(false);
        pRef.Name = "DX";
        pRef.Value = index;
        soundFX[indexSoundFX].Params[0] = pRef;
        soundFX[indexSoundFX].gameObject.SetActive(true);
    }

    public void SetAudio(int indexSoundFX)
    {
        soundFX[indexSoundFX].gameObject.SetActive(false);
        soundFX[indexSoundFX].gameObject.SetActive(true);
    }
}