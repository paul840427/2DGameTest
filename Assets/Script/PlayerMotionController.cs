using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour {
    public Animator animator;

    public float horizontalDirection;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        horizontalDirection = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", horizontalDirection);
    }
}
