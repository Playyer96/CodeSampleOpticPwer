using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;

public class ManagerIntro : MonoBehaviour {
    public Image[] imgs;
    [Range (0, 5)]
    public float timeFade;
    [Range (1, 10)]
    public float timeOnScreen;
    [Range (0, 5)]
    public float timeBetwenImages;
    public string sceneName = "";

    public AudioManager a_Audio = null;
    public int audioQuantity = 2;

    //---------------------
    public TMPro.TextMeshProUGUI txtIntro;
    [Range (0, 5)]
    public float timeFadeTxt;

    private void Awake () {
        Color c;
        for (int i = 0; i < imgs.Length; i++) {
            c = imgs[i].color;
            c.a = 0;
            imgs[i].color = c;
        }
        c = txtIntro.color;
        c.a = 0;
        txtIntro.color = c;
    }

    private void Start () {
        StartCoroutine (IntroSecuence (imgs));
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            CallScene ();
        }
    }

    IEnumerator IntroSecuence (Image[] images) {
        Color c;

        if (a_Audio)
        {
            yield return new WaitForEndOfFrame();
            a_Audio.SetAudio(0, 0);
            yield return new WaitForSeconds(4f);
            a_Audio.SetAudio(0, 1);
            yield return new WaitForSeconds(4f);
        }

        for (int i = 0; i < images.Length; i++) {
            c = images[i].color;
            c.a = 0;
            images[i].color = c;
            while (c.a < 1) {
                yield return null;
                c.a = Mathf.MoveTowards (c.a, 1, Time.deltaTime / timeFade);
                images[i].color = c;
            }
            c.a = 1;
            imgs[i].color = c;
            yield return new WaitForSeconds (timeOnScreen);
            while (c.a > 0) {
                yield return null;
                c.a = Mathf.MoveTowards (c.a, 0, Time.deltaTime / timeFade);
                images[i].color = c;
            }
            yield return new WaitForSeconds (timeBetwenImages);
        }

        StartCoroutine (EnterSecuence (txtIntro));
    }

    IEnumerator EnterSecuence (TMPro.TextMeshProUGUI txt) {
        Color c;
        c = txt.color;
        c.a = 0;
        while (c.a < 1) {
            yield return null;
            c.a = Mathf.MoveTowards (c.a, 1, Time.deltaTime / timeFadeTxt);
            txt.color = c;
        }
        c.a = 1;
        while (c.a > 0) {
            yield return null;
            c.a = Mathf.MoveTowards (c.a, 0, Time.deltaTime / timeFadeTxt);
            txt.color = c;
        }
        StartCoroutine (EnterSecuence (txt));
    }

    public void CallScene () {
        Debug.Log ("Enter");
        SceneManager.LoadScene (sceneName);
    }
}