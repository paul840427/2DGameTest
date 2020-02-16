using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game2Manager : GameManager
{
    #region Insepctor
    // 動作偵測腳本
    public DetectManager detect_manager;

    // 音效管理
    public AudioManager audio_manager;

    // kinect
    //public GameObject kinect;
    #endregion

    #region Game Config
    GameConfig config;

    // 總共有幾題
    int ROUND;
    // 目前過了幾題
    int round;
    // 每一題要回答多少次
    int NUMBER;
    // 回答次數
    int number;
    // 正確題數
    int correct;
    // 每一輪的時間上限
    float ROUND_TIME;
    // 每一輪的時間
    float round_time;
    // 題目出現後緩衝數秒再開始偵測
    float QUESTION_BUFFER;
    // 每次偵測的間隔時間
    float DETECT_INTERVAL_TIME;
    // 圈叉呈現時間
    float YESNO_DISPLAY_TIME;
    // 回饋呈現時間
    float FEEDBACK_DISPLAY_TIME;
    // 回合等待時間
    float ROUND_INTERVAL_TIME;
    // 分數呈現時間
    float SCORE_DISPLAY_TIME;

    #region 音樂大小設定
    // 背景音量
    float BG_VOL;
    // 正確回饋音量
    float CORRECT_VOL;
    // 錯誤回饋音量
    float WRONG_VOL;
    // 成功回饋音量
    float SUCCESS_VOL;
    // 失敗回饋音量
    float FAIL_VOL;
    #endregion
    #endregion

    #region 數據紀錄
    // 記錄檔的時間戳
    string file_id;

    ScoreBoard score_board;

    GameRecord game_record;

    GameItem[] selected_items;

    Dictionary<int, GameItem[]> apple_dict = new Dictionary<int, GameItem[]>
    {
        {1, new GameItem[]{ GameItem.Apple1}},
        {2, new GameItem[]{ GameItem.Apple2}},
        {3, new GameItem[]{ GameItem.Apple3}},
        {4, new GameItem[]{ GameItem.Apple4}},
        {5, new GameItem[]{ GameItem.Apple5}},
        {6, new GameItem[]{ GameItem.Apple6}},
        {7, new GameItem[]{ GameItem.Apple7}},
        {8, new GameItem[]{ GameItem.Apple8}},
        {9, new GameItem[]{ GameItem.Apple9}}
    };

    Dictionary<int, GameItem[]> number_dict = new Dictionary<int, GameItem[]>
    {
        {1, new GameItem[]{ GameItem.One}},
        {2, new GameItem[]{ GameItem.Two}},
        {3, new GameItem[]{ GameItem.Three}},
        {4, new GameItem[]{ GameItem.Four}},
        {5, new GameItem[]{ GameItem.Five}},
        {6, new GameItem[]{ GameItem.Six}},
        {7, new GameItem[]{ GameItem.Seven}},
        {8, new GameItem[]{ GameItem.Eight}},
        {9, new GameItem[]{ GameItem.Nine}}
    };
    #endregion

    // 目標數字
    int target;
    // 左選項數值
    int left_number;
    // 右選項數值
    int right_number;
    // 玩家所選數值(累加)
    int answer;

    #region Life cycle
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (detect_manager == null)
        {
            print(string.Format("{0} is nu!!", "detect_manager"));
        }

        if (audio_manager == null)
        {
            print(string.Format("{0} is nu!!", "audio_manager"));
        }

        //if (kinect == null)
        //{
        //    print(string.Format("{0} is nu!!", "kinect"));
        //}

        #region Game Config
        config = GameConfig.loadGameConfig();

        // 總共有幾輪
        ROUND = config.round1;

        // 每一輪要吃多少次
        NUMBER = config.number1;

        // 每一輪的時間上限
        ROUND_TIME = config.round_time1;

        // 題目出現後緩衝數秒再開始偵測
        QUESTION_BUFFER = config.question_buffer1;

        // 每次偵測的間隔時間
        DETECT_INTERVAL_TIME = config.detect_interval_time1;

        // 圈叉呈現時間
        YESNO_DISPLAY_TIME = config.yesno_display_time1;

        // 回饋呈現時間
        FEEDBACK_DISPLAY_TIME = config.feedback_display_time1;

        //回合結束等待時間
        ROUND_INTERVAL_TIME = config.round_interval_time1;

        // 分數呈現時間
        SCORE_DISPLAY_TIME = config.score_display_time1;

        #region 音樂大小設定
        // 背景音量
        BG_VOL = config.bg_vol1;

        // 正確回饋音量
        CORRECT_VOL = config.correct_vol1;

        // 錯誤回饋音量
        WRONG_VOL = config.wrong_vol1;

        // 成功回饋音量
        SUCCESS_VOL = config.success_vol1;

        // 失敗回饋音量
        FAIL_VOL = config.fail_vol1;
        #endregion
        #endregion

        #region 數據紀錄
        // story 1：小紅帽
        detect_manager.setStoryStage(1);

        // 動作正確次數-計數用字典
        //resetCount();

        game_record = new GameRecord();
        #endregion

        StartCoroutine(gameStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 50;
        //GUILayout.Label(string.Format("\n\ntime:{0:F4}", round_time));
    }
    #endregion

    IEnumerator gameStart()
    {
        print("gameStart");

        // 記錄檔的時間戳
        file_id = DateTime.Now.ToString("HH-mm-ss-ffff");

        // 設置關卡
        detect_manager.setGameStage(2);

        #region Game Config
        // 目前玩了幾輪
        round = 0;

        // 正確吃多少次
        number = 0;

        // 正確題數
        correct = 0;

        // 每一輪的時間
        round_time = 0;
        #endregion

        yield return new WaitForSeconds(Time.deltaTime);

        StartCoroutine(gamePlaying());
    }

    IEnumerator gamePlaying()
    {        
        print("gamePlaying");

        // 開始播放背景音樂
        audio_manager.modifyVolumn(BG_VOL);
        audio_manager.playOnLoop(AudioManager.AudioName.Game1);
        yield return new WaitForSeconds(ROUND_INTERVAL_TIME);

        layout.showStage(false);
        DetectSkeleton? result;
        string path;

        //關卡機制
        while (round < ROUND)
        {
            // 告訴玩家這是第幾題 & 總題數
            //layout.setQuestionInfo(round + 1, ROUND);
            layout.setOperator("+");

            // create question 3 ~ 9
            target = Random.Range(3, 10);
            print(string.Format("target number: {0}", target));
            path = string.Format("GameImage/Number{0}", target);
            layout.setTarget(path);

            number = 0;
            answer = 0;

            // round_time 歸零
            round_time = 0f;

            #region 一輪
            while (number < NUMBER && round_time < ROUND_TIME)
            {
                // 告訴玩家目前是第幾回合
                layout.showRound(number, true);
                yield return new WaitForSeconds(2f);

                // 關閉回合提示
                layout.showRound(number, false);

                // 第 0 輪
                if (number == 0)
                {
                    // 呈現題目
                    layout.showQuestion(true);
                    layout.setOperator("+");

                    // 要回答的空格，以問號填入 且 框色為紅色，讓玩家清楚現在要做的事情
                    path = string.Format("GameImage/QuestionMark");
                    layout.setFirstNumber(path, Box.Red);

                    // 尚未要回答的空格，框色為黑色
                    path = string.Format("GameImage/None");
                    layout.setSecondNumber(path, Box.Black);

                    left_number = Random.Range(1, target);
                    right_number = Random.Range(1, target);
                    while (left_number == right_number)
                    {
                        left_number = Random.Range(1, target);
                    }
                }
                // 其他輪
                else
                {
                    // "不"呈現第 0 輪所選之答案
                    path = string.Format("GameImage/None");
                    layout.setFirstNumber(path, Box.Black);

                    // 要回答的空格，以問號填入，讓玩家清楚現在要做的事情
                    path = string.Format("GameImage/QuestionMark");
                    layout.setSecondNumber(path, Box.Red);

                    int temp = left_number;
                    left_number = target - right_number;
                    right_number = target - temp;
                }

                path = string.Format("_Apple/{0}", left_number);
                layout.setLeftOption(path);
                path = string.Format("_Apple/{0}", right_number);
                layout.setRightOption(path);

                // 題目出現後緩衝數秒再開始偵測
                yield return new WaitForSeconds(QUESTION_BUFFER);

                // 開始偵測
                detect_manager.startMatch(DetectSkeleton.Game2);
                game_record.start(
                    detect_manager.getGuid(),
                    detect_manager.getStage(),
                    getFirst(number),
                    "+",
                    getSecond(number),
                    getAnswer(target));

                while ((result = detect_manager.whichOnePass(DetectSkeleton.Game2)) == null && number < NUMBER && round_time < ROUND_TIME)
                {
                    round_time += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                // 無論是否做到動作，回合數皆 +1
                number++;

                // 根據玩家做的動作給予回饋
                yield return StartCoroutine(playerSelection(result, number, round));

                // 結束偵測
                detect_manager.endMatch(DetectSkeleton.Game2, file_id);

                selection_messege = "";
            }
            #endregion

            // 一輪結束，關閉題目 UI
            layout.showQuestion(false);

            // 無論是否答對題目，輪數皆 +1
            round++;

            // 兩輪之間的緩衝時間
            yield return new WaitForSeconds(ROUND_INTERVAL_TIME);
        }

        audio_manager.fadeOut();
        StartCoroutine(gameEnd());
    }

    IEnumerator gameEnd()
    {
        print("gameEnd");
        // 關閉背景音樂
        audio_manager.stop();

        // 刪除此關物件
        Destroy(detect_manager);
        Destroy(audio_manager);
        //Destroy(kinect);
        yield return new WaitForSeconds(1.5f);

        // 開始呈現分數
        score_board_go.SetActive(true);
        score_board = score_board_go.GetComponent<ScoreBoard>();

        // 設定關卡
        score_board.setLevel(2);

        // 取得 動作正確次數 與 遊戲得分
        int count = GameInfo.movement_count[DetectSkeleton.SlidingSideShift];
        score_board.setScore1(DetectSkeleton.SlidingSideShift, GameInfo.movement_count[DetectSkeleton.SlidingSideShift], ROUND);

        count = GameInfo.movement_count[DetectSkeleton.LeftThrowBall] + GameInfo.movement_count[DetectSkeleton.RightThrowBall];
        score_board.setScore2(DetectSkeleton.ThrowBall, count, ROUND);

        score_board.setScore3(DetectSkeleton.Game2, correct, ROUND);

        audio_manager.modifyVolumn(SUCCESS_VOL, 2);
        audio_manager.play(AudioManager.AudioName.Success, 2);
        yield return new WaitForSeconds(SCORE_DISPLAY_TIME + DETECT_INTERVAL_TIME);

        score_board.ok.onClick.AddListener(() => {
            // 關閉分數呈現
            score_board_go.SetActive(false);

            // 讓玩家選擇：再玩一次 、 開啟關卡選單 或 玩下一關
            end_of_game_go.SetActive(true);
        });
    }


    #region 根據玩家選擇的動作，給予回饋
    IEnumerator playerSelection(DetectSkeleton? selection, int _number, int _round)
    {
        switch (selection)
        {
            // 玩家做了 單腳跳 或 投球
            case DetectSkeleton.LeftSingleJump:
            case DetectSkeleton.RightSingleJump:
            case DetectSkeleton.LeftThrowBall:
            case DetectSkeleton.RightThrowBall:
                GameInfo.movement_count[(DetectSkeleton)selection] += 1;

                // 選擇左邊的答案
                answer += left_number;

                // 關閉左邊選項
                layout.showLeftOption(false);

                selection_messege = string.Format("動作: {0}, 數字: {1}, answer: {2}",
                    (DetectSkeleton)selection, left_number, answer);

                selected_items = apple_dict[left_number];
                game_record.setMovementSelection((DetectSkeleton)selection, selected_items);

                yield return StartCoroutine(afterSelection("left", _number));
                break;

            //玩家做了 RaiseTwoHand
            case DetectSkeleton.RaiseTwoHand:
            case DetectSkeleton.SlidingSideShift:
                GameInfo.movement_count[(DetectSkeleton)selection] += 1;

                // 選擇右邊的答案
                answer += right_number;

                // 關閉右邊選項
                layout.showRightOption(false);
                selection_messege = string.Format("動作: {0}, 數字: {1}, answer: {2}",
                    (DetectSkeleton)selection, right_number, answer);

                selected_items = apple_dict[right_number];
                game_record.setMovementSelection((DetectSkeleton)selection, selected_items);

                yield return StartCoroutine(afterSelection("right", _number));
                break;

            default:
                selected_items = new GameItem[] { GameItem.None };
                game_record.setMovementSelection(DetectSkeleton.None, selected_items);
                yield return StartCoroutine(afterSelection("none", _number));
                break;
        }
    }

    //動作偵測結束後的關卡後續處理
    IEnumerator afterSelection(string select, int num)
    {
        if (num == NUMBER)
        {
            if (answer == target)
            {
                layout.correctFeedback(true);
                correct++;

                // 開啟正確回饋音效
                audio_manager.modifyVolumn(CORRECT_VOL, 2);
                audio_manager.play(AudioManager.AudioName.Correct, 2);

                game_record.end(GameItem.Correct, correct, ROUND);
            }
            else
            {
                layout.errorFeedback(true);
                audio_manager.modifyVolumn(WRONG_VOL, 2);
                audio_manager.play(AudioManager.AudioName.Wrong, 2);

                game_record.end(GameItem.Error, correct, ROUND);
            }
            yield return new WaitForSeconds(YESNO_DISPLAY_TIME);
        }
        else
        {
            game_record.end(GameItem.None, correct, ROUND);
        }

        // 回饋出現 FEEDBACK_DISPLAY_TIME 秒
        yield return new WaitForSeconds(FEEDBACK_DISPLAY_TIME);

        layout.correctFeedback(false);
        layout.errorFeedback(false);

        if (select == "left")
        {
            // 玩家原本選擇左邊，回饋結束後關閉右選項
            layout.showRightOption(false);
        }
        else if (select == "right")
        {
            // 玩家原本選擇右邊，回饋結束後關閉左選項
            layout.showLeftOption(false);
        }
        else
        {
            // 左右都未選，結束後關閉選項
            layout.showRightOption(false);
            layout.showLeftOption(false);
        }

        // reset game_record
        game_record = new GameRecord();
    }
    #endregion

    #region game record
    GameItem[] getFirst(int _number)
    {
        if (_number == 0)
        {
            return new GameItem[] { GameItem.What };
        }
        else
        {
            return new GameItem[] { GameItem.None };
        }
    }

    GameItem[] getSecond(int _number)
    {
        if (_number == 0)
        {
            return new GameItem[] { GameItem.None };
        }
        else
        {
            return new GameItem[] { GameItem.What };
        }
    }

    GameItem[] getAnswer(int _target)
    {
        return number_dict[_target];
    }
    #endregion
}
