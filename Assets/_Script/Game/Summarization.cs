using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summarization : MonoBehaviour
{
    public GameObject menu;
    public Button back;

    // 穩定性
    public Button stability;
    // 操作性
    public Button manipulation;
    // 移動性
    public Button locomotor;
    // 1~3 關得分
    public Button game1_3;
    // 4~6 關得分
    public Button game4_6;

    public GameObject title1;
    public GameObject score1;
    public GameObject title2;
    public GameObject score2;
    public GameObject title3;
    public GameObject score3;
    public GameObject title4;
    public GameObject score4;
    public GameObject title5;
    public GameObject score5;

    readonly int N_ACTION = 3;
    GameObject[] titles;
    GameObject[] scores;

    // Start is called before the first frame update
    void Start()
    {
        titles = new GameObject[] { title1, title2, title3, title4, title5 };
        scores = new GameObject[] { score1, score2, score3, score4, score5 };

        //resetScore();
        setStability();

        GameInfo.game_count = new Dictionary<int, int>();

        for (int i = 1; i <= 6; i++)
        {
            GameInfo.game_count.Add(i, 0);
        }

        stability.onClick.AddListener(setStability);
        manipulation.onClick.AddListener(setManipulation);
        locomotor.onClick.AddListener(setLocomotor);

        game1_3.onClick.AddListener(setGame1_3Score);
        game4_6.onClick.AddListener(setGame4_6Score);

        back.onClick.AddListener(()=> {
            menu.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void resetScore()
    {
        for(int i = 0; i < titles.Length; i++)
        {
            setScore(titles[i], scores[i], "", "");
        }
    }

    void setStability()
    {
        resetScore();

        setMovements(new DetectSkeleton[] {
            DetectSkeleton.RaiseTwoHand,
            DetectSkeleton.Sit,
            DetectSkeleton.SingleStand,
        });
    }

    void setManipulation()
    {
        resetScore();

        setMovements(new DetectSkeleton[] {
            DetectSkeleton.Hit,
            DetectSkeleton.Kick,
            DetectSkeleton.ThrowBall,
            DetectSkeleton.Dribble,
        });
    }

    void setLocomotor()
    {
        resetScore();

        setMovements(new DetectSkeleton[] {
            DetectSkeleton.Run,
            DetectSkeleton.SingleJump,
            DetectSkeleton.DoubleJump,
            DetectSkeleton.CrossJump,
            DetectSkeleton.SlidingSideShift,
        });
    }

    void setMovements(DetectSkeleton[] skeletons)
    {
        int len = skeletons.Length;
        for (int i = 0; i < len; i++)
        {
            setMovement(titles[i], scores[i], skeletons[i]);
        }
    }

    void setMovement(GameObject _title_text, GameObject _score_text, DetectSkeleton _skeleton)
    {
        // NullReferenceException
        try
        {
            setScore(
                _title_text,
                _score_text,
                ScoreBoard.movement_name[_skeleton],
                formatActionScore(_skeleton));
        }
        catch (NullReferenceException nre)
        {
            print(nre.Message);
            setScore(
                _title_text,
                _score_text,
                ScoreBoard.movement_name[_skeleton],
                formatActionScore(0));
        }
        
    }

    void setScore(GameObject _title_text, GameObject _score_text, string _title, string _score)
    {
        _title_text.GetComponent<Text>().text = _title;
        _score_text.GetComponent<Text>().text = _score;
    }

    string formatActionScore(DetectSkeleton _skeleton)
    {
        float _score;
        switch (_skeleton)
        {
            case DetectSkeleton.SingleJump:
                _score = GameInfo.movement_count[DetectSkeleton.LeftSingleJump] +
                    GameInfo.movement_count[DetectSkeleton.RightSingleJump];
                break;
            default:
                _score = GameInfo.movement_count[_skeleton];
                break;
        }

        if (_score > N_ACTION)
        {
            _score = N_ACTION;
        }

        return string.Format("{0} / {1}", _score, N_ACTION);
    }

    string formatActionScore(int _score)
    {
        if(_score > N_ACTION)
        {
            _score = N_ACTION;
        }

        return string.Format("{0} / {1}", _score, N_ACTION);
    }

    void setGame1_3Score()
    {
        resetScore();

        setScore(
                title1,
                score1,
                ScoreBoard.movement_name[DetectSkeleton.Game1],
                GameInfo.game_count[1].ToString());

        setScore(
            title2,
            score2,
            ScoreBoard.movement_name[DetectSkeleton.Game2],
            GameInfo.game_count[2].ToString());

        setScore(
            title3,
            score3,
            ScoreBoard.movement_name[DetectSkeleton.Game3],
            GameInfo.game_count[3].ToString());
    }

    void setGame4_6Score()
    {
        resetScore();

        setScore(
                title1,
                score1,
                ScoreBoard.movement_name[DetectSkeleton.Game4],
                GameInfo.game_count[4].ToString());

        setScore(
            title2,
            score2,
            ScoreBoard.movement_name[DetectSkeleton.Game5],
            GameInfo.game_count[5].ToString());

        setScore(
            title3,
            score3,
            ScoreBoard.movement_name[DetectSkeleton.Game6],
            GameInfo.game_count[6].ToString());
    }
}
