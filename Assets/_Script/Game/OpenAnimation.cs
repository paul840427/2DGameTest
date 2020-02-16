using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAnimation : MonoBehaviour
{
    public GameObject this_object;
    public GameObject animation_object;

    public Button open;

    // Start is called before the first frame update
    void Start()
    {
        open.onClick.AddListener(()=> {
            animation_object.SetActive(true);
            this_object.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
