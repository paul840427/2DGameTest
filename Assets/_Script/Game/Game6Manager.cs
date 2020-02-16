using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game6Manager : GameManager
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

    Dictionary<int, GameItem[]> first_dict = new Dictionary<int, GameItem[]>
    {
        { 0, new GameItem[]{GameItem.BlueDefaultStone5 } },
        { 1, new GameItem[]{GameItem.RedDefaultStone5 }},
        { 2, new GameItem[]{GameItem.RedSharpStone8 }},
        { 3, new GameItem[]{GameItem.BlueDefaultStone6 }},
        { 4, new GameItem[]{GameItem.BlueDefaultStone8 }},
        { 5, new GameItem[]{GameItem.RedSharpStone5 }},
    };

    Dictionary<int, GameItem[]> answer_dict = new Dictionary<int, GameItem[]>
    {
        { 0, new GameItem[]{GameItem.BlueDefaultStone3 } },
        { 1, new GameItem[]{GameItem.RedDefaultStone4 }},
        { 2, new GameItem[]{GameItem.RedSharpStone4 }},
        { 3, new GameItem[]{GameItem.BlueDefaultStone5 }},
        { 4, new GameItem[]{GameItem.BlueDefaultStone6 }},
        { 5, new GameItem[]{GameItem.RedSharpStone3 }},
    };

    Dictionary<int, GameItem[]> left_dict = new Dictionary<int, GameItem[]>
    {
        { 0, new GameItem[]{GameItem.BlueDefaultStone2 } },
        { 1, new GameItem[]{GameItem.BlueDefaultStone1 }},
        { 2, new GameItem[]{GameItem.RedSharpStone4 }},
        { 3, new GameItem[]{GameItem.BlueSharpStone1 }},
        { 4, new GameItem[]{GameItem.BlueDefaultStone2 }},
        { 5, new GameItem[]{GameItem.RedSharpStone3 }},
    };

    Dictionary<int, GameItem[]> right_dict = new Dictionary<int, GameItem[]>
    {
        { 0, new GameItem[]{GameItem.RedDefaultStone2 } },
        { 1, new GameItem[]{GameItem.RedDefaultStone1 }},
        { 2, new GameItem[]{GameItem.RedDefaultStone4 }},
        { 3, new GameItem[]{GameItem.BlueDefaultStone1 }},
        { 4, new GameItem[]{GameItem.BlueDefaultStone3 }},
        { 5, new GameItem[]{GameItem.RedSharpStone2 }},
    };
    #endregion

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
        else
        {
            print("detect_manager is existed.");
        }

        if (audio_manager == null)
        {
            print(string.Format("{0} is nu!!", "audio_manager"));
        }
        else
        {
            print("audio_manager is existed.");
        }

        //if (kinect == null)
        //{
        //    print(string.Format("{0} is nu!!", "kinect"));
        //}
        //else
        //{
        //    print("kinect is existed.");
        //}

        #region Game Config
        config = GameConfig.loadGameConfig();

        // 總共有幾輪
        ROUND = config.round5;

        // 每一輪要吃多少次
        NUMBER = config.number5;

        // 每一輪的時間上限
        ROUND_TIME = config.round_time5;

        // 題目出現後緩衝數秒再開始偵測
        QUESTION_BUFFER = config.question_buffer5;

        // 每次偵測的間隔時間
        DETECT_INTERVAL_TIME = config.detect_interval_time5;

        // 圈叉呈現時間
        YESNO_DISPLAY_TIME = config.yesno_display_time5;

        // 回饋呈現時間
        FEEDBACK_DISPLAY_TIME = config.feedback_display_time5;

        //回合結束等待時間
        ROUND_INTERVAL_TIME = config.round_interval_time5;

        // 分數呈現時間
        SCORE_DISPLAY_TIME = config.score_display_time5;

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
    #endregion Life cycle end

    #region 遊戲機制
    IEnumerator gameStart()
    {
        print("gameStart");

        // 記錄檔的時間戳
        file_id = DateTime.Now.ToString("HH-mm-ss-ffff");

        // 設置關卡
        detect_manager.setGameStage(6);

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
        audio_manager.playOnLoop(AudioManager.AudioName.Game3);
        // StageImage 中要放入該關卡的圖片
        yield return new WaitForSeconds(ROUND_INTERVAL_TIME);

        layout.showStage(false);
        DetectSkeleton? result;

        //關卡機制
        while (round < ROUND)
        {
            // 告訴玩家這是第幾題 & 總題數
            //layout.setQuestionInfo(round + 1, ROUND);

            // 呈現題目
            layout.showQuestion(true);
            // 根據輪數，切換固定題目與選項
            layout.setGame6(round);

            number = 0;

            // round_time 歸零
            round_time = 0f;

            #region 一輪
            while (number < NUMBER && round_time < ROUND_TIME)
            {
                // 題目出現後緩衝數秒再開始偵測
                yield return new WaitForSeconds(QUESTION_BUFFER);

                // 開始偵測
                detect_manager.startMatch(DetectSkeleton.Game6);
                yield return new WaitForSeconds(Time.deltaTime);
                print(detect_manager.getStage());
                print(string.Format("story:{0}, stage:{1}, number:{2}", 
                    detect_manager.getStoryStage(),
                    detect_manager.getGameStage(),
                    detect_manager.getNumberStage()));

                game_record.start(
                    detect_manager.getGuid(),
                    detect_manager.getStage(),
                    getFirst(round),
                    "-",
                    getSecond(),
                    getAnswer(round));

                while ((result = detect_manager.whichOnePass(DetectSkeleton.Game6)) == null && number < NUMBER && round_time < ROUND_TIME)
                {
                    round_time += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                // 無論是否做到動作，回合數皆 +1
                number++;

                // 根據玩家做的動作給予回饋
                yield return StartCoroutine(playerSelection(result, round + 1));

                // 結束偵測
                detect_manager.endMatch(DetectSkeleton.Game6, file_id);

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
        score_board.setLevel(6);

        int count = GameInfo.movement_count[DetectSkeleton.LeftDribble] + GameInfo.movement_count[DetectSkeleton.RightDribble];
        score_board.setScore1(DetectSkeleton.Dribble, count, ROUND / 2);

        count = GameInfo.movement_count[DetectSkeleton.LeftSingleStand] + GameInfo.movement_count[DetectSkeleton.RightSingleStand];
        score_board.setScore2(DetectSkeleton.SingleStand, count, ROUND / 2);

        score_board.setScore3(DetectSkeleton.Game6, correct, ROUND);

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
    #endregion 遊戲機制 end

    #region 根據玩家選擇的動作，給予回饋
    IEnumerator playerSelection(DetectSkeleton? selection, int _round)
    {
        switch (selection)
        {
            //玩家做了 Hit
            case DetectSkeleton.LeftDribble:
            case DetectSkeleton.RightDribble:
                GameInfo.movement_count[(DetectSkeleton)selection] += 1;

                selection_messege = string.Format("動作: {0}", (DetectSkeleton)selection);

                selected_items = left_dict[_round - 1];
                game_record.setMovementSelection((DetectSkeleton)selection, selected_items);

                //選擇左邊的答案
                yield return StartCoroutine(afterSelection("left", _round));
                break;

            //玩家做了 DoubleJump 或 RightKick
            case DetectSkeleton.LeftSingleStand:
            case DetectSkeleton.RightSingleStand:
                GameInfo.movement_count[(DetectSkeleton)selection] += 1;

                selection_messege = string.Format("動作: {0}", (DetectSkeleton)selection);

                selected_items = right_dict[_round - 1];
                game_record.setMovementSelection((DetectSkeleton)selection, selected_items);

                //選擇右邊的答案
                yield return StartCoroutine(afterSelection("right", _round));
                break;

            // 超過時間未做動作
            default:
                selected_items = new GameItem[] { GameItem.None };
                game_record.setMovementSelection(DetectSkeleton.None, selected_items);
                yield return StartCoroutine(afterSelection("none", _round));
                break;
        }
    }

    //動作偵測結束後的關卡後續處理
    IEnumerator afterSelection(string select, int _round)
    {
        switch (_round)
        {
            case 1:
            case 3:
            case 5:
                if (select == "left")
                {
                    // 關閉左邊選項
                    layout.showLeftOption(false);

                    // 開啟正確回饋音效
                    audio_manager.modifyVolumn(CORRECT_VOL, 2);
                    audio_manager.play(AudioManager.AudioName.Correct, 2);
                    layout.correctFeedback(true);
                    correct++;

                    game_record.end(GameItem.Correct, correct, ROUND);
                }
                else
                {
                    if (select == "right")
                    {
                        // 關閉右邊選項
                        layout.showRightOption(false);
                    }

                    audio_manager.modifyVolumn(WRONG_VOL, 2);
                    audio_manager.play(AudioManager.AudioName.Wrong, 2);
                    layout.errorFeedback(true);

                    game_record.end(GameItem.Error, correct, ROUND / 2);
                }
                break;
            case 2:
            case 4:
            case 6:
                if (select == "right")
                {
                    layout.showRightOption(false);
                    // 開啟正確回饋音效
                    audio_manager.modifyVolumn(CORRECT_VOL, 2);
                    audio_manager.play(AudioManager.AudioName.Correct, 2);
                    layout.correctFeedback(true);
                    correct++;

                    game_record.end(GameItem.Correct, correct, ROUND / 2);
                }
                else
                {
                    if (select == "left")
                    {
                        layout.showLeftOption(false);
                    }

                    audio_manager.modifyVolumn(WRONG_VOL, 2);
                    audio_manager.play(AudioManager.AudioName.Wrong, 2);
                    layout.errorFeedback(true);

                    game_record.end(GameItem.Error, correct, ROUND / 2);
                }
                break;
        }

        // 回饋出現 FEEDBACK_DISPLAY_TIME 秒
        yield return new WaitForSeconds(YESNO_DISPLAY_TIME);
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
    GameItem[] getFirst(int _round)
    {
        return first_dict[_round];
    }

    GameItem[] getSecond()
    {
        return new GameItem[] { GameItem.What };
    }

    GameItem[] getAnswer(int _round)
    {
        return answer_dict[_round];
    }
    #endregion
}
