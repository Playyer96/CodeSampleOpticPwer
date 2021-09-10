using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CloseScene : MonoBehaviour
{
    public CanvasGroup cg;
    public float t_TimeWait;
    private float t_Time;
    public float t_TimeToChangeScene;
    public string s_SceneName;
    public float t_TimeToStart;
    private float tt;
    public GameObject SFX_PopUp;

    void Awake()
    {
        cg.alpha = 0;
        SFX_PopUp.SetActive(false);
        t_Time = 0;
        tt = 0;
    }

    private void Update()
    {
        if (tt < t_TimeToStart)
        {
            tt += Time.deltaTime;
        }
        else
        {
            if (t_Time < t_TimeWait)
            {
                t_Time += Time.deltaTime;
                cg.alpha = t_Time / t_TimeWait;
                if (!SFX_PopUp.activeSelf)
                {
                    SFX_PopUp.SetActive(true);
                }
                if (t_Time >= t_TimeWait)
                {
                    StartChangeSecene();
                }
            }
        }
    }

    void StartChangeSecene()
    {
        StartCoroutine(ChangeToStartScene());
    }

    IEnumerator ChangeToStartScene()
    {
        yield return new WaitForSeconds(t_TimeToChangeScene);
        SceneManager.LoadScene(s_SceneName);
    }
}
