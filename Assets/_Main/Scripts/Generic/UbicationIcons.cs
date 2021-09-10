using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class UbicationIcons : MonoBehaviour
{
    public LocationManager manager;
    public ReceptionInvoice RI;
    private CanvasManager canvas;
    public SpectraUISettings settings;

    private void Start()
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        manager = FindObjectOfType<LocationManager>();
        canvas = FindObjectOfType<CanvasManager>();
    }

    //Toma el reception invoice despues de escanear
    public void GetReceptionInvoice(ReceptionInvoice ri)
    {
        RI = ri;
    }

    //Luego de escanear la estanteria se muestra el objeto a escanear
    public void OnSetIcoScan()
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        canvas.isFollow = true;
        canvas.StartSetIco(9, 0);
        canvas.SetFollowTR(RI.transform);
    }

    public void HideIco()
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        StartCoroutine(canvas.SetIcono(-9, 0));
    }

    //Luego de ingresar la informacion en la pocket debe mostrar el producto a recoger
    public void OnSetIcoGrab()
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        canvas.isFollow = true;
        canvas.StartSetIco(4, 0);
        canvas.SetFollowTR(RI.transform);
    }

    //Luego de soltar elproducto en la estanteria debe revisar los prodyctos en el carro y mostrar los que van en la misma ubicacion
    public void OnSetGrabDople()
    {
        if (settings.experienMode == ExperienMode.Evaluacion)
        {
            return;
        }
        for (int i = 0; i < manager.receptionInvoices.Count; i++)
        {
            if (manager.receptionInvoices[i].Product.productId == RI.Product.productId && manager.receptionInvoices[i].StoredInUbication == false)
            {
                canvas.isFollow = true;
                canvas.StartSetIco(4, 0);
                canvas.SetFollowTR(manager.receptionInvoices[i].transform);
                return;
            }
        }
        canvas.StartSetIco(-4, 0);
    }
}