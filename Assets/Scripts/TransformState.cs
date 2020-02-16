using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformState : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform OriginalParent
    {
        get;
        set;
    }

    public Vector3 origin_scale { set; get; }

    void Awake()
    {
        this.OriginalParent = this.transform.parent;
        origin_scale = transform.localScale;
    }
}
