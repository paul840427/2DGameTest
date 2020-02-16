using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //public CharacterController controller;
    public Animator animator;
    Rigidbody2D playerRigidbody2D;

    public float speedX;
    public float newSpeedX;

    public float horizontalDirection;

    float horizontalMove = 0f;
    bool jump = false;

    [Header("水平推力")]
    [Range(0,150)]
    public float xForce;

    //垂直速度
    float speedY;

    [Header("最大水平速度")]
    public float maxSpeedX;

    public void ControlSpeed()
    {
        speedX = playerRigidbody2D.velocity.x;
        speedY = playerRigidbody2D.velocity.y;
        newSpeedX = Mathf.Clamp(speedX, -maxSpeedX, maxSpeedX);
        playerRigidbody2D.velocity = new Vector2(newSpeedX, speedY);
    }

    [Header("垂直向上推力")]
    public float yForce;

    public bool JumpKey
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    void TryJump()
    {
        if (IsGround && JumpKey)
        {
            playerRigidbody2D.AddForce(Vector2.up * yForce);
        }
    }

    [Header("感應地板的距離")]
    [Range(0, 0.5f)]
    public float distance;

    [Header("偵測地板的射線起點")]
    public Transform groundCheck;

    [Header("地面圖層")]
    public LayerMask groundLayer;

    public bool grounded;
    public bool IsGround
    {
        get
        {
            Vector2 start = groundCheck.position;
            Vector2 end = new Vector2(start.x, start.y - distance);
            Debug.DrawLine(start, end,Color.blue);
            grounded = Physics2D.Linecast(start, end, groundLayer);
            return grounded;
        }
    }
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void MovementX()
    {
        horizontalDirection = Input.GetAxis("Horizontal");
        playerRigidbody2D.AddForce(new Vector2(xForce * horizontalDirection,0));
    }

    // Update is called once per frame
    void Update () {
        MovementX();
        ControlSpeed();
        TryJump();
        horizontalMove = Input.GetAxisRaw("Horizontal") * speedX;
        animator.SetFloat("Speed",horizontalMove);
        //if (Input.GetButtonDown("jump"))
        //{
        //    jump = true;
        //    animator.SetBool("IsJumping", true);
        //}
    }

    //public void OnLanding()
    //{
    //    animator.SetBool("IsJumping", false);
    //}
}
