using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = System.DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss");
        Debug.Log(myText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
