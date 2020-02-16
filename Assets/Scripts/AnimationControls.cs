using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControls : MonoBehaviour
{
    Rigidbody2D playerRigidbody2D;

    //在這裡設置人物所會用到的動畫
    public Sprite[] idle;
    public float idleFrame;
    public Sprite[] run;
    public float runFrame;
    public Sprite[] jump;
    public float jumpFrame;
    public Sprite[] attack;
    public float attackFrame;


    float xSpeed = 0;
    float ySpeed = 0;

    //各種人物動畫的變量
    //private CharacterController controller;
    private SpriteRenderer render;
    private PlayerController placon;

    private float framecounter = 0.0f;

    private int idle_count = 0;
    private int run_count = 0;
    private int jump_count = 0;
    private int attack_count = 0;

    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isAnimat = false;

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        render = GetComponent<SpriteRenderer>();
        placon = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //檢查速度來判定腳色的動作
        //因為需要平面移動與跳躍的判定所以需要X軸與Y軸的速度
        xSpeed = placon.speedX;
        ySpeed = placon.speedY;
        //if (xSpeed < 0)
        //{
        //    xSpeed *= -1; //反方向速度移動
        //}

        if (placon.grounded) //如果角色位於地板
        {
            //如果人物回到了地面將停止jump動畫
            isJumping = false;

            if (xSpeed < 0.25f && xSpeed > -0.25f)
            {
                //如果X軸速度趨近0並且是在地面時撥放idle
                idleFrame += 0.16f;
                if (idleFrame > idle_count && idle_count < idle.Length)
                {
                    render.sprite = idle[idle_count];
                    idle_count += 1;
                }
                //讓動畫無限輪播
                if (idleFrame > idle.Length)
                {
                    idleFrame = 0.0f;
                    idle_count = 0;
                }
            }
            else
            {
                if (!isAnimat)
                {
                    //同樣在地面速度不是趨近於0的話就是走動並撥放run
                    runFrame += 0.16f;
                    if (runFrame > run_count && run_count < run.Length)
                    {
                        render.sprite = run[run_count];
                        run_count += 1;
                    }
                    //讓動畫無限輪播
                    if (runFrame > run.Length)
                    {
                        runFrame = 0.0f;
                        run_count = 0;
                    }
                }
            }

            if (!placon.AttackKey)
            {
                isAnimat = false;
                attackFrame = 0.0f;
                attack_count = 0;
            }
            else
            {
                isAnimat = true;
                attackFrame += 0.16f;
                if (attackFrame > attack_count && attack_count < attack.Length)
                {

                    render.sprite = attack[attack_count];
                    attack_count += 1;
                }
                //else
                //{
                //    placon.AttackKey = false;

                //}  
            }

        }
        else //如果角色位於空中
        {
            if (!isJumping && ySpeed > 0.5f) //重置跳躍數值
            {
                isJumping = true;
                jumpFrame = 0.0f;
                jump_count = 0;
            }
            if (isJumping) //執行跳躍動畫
            {
                jumpFrame += 0.16f;
                if (jumpFrame > jump_count && jump_count < jump.Length)
                {
                    render.sprite = jump[jump_count];
                    jump_count += 1;
                }
            }
        }
    }
}
