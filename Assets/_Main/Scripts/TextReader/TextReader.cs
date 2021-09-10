using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class TextReader : MonoBehaviour
{
    public TextAsset[] documentos;
    public GrupoTextosReader[] gruposTextos;

    private void Awake()
    {
        gruposTextos = new GrupoTextosReader[documentos.Length];
        for (int i = 0; i < documentos.Length; i++)
        {
            string sourse = documentos[i].text;
            gruposTextos[i] = new GrupoTextosReader();
            gruposTextos[i].lineasTexto = sourse.Split("\n"[0]);
        }
    }

    public string GetText(int indexDocument, int indexLine)
    {
        string txt = "";
        return txt = gruposTextos[indexDocument].lineasTexto[indexLine].Replace("|", Environment.NewLine);
    }
}


[System.Serializable]
public class GrupoTextosReader
{
    public string[] lineasTexto;
}




