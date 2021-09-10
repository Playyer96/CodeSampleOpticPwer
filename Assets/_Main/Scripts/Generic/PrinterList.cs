using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.Events;

public class PrinterList : MonoBehaviour
{
    public static List<DataBox> db;
    public PocketManager pm;
    public PocketFunctions pf;
    public int productAmmount;
    public UnityEvent e_OnPrintAll;
    public SpectraUISettings Settings;

    private void Start()
    {
        db = new List<DataBox>();
        if (e_OnPrintAll == null)
        {
            e_OnPrintAll = new UnityEvent();
        }
    }

    public void StartPrintInvoices()
    {
        StartCoroutine(PrintList(db));
    }

    public IEnumerator PrintList(List<DataBox> db)
    {
        for (int i = 0; i < db.Count; i++)
        {
            if (!db[i].b_hasPrinted)
            {
                Printer.instance.product = db[i].pi.Product;
                db[i].b_hasPrinted = true;
                if (Settings.experienMode == ExperienMode.Entrenamiento)
                {
                    if (i == 0)
                    {
                        Printer.instance.invoiceToPrint.GetComponent<DreamHouseStudios.VR.Interactable>().enabled =
                            false;
                    }
                    else
                    {
                        Printer.instance.invoiceToPrint.GetComponent<DreamHouseStudios.VR.Interactable>().enabled =
                            true;
                    }
                }

                Printer.instance.PrintInvoice();
                pf.ExternalCallPrint();
                yield return new WaitForSeconds(.56f);
                if (Settings.experienMode == ExperienMode.Evaluacion)
                {
                    if (i == productAmmount - 1)
                    {
                        e_OnPrintAll.Invoke();
                    }
                }
            }
        }
        pm.canScan = true;
    }
}