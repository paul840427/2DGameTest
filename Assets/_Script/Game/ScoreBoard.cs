using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Image level;
    public Text title1;
    public Text score1;
    public Text title2;
    public Text score2;
    public Text title3;
    public Text score3;

    public Button ok;

    public static Dictionary<DetectSkeleton, string> movement_name = new Dictionary<DetectSkeleton, string>()
    {
        { DetectSkeleton.None, "無偵測"},
        { DetectSkeleton.Game1, "第 1 關"},
        { DetectSkeleton.Game2, "第 2 關"},
        { DetectSkeleton.Game3, "第 3 關"},
        { DetectSkeleton.Game4, "第 4 關"},
        { DetectSkeleton.Game5, "第 5 關"},
        { DetectSkeleton.Game6, "第 6 關"},

        { DetectSkeleton.RaiseTwoHand, "伸展"},
        { DetectSkeleton.SingleJump, "單腳跳"},
        { DetectSkeleton.LeftSingleJump, "左單腳跳"},
        { DetectSkeleton.RightSingleJump, "右單腳跳"},
        { DetectSkeleton.SlidingSideShift, "滑步側移"},
        { DetectSkeleton.ThrowBall, "投球"},
        { DetectSkeleton.LeftThrowBall, "左投球"},
        { DetectSkeleton.RightThrowBall, "右投球"},
        { DetectSkeleton.Sit, "蹲"},
        { DetectSkeleton.Kick, "踢"},
        { DetectSkeleton.LeftKick, "左踢"},
        { DetectSkeleton.RightKick, "右踢"},
        { DetectSkeleton.Run, "跑"},
        { DetectSkeleton.CrossJump, "跨跳"},
        { DetectSkeleton.LeftCrossJump, "左跨跳"},
        { DetectSkeleton.RightCrossJump, "右跨跳"},
        { DetectSkeleton.Hit, "打擊"},
        { DetectSkeleton.LeftHit, "左打擊"},
        { DetectSkeleton.RightHit, "右打擊"},
        { DetectSkeleton.DoubleJump, "雙腳跳"},
        { DetectSkeleton.Dribble, "拍球"},
        { DetectSkeleton.LeftDribble, "左拍球"},
        { DetectSkeleton.RightDribble, "右拍球"},
        { DetectSkeleton.SingleStand, "單腳站"},
        { DetectSkeleton.LeftSingleStand, "左單腳站"},
        { DetectSkeleton.RightSingleStand, "右單腳站"},
    };

    public void setLevel(int _level)
    {
        level.sprite = Resources.Load<Sprite>(string.Format("Game{0}", _level));
    }

    void setScore(int _order, DetectSkeleton _movement, int _score, int _total)
    {
        string _type = movement_name[_movement];

        #region 限制各項得分不高於總要求次數
        if(_score > _total)
        {
            _score = _total;
        }
        #endregion

        string _score_summary = string.Format("{0} / {1}", _score, _total);

        if (_order == 1)
        {
            title1.text = _type;
            score1.text = _score_summary;
        }
        else if (_order == 2)
        {
            title2.text = _type;
            score2.text = _score_summary;
        }
        else if (_order == 3)
        {
            title3.text = _type;
            score3.text = _score_summary;
        }
    }

    public void setScore1(DetectSkeleton _movement, int _score, int _total)
    {
        setScore(1, _movement, _score, _total);
    }

    public void setScore2(DetectSkeleton _movement, int _score, int _total)
    {
        setScore(2, _movement, _score, _total);
    }

    public void setScore3(DetectSkeleton _movement, int _score, int _total)
    {
        setScore(3, _movement, _score, _total);
    }
}