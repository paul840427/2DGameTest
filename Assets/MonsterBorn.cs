using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBorn : MonoBehaviour
{

    //JOJO物件
    public GameObject JOJO;
    //生成範圍
    public int xRangeStart = 0;
    public int xRangeEnd = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 2; i++)
        {
            int xRange = Random.Range(xRangeStart, xRangeEnd);
            Instantiate(JOJO, new Vector3(xRange, 2, 0), new Quaternion(0, 0, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Monster_Born();
    }
}
