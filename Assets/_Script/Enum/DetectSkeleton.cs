using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectSkeleton
{
    None,             // 無偵測

    Game1,            // 第一關遊戲偵測的動作
    RaiseTwoHand,     // 舉雙手
    SingleJump,       // 單腳跳
    LeftSingleJump,   // 左單腳跳
    RightSingleJump,  // 右單腳跳

    Game2,            // 第二關遊戲偵測的動作
    SlidingSideShift, // 滑步側移
    ThrowBall,        // 丟球
    LeftThrowBall,    // 左丟球
    RightThrowBall,   // 右丟球

    Game3,            // 第三關遊戲偵測的動作
    Sit,              // 蹲
    Kick,             // 踢
    LeftKick,         // 左踢
    RightKick,        // 右踢

    Game4,            // 第四關遊戲偵測的動作
    Run,              // 跑
    CrossJump,        // 跨跳
    LeftCrossJump,    // 左跨跳
    RightCrossJump,   // 右跨跳

    Game5,            // 第五關遊戲偵測的動作
    Hit,              // 打擊
    LeftHit,          // 左打擊
    RightHit,         // 右打擊
    DoubleJump,       // 雙腳跳

    Game6,            // 第六關遊戲偵測的動作
    Dribble,          // 拍球
    LeftDribble,      // 左拍球
    RightDribble,     // 右拍球
    SingleStand,      // 單腳站
    LeftSingleStand,  // 左單腳站
    RightSingleStand, // 右單腳站
}
