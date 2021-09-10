using UnityEngine;

public class FpsCounter : MonoBehaviour {

    [SerializeField] TMPro.TextMeshProUGUI fpsCounterText;

    float fps;

    private void Update () {

        fps = 1 / Time.deltaTime;

        if (fpsCounterText)
            fpsCounterText.text = fps.ToString ("f2");
    }
}