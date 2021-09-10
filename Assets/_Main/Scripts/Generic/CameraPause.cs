using System;
using System.Collections;
using UnityEngine;

public class CameraPause : MonoBehaviour
{
    private PauseButton pause;
    private IEnumerator Start()
    {
        PauseButton.isPause = false;
        yield return new WaitForEndOfFrame();
        pause = FindObjectOfType<PauseButton>();
        pause.cameraPause = this.transform;
        gameObject.SetActive(false);
    }
}
