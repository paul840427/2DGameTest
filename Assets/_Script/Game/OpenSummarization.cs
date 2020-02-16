using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSummarization : MonoBehaviour
{
    public GameObject summarization_go;
    public GameObject menu;
    public Button summarization_button;

    // Start is called before the first frame update
    void Start()
    {
        summarization_button.onClick.AddListener(()=> {
            summarization_go.SetActive(true);
            menu.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
