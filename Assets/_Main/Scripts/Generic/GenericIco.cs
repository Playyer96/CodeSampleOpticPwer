using System;
using System.Collections;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.UI;

public class GenericIco : MonoBehaviour
{
    public ShelfInvoice[] estanterias;
    public Sprite[] sprites;
    public Image img;
    public float frameTime;
    public Vector3 offset;
    public Transform icono;
    public CanvasManager cm;
    private Vector3 v;
    public SpectraUISettings settings;
    
    private void Start()
    {
        estanterias = FindObjectsOfType<ShelfInvoice>();
        StartCoroutine(AnimateIco());
        HideIco();
    }

    private void Update()
    {
        transform.rotation = cm.DesireRotation(transform);
    }

    public IEnumerator AnimateIco()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            img.sprite = sprites[i];
            yield return new WaitForSeconds(frameTime);
        }

        StartCoroutine(AnimateIco());
    }

    public void SetPos(string id)
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        
        v = offset;
        if (id == "12153A5A" || id == "12153A5C" || id == "12153A4E" || id == "1213A5G")
        {
            float z = offset.z;
            z = z * -1f;
            v.z = z;
        }
        
        for (int i = 0; i < estanterias.Length; i++)
        {
            if (estanterias[i].Data.shelfId == id)
            {
                transform.position = estanterias[i].transform.position + v;
                icono.gameObject.SetActive(true);
            }
        }
    }

    public void HideIco()
    {
        icono.gameObject.SetActive(false);
    }
    
}
