using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JOJOToastAI : MonoBehaviour
{
    Rigidbody2D monsterRigidbody2D;

    bool i = true;

    //移動次數
    int movetimes = 0;
    //等待秒數
    int waittimes = 0;
    //水平推力
    public float xForce = 150;

    //垂直推力
    [Header("垂直向上推力")]
    [Range(0, 150)]
    public float yForce;

    

    #region 怪物移動
    IEnumerator Movement()
    {
        //xForce = Random.Range(-150, 150);
        yield return new WaitForSeconds(waittimes);
        monsterRigidbody2D.AddForce(Vector2.left * xForce);
        monsterRigidbody2D.AddForce(Vector2.up * yForce);
        i = true;
        //yield return new WaitForSeconds(2);
    }
    #endregion


    void Start()
    {
        monsterRigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        movetimes = Random.Range(1, 10);
        waittimes = Random.Range(1, 5);
        xForce = Random.Range(-150, 150);
        for (int a = 0; a < movetimes; a++)
        {
            Debug.Log(movetimes);
            Debug.Log(xForce);
            while (i)
            {
                StartCoroutine("Movement");
                i = false;
            }
        }
    }
    
}
