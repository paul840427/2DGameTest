using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseStage : MonoBehaviour
{
    //[SerializeField] GameObject choose_stage;
    [SerializeField] int stage;

    // Start is called before the first frame update
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(()=> {
            SceneManager.LoadScene(string.Format("_Scenes/Game{0}", stage));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
