using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidbody2D;
    private bool ismoveground;  //是否碰到移動方塊
    private bool isunderground; //是否碰到底層方塊

    //判斷正負用
    private float origin_lookX = 0.1476928f;
    private float origin_lookY = 0.1476928f;
    private float origin_lookZ = 0.1476928f;
    private float lookX;
    private float lookY;
    private float lookZ;

    public GameObject LifePointText; //生命計數
    private int countLife = 99;

    #region 角色水平控制

    [Header("目前的水平速度")]
    public float speedX;

    [Header("目前的水平方向")]
    public float horizontalDirection;

    const string HORIZONTAL = "Horizontal";

    [Header("水平推力")]
    [Range(0, 150)]
    public float xForce;
    
    [Header("最大水平速度")]
    public float maxSpeedX;


    public void ControlSpeed()
    {
        speedX = playerRigidbody2D.velocity.x;
        speedY = playerRigidbody2D.velocity.y;
        float newSpeedX = Mathf.Clamp(speedX, -maxSpeedX, maxSpeedX);
        playerRigidbody2D.velocity = new Vector2(newSpeedX, speedY);
    }
    #endregion

    #region 角色垂直控制

    //目前垂直速度
    public float speedY;

    [Header("垂直向上推力")]
    public float yForce;
    public bool JumpKey
    {
        get {
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

    //判斷角色是否在地面
    public bool grounded;
    bool IsGround
    {
        get
        {
            Vector2 start = groundCheck.position;
            Vector2 end = new Vector2(start.x, start.y - distance);

            Debug.DrawLine(start, end, Color.blue);
            grounded = Physics2D.Linecast(start, end, groundLayer);
            return grounded;
        }
    }
    #endregion

    #region 角色重生控制
    Vector2 RebornPoint;
    Vector3 CameraPoint;

    void Reborn()
    {
        //判斷是否有在底層方塊裡面
        isunderground = UnderGround.isUnderGround;
        if (isunderground)
        {
            countLife--;
            transform.position = new Vector2(RebornPoint.x, RebornPoint.y);
            Camera.main.transform.position = new Vector3(CameraPoint.x, CameraPoint.y, CameraPoint.z);
            UnderGround.isUnderGround = false;
            isunderground = false;
        }
    }
    #endregion

    #region 角色攻擊控制

    public bool AttackKey
    {
        get
        {
            return Input.GetKeyDown(KeyCode.LeftControl);
        }
    }



    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();

        RebornPoint = transform.position; //紀錄重生點
        CameraPoint = Camera.main.transform.position; //紀錄攝影機位置

        lookX = transform.localScale.x;
        lookY = transform.localScale.y;
        lookZ = transform.localScale.z;
    }

    /// <summary> 水平移動 </summary>
    void MovementX()
    {
        horizontalDirection = Input.GetAxis(HORIZONTAL);
        playerRigidbody2D.AddForce(new Vector2(xForce * horizontalDirection, 0));
    }

    // Update is called once per frame
    void Update()
    {
        MovementX();
        ControlSpeed();
        TryJump();
        Reborn();

        //顯示生命計數
        LifePointText.GetComponent<Text>().text = "" + countLife;

        #region 根據踩的地板切換玩家大小
        //判斷是否有在移動方塊裡面
        ismoveground = moveground.stayground;

        bool switchstatus = false; //用來判斷是否有改變值

        Debug.Log(ismoveground);
        if (!ismoveground)
        {
            lookX = origin_lookX;
            lookY = origin_lookY;
            lookZ = origin_lookZ;
            switchstatus = false;
        }
        else if (ismoveground && !switchstatus)
        {
            if (transform.localScale.x < 0)
            {
                lookX = transform.localScale.x * -1;
            }
            else
                lookX = transform.localScale.x;
            lookY = transform.localScale.y;
            lookZ = transform.localScale.z;
            switchstatus = true;
        }
        #endregion


        #region 左右翻轉
        //翻轉人物
        //X軸速度大於0人物要看向右方,小於0則-lookX多個負號改為左邊
        if (speedX > 0)
        {
            transform.localScale = new Vector3(lookX, lookY, lookZ);
            Debug.Log("lookX: " + lookX + " y:"+ transform.localScale.y+ " z: "+ transform.localScale.z);
        }
        if (speedX < 0)
        {
            transform.localScale = new Vector3(-lookX, lookY, lookZ);
            Debug.Log("lookX: " + lookX + " y:" + transform.localScale.y + " z: " + transform.localScale.z);
        }
        //speedX = playerRigidbody2D.velocity.x;
        #endregion
    }
}
