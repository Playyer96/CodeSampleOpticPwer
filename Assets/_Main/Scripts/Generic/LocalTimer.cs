using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class LocalTimer : MonoBehaviour
{
    public Chronometer c;
    public ScenesManager ud;
    public SpectraUISettings settings;
    void Start()
    {
        StartCoroutine(SetLocalTimer());
        StartCoroutine(SetLocalData());
    }

    // Update is called once per frame
    void Update()
    {
        if (c != null)
        {
            SectionChronometer();
        }
    }

    public void SectionChronometer()
    {
        if (PauseButton.isPause && ScenesManager.instance.isLoadingScene || ExperienceUI.instance.stopChronometer)
        {
            return;
        }

        if (PauseButton.isPause)
            return;

        c.sectionTime += Time.deltaTime;
    }

    IEnumerator SetLocalTimer()
    {
        while (c == null)
        {
            yield return null;
            c = FindObjectOfType<Chronometer>();
        }
        c.sectionTime = 0;
    }

    IEnumerator SetLocalData()
    {
        while (ud == null)
        {
            yield return null;
            ud = FindObjectOfType<ScenesManager>();
        }

        ud.canSendData = true;
        if (settings.experienMode == ExperienMode.Entrenamiento)
        {
            ud.userData.moduleAdvance.fechaInicialEntrenamiento = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }
        else
        {
            ud.userData.moduleGrade.fechaInicialEvaluacion = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
