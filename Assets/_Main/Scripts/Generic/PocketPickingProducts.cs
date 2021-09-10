using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PocketPickingProducts : MonoBehaviour
{
    public int i_Index;
    public Text[] t_TxtToShow;
    public Text[] t_TxtToClean;
    public bool b_FirstTime = true;
    public List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> l_ListOfProducts;
    public GenericIco ico;
    public TutorialPicking tp;

    public void GetList(List<RandomProductsAndsShelfsSlots.PickingPocketProductInfo> l)
    {
        l_ListOfProducts = l;
    }

    public void ShowInfo(int index)
    {
        t_TxtToShow[0].text = l_ListOfProducts[index].shelfCode;
        t_TxtToShow[1].text = l_ListOfProducts[index].productCode;
        t_TxtToShow[2].text = l_ListOfProducts[index].quantity.ToString();
        t_TxtToShow[3].text = "Falta agregar este campo";
        ico.SetPos(l_ListOfProducts[index].shelfCode);
    }

    public void StartInfo()
    {
        if (b_FirstTime)
        {
            b_FirstTime = false;
            i_Index = 0;
            ShowInfo(i_Index);
        }
    }

    public bool NextItem()
    {
        i_Index++;
        for (int i = 0; i < t_TxtToClean.Length; i++)
        {
            t_TxtToClean[i].text = "";
        }
        if (i_Index < l_ListOfProducts.Count)
        {
            ShowInfo(i_Index);
        }

        if (i_Index == l_ListOfProducts.Count - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ShowActualIndex()
    {
        ShowInfo(i_Index);
    }
}