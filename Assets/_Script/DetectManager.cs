using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectManager : MonoBehaviour {
    [Header("玩家人物")]
    // 用於存取骨架
    public PoseModelHelper model_helper;

    [SerializeField] HintManager hint;

    #region 動作偵測
    [Header("選擇偵測何種動作")]
    public DetectSkeleton detect_skeleton = DetectSkeleton.None;

    [Header("各個動作 的 動作物件")]
    public Movement[] movements;

    // 動作名稱 與 動作物件 的字典
    Dictionary<DetectSkeleton, Movement> movement_map;

    // 用於比較與初始位置的前後左右上下距離
    Vector3? init_pos;

    // 歐拉距離之正確率(distance / threshold)
    float additional_accuracy;

    // 左右上下前後 距離門檻
    float x_distance_threshold;
    float y_distance_threshold;
    float z_distance_threshold;

    float max_x_distance;
    float max_y_distance;
    float max_z_distance;

    #endregion

    #region 紀錄遊戲數據
    RecordData record_data;
    string guid;

    // stage = "story-game-number"
    string stage;
    int story_satge;
    int game_satge;
    int number_satge;

    #region 紀錄骨架位置
    Dictionary<int, HumanBodyBones> index_to_bone;
    Transform bone;

    // skeleton position or rotation 
    Vector3 vector3;
    Dictionary<HumanBodyBones, Vector3> skeletons;
    Dictionary<HumanBodyBones, Vector3> rotations;
    bool is_recording;
    int bones_number;
    #endregion
    #endregion

    // 偵測結果呈現
    string pose_type;
    string message;

    #region Life cycle
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        print("DetectManager created!!!!!!!!!!!!!!!!!!!!!");

        #region 初始化 變數   
        #region 初始化 比對標準(動作名稱，模型，比對關節，門檻值)
        movement_map = new Dictionary<DetectSkeleton, Movement>();
        DetectSkeleton movement_type;
        foreach (Movement _movement in movements)
        {
            movement_type = _movement.getMovementType();
            movement_map.Add(movement_type, _movement);
            print(string.Format("Add {0} into movement_map", movement_type));
        }

        initMovement();

        // init_pos, max_x_distance, max_y_distance, max_z_distance
        // 各個動作，是否通過 與 正確率
        resetState();
        #endregion 初始化 比對標準 end

        GameConfig config = GameConfig.loadGameConfig();
        x_distance_threshold = config.x_distance_threshold / 100.0f;
        y_distance_threshold = config.y_distance_threshold / 100.0f;
        z_distance_threshold = config.z_distance_threshold / 100.0f;
        #endregion

        #region 紀錄遊戲數據
        // stage = "story-game-stage_number"
        story_satge = 0;
        game_satge = 0;
        number_satge = 0;

        #region 紀錄骨架位置
        // 關節 index 對應關節名稱的字典
        index_to_bone = PoseModelHelper.boneIndex2MecanimMap;

        // 是否紀錄數據
        is_recording = false;

        // 取得關節數量
        bones_number = model_helper.GetBoneTransformCount();
        #endregion

        // 偵測結果呈現
        pose_type = "";
        message = "";
        #endregion 初始化 變數 end

        print("====================");
    }

    // Update is called once per frame
    void Update()
    {
        KinectManager kinect_manager = KinectManager.Instance;

        if (kinect_manager != null && kinect_manager.IsInitialized())
        {
            switch (detect_skeleton)
            {
                // 配對第一關動作
                case DetectSkeleton.Game1:
                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 上下移動幅度超過一定距離 
                    max_y_distance = Mathf.Max(yDistance(), max_y_distance);
                    additional_accuracy = max_y_distance / y_distance_threshold;

                    // 左單腳跳
                    compareMovement(DetectSkeleton.LeftSingleJump, additional_accuracy);

                    // 右單腳跳
                    compareMovement(DetectSkeleton.RightSingleJump, additional_accuracy);

                    // 伸展
                    compareMovement(DetectSkeleton.RaiseTwoHand);

                    //print(string.Format("singlejump 額外條件:{0}, y_distance_threshold:{1:F4}, max_y_distance:{2:F4}",
                    //    additional_accuracy, y_distance_threshold, max_y_distance));

                    break;

                // 配對第二關動作
                case DetectSkeleton.Game2:
                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 上下移動幅度超過一定距離                    
                    max_x_distance = Mathf.Max(xDistance(), max_x_distance);
                    additional_accuracy = max_x_distance / x_distance_threshold;

                    // 滑步側移
                    compareMovement(DetectSkeleton.SlidingSideShift, additional_accuracy);

                    //print(string.Format("SlidingSideShift add:{0}, x_distance_threshold:{1:F4}, max_x_distance:{2:F4}",
                    //    additional_accuracy, x_distance_threshold, max_x_distance));

                    // 左丟球
                    compareMovement(DetectSkeleton.LeftThrowBall);

                    // 右丟球
                    compareMovement(DetectSkeleton.RightThrowBall);
                    break;

                // 配對第三關動作
                case DetectSkeleton.Game3:

                    // 蹲
                    compareMovement(DetectSkeleton.Sit);

                    // 左踢
                    compareMovement(DetectSkeleton.LeftKick);

                    // 右踢
                    compareMovement(DetectSkeleton.RightKick);
                    break;

                // 配對第四關動作
                case DetectSkeleton.Game4:

                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 上下移動幅度超過一定距離                    
                    max_z_distance = Mathf.Max(zDistance(), max_z_distance);
                    additional_accuracy = max_z_distance / z_distance_threshold;

                    // 跑
                    compareMovement(DetectSkeleton.Run);

                    // 左跨跳
                    compareMovement(DetectSkeleton.LeftCrossJump, additional_accuracy);

                    // 右跨跳
                    compareMovement(DetectSkeleton.RightCrossJump, additional_accuracy);

                    break;

                // 配對第五關動作
                case DetectSkeleton.Game5:

                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 上下移動幅度超過一定距離                    
                    max_y_distance = Mathf.Max(yDistance(), max_y_distance);
                    additional_accuracy = max_y_distance / y_distance_threshold;

                    // 左打擊
                    compareMovement(DetectSkeleton.LeftHit);

                    // 右打擊
                    compareMovement(DetectSkeleton.RightHit);

                    // 雙腳跳
                    compareMovement(DetectSkeleton.DoubleJump, additional_accuracy);

                    break;

                // 配對第六關動作
                case DetectSkeleton.Game6:

                    // 左拍球
                    compareMovement(DetectSkeleton.LeftDribble);
                    // 右拍球
                    compareMovement(DetectSkeleton.RightDribble);

                    // 左單腳站
                    compareMovement(DetectSkeleton.LeftSingleStand);
                    // 右單腳站
                    compareMovement(DetectSkeleton.RightSingleStand);

                    break;

                // 配對跨跳
                case DetectSkeleton.CrossJump:
                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 前後移動幅度超過一定距離
                    max_z_distance = Mathf.Max(zDistance(), max_z_distance);
                    additional_accuracy = max_z_distance / z_distance_threshold;

                    // 左跨跳
                    compareMovement(DetectSkeleton.LeftCrossJump, additional_accuracy);

                    // 右跨跳
                    compareMovement(DetectSkeleton.RightCrossJump, additional_accuracy);

                    //print(string.Format("xDistance:{0:F4}, x_distance_threshold:{1:F4}", xDistance(), x_distance_threshold));
                    break;

                // 單腳跳配對 
                case DetectSkeleton.SingleJump:
                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                        print(string.Format("init_pos:{0}", init_pos));
                    }

                    // 額外條件 上下移動幅度超過一定距離                    
                    max_y_distance = Mathf.Max(yDistance(), max_y_distance);
                    additional_accuracy = max_y_distance / y_distance_threshold;

                    // 左單腳跳
                    compareMovement(DetectSkeleton.LeftSingleJump, additional_accuracy);

                    // 右單腳跳
                    compareMovement(DetectSkeleton.RightSingleJump, additional_accuracy);

                    //print(string.Format("singlejump 額外條件:{0}, y_distance_threshold:{1:F4}, max_y_distance:{2:F4}",
                    //    additional_accuracy, y_distance_threshold, max_y_distance));
                    break;

                
                // 蹲配對 
                case DetectSkeleton.Sit:
                    compareMovement(DetectSkeleton.Sit);
                    break;
               
                // 伸展配對 
                case DetectSkeleton.RaiseTwoHand:
                    compareMovement(DetectSkeleton.RaiseTwoHand);
                    break;
                    
                // 跑配對 
                case DetectSkeleton.Run:
                    compareMovement(DetectSkeleton.Run);
                    break;

                // 雙腳跳配對 
                case DetectSkeleton.DoubleJump:
                    if (init_pos == null)
                    {
                        // 初始化玩家位置，開始偵測當下的位置
                        init_pos = model_helper.transform.position;
                    }

                    // 額外條件 上下移動幅度超過一定距離                    
                    max_y_distance = Mathf.Max(yDistance(), max_y_distance);
                    additional_accuracy = max_y_distance / y_distance_threshold;
                    //print(string.Format("max_y_distance:{0:F4}", max_y_distance));

                    compareMovement(DetectSkeleton.DoubleJump, additional_accuracy);
                    break;
                
                default:
                    // pose_type = DetectSkeleton.None
                    message = "";
                    break;
            }
        }
        else
        {
            print("kinect_manager is null");
        }
    }

    private void FixedUpdate()
    {
        if (GameInfo.isRunning)
        {
            // 每次紀錄1幀，所有關節位置，直到 is_recording = false，最終會記錄許多幀
            if (is_recording)
            {
                skeletons = new Dictionary<HumanBodyBones, Vector3>();
                rotations = new Dictionary<HumanBodyBones, Vector3>();

                for (int i = 0; i < bones_number; i++)
                {
                    if ((bone = model_helper.GetBoneTransform(i)) == null)
                    {
                        continue;
                    }

                    vector3 = bone.transform.position;
                    skeletons.Add(index_to_bone[i], vector3);

                    vector3 = bone.transform.rotation.eulerAngles;
                    rotations.Add(index_to_bone[i], vector3);

                }
                record_data.addSkeletons(skeletons);
                record_data.addRotations(rotations);
            }
        }
    }    

    private void OnGUI()
    {
        //GUI.color = Color.red;
        //GUI.skin.label.fontSize = 50;
        //pose_type = string.Format("pose_type:{0}", detect_skeleton);
        //GUILayout.Label(pose_type + "\n" + message);
    }
    #endregion Life cycle end

    #region 動作偵測程式碼
    // 取得單一姿勢正確率
    float getAccuracy(PoseModelHelper poseModelHelper, List<HumanBodyBones> comparingParts)
    {
        int i, index, len = comparingParts.Count;
        Transform p1, p2, s1, s2;
        Vector3 playerBone, standardBone;

        float diff = 0f, total_diff = 0f;

        for (i = 0; i < len; i++)
        {
            index = PoseModelHelper.bone2IndexMap[comparingParts[i]];
            if ((p1 = model_helper.GetBoneTransform(index)) == null)
            {
                continue;
            }
            if ((s1 = poseModelHelper.GetBoneTransform(index)) == null)
            {
                continue;
            }

            if ((i + 1) >= len)
            {
                index = PoseModelHelper.bone2IndexMap[comparingParts[0]];
            }
            else
            {
                index = PoseModelHelper.bone2IndexMap[comparingParts[i + 1]];
            }

            if ((p2 = model_helper.GetBoneTransform(index)) == null)
            {
                continue;
            }
            if ((s2 = poseModelHelper.GetBoneTransform(index)) == null)
            {
                continue;
            }

            //取得玩家與標準模型 目前節點(jointType)的向量
            playerBone = (p2.position - p1.position).normalized;
            standardBone = (s2.position - s1.position).normalized;

            //計算玩家骨架 與 姿勢骨架角度差距
            diff = Vector3.Angle(playerBone, standardBone);
            if (diff > 90f)
            {
                diff = 90f;
            }
            total_diff += diff / 90f;
        }

        total_diff /= len;

        return 1f - total_diff;
    }

    // 比對動作
    void compareMovement(DetectSkeleton check_pose)
    {
        try
        {
            Movement _movement = movement_map[check_pose];
            PoseModelHelper[] poseModels = _movement.getModels();
            float[] thresholds = _movement.getThresholds();
            int i, len = _movement.getMovementNumber();
            float acc;
            for (i = 0; i < len; i++)
            {
                acc = getAccuracy(poseModels[i], _movement.getComparingParts());
                message = string.Format("type:{0}[{1}], acc: {2:F4}, threshold: {3:F4} ",
                    check_pose, i, _movement.getAccuracy(i), thresholds[i]);
                print(message);

                // 記錄各個分解動作的最高值
                _movement.setAccuracy(i, Mathf.Max(acc, _movement.getAccuracy(i)));

                // 正確率超過門檻值，則 matchMap 記錄通過
                if (acc > thresholds[i])
                {
                    _movement.setMatched(i, true);
                }
            }

            // 當所有動作皆通過                       
            if (compare(check_pose))
            {
                // 任一動作完成偵測，停止偵測
                detect_skeleton = DetectSkeleton.None;
            }
        }
        catch (KeyNotFoundException)
        {
            print(string.Format("No {0} in movement_map", check_pose));
        }
    }

    // 比對動作 + 額外條件
    void compareMovement(DetectSkeleton check_pose, float additional_accuracy)
    {
        try
        {
            Movement _movement = movement_map[check_pose];
            PoseModelHelper[] poseModels = _movement.getModels();
            float[] thresholds = _movement.getThresholds();
            int i, len = _movement.getMovementNumber();
            float acc;
            for (i = 0; i < len; i++)
            {
                acc = getAccuracy(poseModels[i], _movement.getComparingParts());
                message = string.Format("type:{0}[{1}], acc: {2:F4}, threshold: {3:F4} ",
                    check_pose, i, _movement.getAccuracy(i), thresholds[i]);
                print(message);

                // 記錄各個分解動作的最高值
                _movement.setAccuracy(i, Mathf.Max(acc, _movement.getAccuracy(i)));

                // 若正確率超過門檻值
                if (acc > thresholds[i])
                {
                    // 令該動作狀態為通過
                    _movement.setMatched(i, true);
                }
            }

            // 附加額外通關條件
            if (_movement.hasAddtionalCondition())
            {
                // 取得正確率最高值
                acc = Mathf.Max(additional_accuracy, _movement.getAccuracy(i));

                // 確保正確率最大值為 1(額外條件為距離，因此正確率可能超過 1)
                acc = Mathf.Min(1f, acc);

                // 設定動作正確率
                _movement.setAccuracy(i, acc);
                //print(string.Format("additional_accuracy: {0:F4}", acc));

                // 若正確率超過門檻值
                if (acc >= thresholds[i])
                {
                    // 令該動作狀態為通過
                    _movement.setMatched(i, true);

                    //print(string.Format("additional: i={0}, threshold={1:F4}, acc={2:F4}",
                    //    i, thresholds[i], acc));
                }
            }

            // 當所有動作皆通過                       
            if (compare(check_pose))
            {
                // 任一動作完成偵測，停止偵測
                detect_skeleton = DetectSkeleton.None;
            }
        }
        catch (KeyNotFoundException)
        {
            print(string.Format("No {0} in movement_map", check_pose));
        }
    }

    #region 結果判斷
    // 回傳多動作判斷結果
    public DetectSkeleton? whichOnePass(DetectSkeleton detects)
    {
        switch (detects)
        {
            case DetectSkeleton.Game1:
                return whichPass(DetectSkeleton.LeftSingleJump, DetectSkeleton.RightSingleJump, DetectSkeleton.RaiseTwoHand);
            case DetectSkeleton.Game2:
                return whichPass(DetectSkeleton.SlidingSideShift, DetectSkeleton.LeftThrowBall, DetectSkeleton.RightThrowBall);
            case DetectSkeleton.Game3:
                return whichPass(DetectSkeleton.Sit, DetectSkeleton.LeftKick, DetectSkeleton.RightKick);
            case DetectSkeleton.Game4:
                return whichPass(DetectSkeleton.Run, DetectSkeleton.LeftCrossJump, DetectSkeleton.RightCrossJump);
            case DetectSkeleton.Game5:
                return whichPass(DetectSkeleton.LeftHit, DetectSkeleton.RightHit, DetectSkeleton.DoubleJump);
            case DetectSkeleton.Game6:
                return whichPass(DetectSkeleton.LeftDribble, DetectSkeleton.RightDribble, DetectSkeleton.LeftSingleStand, DetectSkeleton.RightSingleStand);

            case DetectSkeleton.CrossJump:
                return whichPass(DetectSkeleton.LeftCrossJump, DetectSkeleton.RightCrossJump);
            case DetectSkeleton.SingleJump:
                return whichPass(DetectSkeleton.LeftSingleJump, DetectSkeleton.RightSingleJump);

            default:
                return whichPass(detects);
        }
    }

    DetectSkeleton? whichPass(params DetectSkeleton[] detects)
    {
        int i, numbers = detects.Length;
        DetectSkeleton skeleton;

        for (i = 0; i < numbers; i++)
        {
            skeleton = detects[i];
            //print(string.Format("Is {0} pass?", skeleton));
            if (compare(skeleton))
            {
                return skeleton;
            }
        }
        return null;
    }

    // 檢查是否所有動作皆通過
    bool compare(DetectSkeleton skeleton)
    {
        bool _pass = true;
        Movement _movement = movement_map[skeleton];
        int len = _movement.getThresholdNumber(), i;
        for (i = 0; i < len; i++)
        {
            _pass &= _movement.isMatched(i);
        }
        return _pass;
    }

    // 處理多動作配對失敗，返回 "誤差最小" 的那個動作，推測是做該動作但失敗
    public DetectSkeleton thisOneFailed(DetectSkeleton detects)
    {
        switch (detects)
        {
            case DetectSkeleton.Game1:
                return compareAccuracy(DetectSkeleton.LeftSingleJump, DetectSkeleton.RightSingleJump, DetectSkeleton.RaiseTwoHand);
            case DetectSkeleton.Game2:
                return compareAccuracy(DetectSkeleton.SlidingSideShift, DetectSkeleton.LeftThrowBall, DetectSkeleton.RightThrowBall);
            case DetectSkeleton.Game3:
                return compareAccuracy(DetectSkeleton.Sit, DetectSkeleton.LeftKick, DetectSkeleton.RightKick);
            case DetectSkeleton.Game4:
                return compareAccuracy(DetectSkeleton.Run, DetectSkeleton.LeftCrossJump, DetectSkeleton.RightCrossJump);            
            case DetectSkeleton.Game5:
                return compareAccuracy(DetectSkeleton.LeftHit, DetectSkeleton.RightHit, DetectSkeleton.DoubleJump);
            case DetectSkeleton.Game6:
                return compareAccuracy(DetectSkeleton.LeftDribble, DetectSkeleton.RightDribble, DetectSkeleton.LeftSingleStand, DetectSkeleton.RightSingleStand);

            case DetectSkeleton.CrossJump:
                return compareAccuracy(DetectSkeleton.LeftCrossJump, DetectSkeleton.RightCrossJump);
            case DetectSkeleton.SingleJump:
                return compareAccuracy(DetectSkeleton.LeftSingleJump, DetectSkeleton.RightSingleJump);
           
            default:
                return compareAccuracy(detects);
        }
    }

    /// <summary>
    /// 回傳 "誤差最小" 的那個動作
    /// </summary>
    /// <param name="detects">要判斷的動作列表</param>
    /// <returns></returns>
    public DetectSkeleton compareAccuracy(params DetectSkeleton[] detects)
    {
        int length = detects.Length, index, i, len, most_likely_index = 0;
        float _acc, _threshold, loss = 0f, min_loss = 100f;
        DetectSkeleton skeleton;
        Movement _movement;
        
        for (index = 0; index < length; index++)
        {
            // 取得動作類別
            skeleton = detects[index];

            // 取得動昨物件
            _movement = movement_map[skeleton];

            // 取得門檻數量
            len = _movement.getThresholdNumber();

            for (i = 0; i < len; i++)
            {
                // 取得動作正確率
                _acc = _movement.getAccuracy(i);

                // 取得動作門檻值
                _threshold = _movement.getThreshold(i);

                // 累加動作誤差
                loss += (_threshold - _acc);
            }

            // 計算動作誤差的平均
            loss /= len;

            // 比較 動作誤差的平均 大小，傳回 "誤差最小" 的那個動作
            if (loss < min_loss)
            {
                min_loss = loss;
                most_likely_index = index;
            }
        }

        return detects[most_likely_index];
    }
    #endregion

    #endregion

    #region 歐拉距離
    /// <summary>
    /// 玩家當前位置 X 座標 - 初始位置的 X 座標
    /// </summary>
    /// <returns>玩家 左右 移動距離</returns>
    float xDistance()
    {
        // 相對於初始位置的移動(向量)
        Vector3 vector3 = model_helper.transform.position - (Vector3)init_pos;
        float distance = Math.Abs(vector3.x);
        return distance;
    }

    /// <summary>
    /// 玩家當前位置 Y 座標 - 初始位置的 Y 座標
    /// </summary>
    /// <returns>玩家 上下 移動距離</returns>
    float yDistance()
    {
        // 相對於初始位置的移動(向量)
        Vector3 vector3 = model_helper.transform.position - (Vector3)init_pos;
        float distance = Math.Abs(vector3.y);
        return distance;
    }

    /// <summary>
    /// 玩家當前位置 Z 座標 - 初始位置的 Z 座標
    /// </summary>
    /// <returns>玩家 前後 移動距離</returns>
    float zDistance()
    {
        // 相對於初始位置的移動(向量)
        Vector3 vector3 = model_helper.transform.position - (Vector3)init_pos;
        float distance = Math.Abs(vector3.z);
        return distance;
    }
    #endregion

    void initMovement()
    {
        // 取得遊戲數據
        Dictionary<string, float[]> threshold_config = GameConfig.loadThreshold();

        Array array = Enum.GetValues(typeof(DetectSkeleton));
        DetectSkeleton skeleton;
        Movement _movement;
        float[] _threshold;

        for (int i = 0; i < array.Length; i++)
        {
            // 取得動作名稱
            skeleton = (DetectSkeleton)array.GetValue(i);

            try
            {
                // 透過名稱，取得 動作物件
                _movement = movement_map[skeleton];
            }
            catch (KeyNotFoundException)
            {
                continue;
            }

            try
            {
                string key = string.Format("{0}", skeleton);
                // 透過名稱，取得 門檻值
                _threshold = threshold_config[key];

                // 設置門檻值
                _movement.setThresholds(_threshold);

                // 有門檻值後，才能確定 正確率 個數
                _movement.resetState();
            }
            catch (KeyNotFoundException)
            {
                print(string.Format("Loss threshold of {0}", skeleton));
                continue;
            }
        }
    }

    /// <summary>
    /// 還原回尚未配對狀態
    /// </summary>
    public void resetState()
    {
        // 還原初始位置
        init_pos = null;
        max_x_distance = 0f;
        max_y_distance = 0f;
        max_z_distance = 0f;

        // 初始化各個動作，是否通過 與 正確率
        foreach (Movement _movement in movement_map.Values)
        {
            _movement.resetState();
        }
    }

    #region 供各關 GameManager 呼叫 story, game, stage_number
    public void setStoryStage(int _story)
    {
        print(string.Format("setStoryStage = {0}", _story));
        story_satge = _story;
    }

    public int getStoryStage()
    {
        return story_satge;
    }

    public void setGameStage(int _game)
    {
        print(string.Format("setGameStage = {0}", _game));
        game_satge = _game;

        // 每關都從 1 開始紀錄
        setNumberStage(1);
    }

    public int getGameStage()
    {
        return game_satge;
    }

    public void setNumberStage(int _number)
    {
        print(string.Format("setNumberStage = {0}", _number));
        number_satge = _number;
    }

    public int getNumberStage()
    {
        return number_satge;
    }

    public string getStage()
    {
        return stage;
    }

    public string getGuid()
    {
        return guid;
    }

    public void startMatch(DetectSkeleton _detect_skeleton)
    {
        print(string.Format("startMatch:{0}", _detect_skeleton));

        // 開始偵測指定動作
        detect_skeleton = _detect_skeleton;

        record_data = new RecordData();
        guid = record_data.guid;

        // 開始紀錄數據
        stage = string.Format("{0}-{1}-{2}", story_satge, game_satge, number_satge);
        record_data.setStage(stage);

        record_data.setStartTime();

        // 開始記錄骨架資訊
        is_recording = true;

        // 開始動作提示
        hint.selectHint(detect_skeleton);
    }

    public void endMatch(DetectSkeleton _detect_skeleton ,string file_id)
    {
        print("endMatch");

        // 設置結束時間
        record_data.setEndTime();

        // 記錄三維歐拉距離
        string remark = string.Format("{0:F4}-{1:F4}-{2:F4}", 
            max_x_distance, max_y_distance, max_z_distance);
        record_data.setRemark(remark);

        // 動作流水編號，自動增加
        number_satge++;

        // 停止紀錄骨架
        is_recording = false;

        // 停止動作提示
        hint.stopVideo();

        //動作提示、紀錄數據
        DetectSkeleton? result = whichOnePass(_detect_skeleton);
        DetectSkeleton detect;

        if (result == null)
        {
            // 如果都失敗
            detect = thisOneFailed(detect_skeleton);
        }
        else
        {
            // 如果其中一個成功
            detect = (DetectSkeleton)result;
        }

        Movement _movement = movement_map[detect];
        record_data.setType(detect);
        record_data.setThreshold(_movement.getThresholds());
        record_data.setAccuracy(_movement.getAccuracy());

        // 取消偵測
        detect_skeleton = DetectSkeleton.None;

        // 完成數據紀錄後，還原回尚未配對狀態
        resetState();

        // 紀錄相關數據並寫出
        RecordData.save(file_id, record_data);
        record_data = new RecordData();
    }
    #endregion
}
