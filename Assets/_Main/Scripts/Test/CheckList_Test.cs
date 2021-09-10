using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckList_Test : MonoBehaviour
{
    int i_index = -1;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Test();
        }
    }

    void Test()
    {
        switch (i_index)
        {
            case 0:
                Checklist.Set("Epps", "Camisa", true);
                break;
            case 1:
                Checklist.Set("Epps", "Pantalon", true);

                break;
            case 2:
                Checklist.Set("Epps", "Botas", true);

                break;
            case 3:
                Checklist.Set("Epps", "Casco", true);

                break;
        }
        Debug.Log(Checklist.Get("Epps", "Camisa"));
        Debug.Log(Checklist.Get("Epps", "Pantalon"));
        Debug.Log(Checklist.Get("Epps", "Botas"));
        Debug.Log(Checklist.Get("Epps", "Casco"));
        i_index++;
    }
}
