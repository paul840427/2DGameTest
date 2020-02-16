﻿using UnityEngine;
using System.Collections;

public class playerControls : MonoBehaviour {
	//走路速度
	public float runSpeed = 12.0f;
	//跳躍高度
	public float jumpHeight = 50.0f;
	//墜落死亡
	public float fall = -12;
	//音效.跳躍聲音
	public AudioClip jumpSound;
    //攻擊控制
    public bool attack = false;
    //下水管判定值
    public bool downtube = false;


    //撞
    private RaycastHit hit;
	//跳躍計時
	private float jumpCounter = 0.0f;
	//角色控制
	private CharacterController controller;
	//方向速度
	private Vector3 vel;
	//判斷正負用
	private float lookX;
	//是否可以控制
	private bool canControl = true;
	//是否可以撞到天花板
	private bool canCeiling = true;

    Rigidbody rig;
    

	void Start () {
		controller = GetComponent<CharacterController>();
		lookX = transform.localScale.x;

        //rig = controller.attachedRigidbody;
    }

    void Update () {
		
		//重力應用
		//如果人物不在地面將給予Y軸重力40
		//如果人物沒有重力就像在外太空,一躍起將會突破天際
		//否則給予人物Y軸-1的數值來穩定腳色站在地面上
		//如果今天想製作彈力球不仿輸入5左右的正數,存在重力的腳色將會不停的跳動
		if(!controller.isGrounded){
			jumpCounter += Time.deltaTime;
			vel.y -= Time.deltaTime*30;
		}else{
			jumpCounter = 0.0f;
			vel.y = -1;
		}
		
		//翻轉人物
		//X軸速度大於0人物要看向右方,小於0則-lookX多個負號改為左邊
		if(controller.velocity.x > 0){
			transform.localScale = new Vector3(lookX,transform.localScale.y,transform.localScale.z);
		}
		if(controller.velocity.x < 0){
			transform.localScale = new Vector3(-lookX,transform.localScale.y,transform.localScale.z);
           
		}
		
		//設定移動按鍵
		//鍵盤上下左右作為移動按鍵
		if(canControl){
			if(Input.GetKey("left") || Input.GetKey("right")){
				//如果按下左鍵往左走
				if(Input.GetKey(KeyCode.LeftArrow)){
					vel.x = -runSpeed;
				}
				//如果按下右鍵往右走
				if(Input.GetKey(KeyCode.RightArrow)){
					vel.x = runSpeed;
				}
				//如果沒有按的話就停止不動
			}else{
				vel.x = 0;
			}
			//跳躍的按鈕是上鍵
			//如果人物離地<0.1的話可以跳躍,這樣人物就算稍微離地的瞬間也能跳躍才不會玩起來很卡很刁難操作
			//跳躍則為Y軸所設定的跳躍高度,可以在unity內直接做編輯
			//最後是撥放跳躍音效
			if(Input.GetKey(KeyCode.LeftAlt)){
				if(jumpCounter < 0.1f){
					vel.y = jumpHeight;
					jumpCounter = 0.1f;
					GetComponent<AudioSource>().PlayOneShot(jumpSound);
				}
			}

            if (Input.GetKey(KeyCode.LeftControl))
            {
                    attack = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                downtube = true;
                print("按了");
            }
            else
            {
                downtube = false;
            }

        }
		
		//如果跳躍撞到東西時會停止跳躍的加速度並且重製碰撞的處理
		if ((controller.collisionFlags & CollisionFlags.Above) != 0 && canCeiling){
			canCeiling = false;
			vel.y = 0;
			StartCoroutine(resetCeiling());
		}

        //應用動作向量加速度到玩家
        if(transform.parent == gameObject.GetComponent<TransformState>().OriginalParent)
        {
            controller.Move(vel * Time.deltaTime);
        }
        else
        {
            print(string.Format("curr parent: {0}", transform.parent.name));
        }

        //Vector2 v2 = new Vector2(vel.x, vel.y);
        //rig.AddForce(v2);

        ////如果玩家落到限制高度以下將恢復原本位置
        //if (transform.position.y < fall)
        //{
        //    string lvlName = Application.loadedLevelName;
        //    Application.LoadLevel(lvlName);
        //}
    }
	//角色跳躍撞到東西時的處理時間,如果角色跳躍時間較長就需要注意角色是否會黏在天花板下不來
	public IEnumerator resetCeiling () {
		yield return new WaitForSeconds (0.25f);
		canCeiling = true;
	}
	//如果角色死亡將不能操作
	void died () {
		canControl = false;
		vel.x = 0;
	}
}
