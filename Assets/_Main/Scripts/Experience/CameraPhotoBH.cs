using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

public class CameraPhotoBH : MonoBehaviour
{
    public static CameraPhotoBH Instance;
    Camera m_Camera;
    Texture2D m_ScreenShot;
    public int resWidth, resHeight;
   // public LayerMask l_LM;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        m_Camera = GetComponent<Camera>();
        m_Camera.enabled = false;
    }

    public Sprite TakePicture(Transform tr)
    {
        GameObject flash = GameObject.Find("Flash");
        if (flash != null)
            flash.SetActive(true);
        transform.position = tr.position;
        transform.rotation = tr.rotation;

        m_Camera.enabled = true;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 48);
        m_Camera.targetTexture = rt;
        m_ScreenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        m_Camera.Render();
        RenderTexture.active = rt;
        m_ScreenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        m_ScreenShot.Apply();
        m_Camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        m_Camera.enabled = false;
        if (flash != null)
            flash.SetActive(false);
        return Sprite.Create(m_ScreenShot, new Rect(0, 0, resWidth, resHeight), new Vector2(0, 0));
    }
}