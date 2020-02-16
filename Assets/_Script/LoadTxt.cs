using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTxt : MonoBehaviour
{
    //public TextAsset TxtFile;   //建立TextAsset
    //private string Mytxt;       //用来存放文本内容
    // Start is called before the first frame update
    void Start()
    {
        //Mytxt = ((TextAsset)Resources.Load("Data")).text;
        TextAsset mytxt = Resources.Load("Data") as TextAsset;
        string[] strs = mytxt.text.Split('\n');
        foreach (string str in strs)
        {
            string[] s = str.Split(',');
            Debug.Log(s[0]);
            Debug.Log(s[1]);
            Debug.Log(s[2]);
            Debug.Log(s[3]);
            Debug.Log(s[4]);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
