using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveground : MonoBehaviour
{
    TransformState state;
    public float movetimes;
    int a = 0;
    static public bool stayground;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        //StartCoroutine("ground01");
    }
    //IEnumerator ground01()
    //{
    //    if (a == 0)
    //    {
    //        transform.Translate(new Vector2(0.2f, 0));
    //        yield return new WaitForSeconds(movetimes);
    //        a++;
    //    }
    //    else
    //    {
    //        transform.Translate(new Vector2(-0.2f, 0));
    //        yield return new WaitForSeconds(movetimes);
    //        a = 0;
    //    }
    //}

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            stayground = true;
            state = collision.gameObject.GetComponent<TransformState>();
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //state = collision.gameObject.GetComponent<TransformState>();
            collision.gameObject.transform.SetParent(null);
            stayground = false;
        }
    }

    
    //private void OnTriggerEnter(Collider c)
    //{
    //    if(c.gameObject.name == "Player")
    //    {
    //        state = c.gameObject.GetComponent<TransformState>();
    //        var target = c.gameObject.transform;
    //        target.SetParent(gameObject.transform);
    //        //target.localScale = state.origin_scale;
    //    }
    //}

    //private void OnTriggerExit(Collider c)
    //{
    //    if (c.gameObject.name == "Player")
    //    {
    //        var target = c.gameObject.transform;
    //        var original = state.OriginalParent;
    //        target.SetParent(original);
    //    }
    //}
}
