using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    public List<Renderer> Renderers;
    public Material alphaMaterial, diffuseMaterial;
    public bool blinking;
    float a;
    public bool blinkOnEnabled = true;

    public float min = .05f, max = .5f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void OnEnable()
    {
        //Renderer = GetComponent<MeshRenderer>();
        //if (Renderer == null)
        //    Renderer = GetComponent<Renderer>();
        if (blinkOnEnabled)
            StartBlinking();
    }


    [ContextMenu("StartBlinking")]
    public void StartBlinking()
    {
        Renderers.ForEach(r => { r.sharedMaterial = alphaMaterial; });
        blinking = true;
    }

    [ContextMenu("StopBlinking")]
    public void StopBlinking()
    {
        Renderers.ForEach(r => { r.sharedMaterial = diffuseMaterial; });
        //Renderer.sharedMaterial = diffuseMaterial;
        blinking = false;
    }

    float frame = 0f;
    public float speedRate = .05f;
    void Update()
    {
        if (frame == 1000f)
            frame = 0f;

        if (!blinking)
            return;
        float sin = Mathf.Sin(frame) + 1f;
        float b = sin / 2f;
        a = min + (b * Mathf.Abs(max - min));

        Renderers.ForEach(r =>
        {
            Color c = r.sharedMaterial.GetColor("_Color");
            //a = Mathf.PingPong(Time.time, max);
            //a = Mathf.PingPong(Time.time, Mathf.Max(min, max));
            //a = Mathf.Sin(Time.time);
            r.sharedMaterial.SetColor("_Color", new Color(c.r, c.g, c.b, a));
        });
        frame += speedRate;

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    StartBlinking();
        //if (Input.GetKeyDown(KeyCode.Space))
        //    StopBlinking();

    }
}
