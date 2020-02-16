using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameEvent : MonoBehaviour
{
    private playerControls placon;
    public GameObject cointext;
    int countcoin = 0;
    // Start is called before the first frame update
    void Start()
    {
        placon = GetComponent<playerControls>();
    }

    // Update is called once per frame
    void Update()
    {
    }
        

    private void OnTriggerStay2D(Collider2D collision)
    {
        string obj = (collision.gameObject.name);

        //string pattern = @"\bcoin\w+\b";
        //Regex rgx = new Regex(pattern);
        //foreach (Match match in rgx.Matches(obj))
        {
            switch (obj)
            {
                //case ("EnterPoint"):
                //    if (placon.downtube == true)
                //    {
                //        transform.Translate(Vector3.forward * 1);
                //    }
                //    break;

                case ("coin"):
                    collision.gameObject.SetActive(false);
                    countcoin++;
                    cointext.GetComponent<Text>().text = "" + countcoin;
                    break;
            }
        }
        //if (collision.gameObject.name == "EnterPoint")
        //{
        //    if (placon.downtube == true)
        //    {
        //        transform.Translate(Vector3.forward * 1);
        //    }
        //}

    }
}
