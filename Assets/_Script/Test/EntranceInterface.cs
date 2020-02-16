using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EntranceInterface : MonoBehaviour
{
    public InputField input_field;
    public Dropdown drop_down;
    public Button sign_up_button;
    public Button sign_in_button;

    private string path;

    public GameObject bg_image;
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, "user_name.csv");
        // 讀入玩家名單
        initOptions();

        sign_up_button.onClick.AddListener(signUp);
        sign_in_button.onClick.AddListener(signIn);

        // 初始化記錄動作次數與各關卡得分
        initMovmentCountAndGameCount();
    }

    void initOptions()
    {
        if (File.Exists(path))
        {
            // 若檔案存在，則將玩家名單讀入
            string line;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    addOption(line);
                }
            }
        }
    }

    void signUp()
    {
        string new_option = input_field.text;
        input_field.text = "";

        // 若玩家名稱已存在
        if (isUserExisted(new_option))
        {            
            Debug.Log(string.Format("User {0} has existed.", new_option));
        }
        // 若玩家名稱不存在
        else
        {
            // 加入下拉選單中
            addOption(new_option);

            // 使用者名稱寫入紀錄
            addNewUser(new_option);
        }
    }

    void addOption(string option)
    {
        drop_down.options.Add(new Dropdown.OptionData(option));
    }

    void addNewUser(string user_name)
    {
        // 檢查檔案是否存在，不存在則建立
        StreamWriter writer;
        if (!File.Exists(path))
        {
            // 檔案不存在，建立檔案
            writer = new FileInfo(path).CreateText();
        }
        else
        {
            // 檔案存在，新增名單
            writer = new FileInfo(path).AppendText();
        }

        // 寫入新使用者名稱
        writer.WriteLine(user_name);
        writer.Close();
        writer.Dispose();
    }

    void signIn()
    {
        GameInfo.id = drop_down.captionText.text;
        print(string.Format("Hello {0}", GameInfo.id));

        menu.SetActive(true);
        bg_image.SetActive(false);
        gameObject.SetActive(false);
    }

    bool isUserExisted(string _user_name)
    {
        // 檔案存在，檢查使用者是否已經存在
        foreach (Dropdown.OptionData _option in drop_down.options)
        {
            if (_option.text.Equals(_user_name))
            {               
                return true;
            }
        }

        return false;
    }

    // 初始化記錄動作次數與各關卡得分
    void initMovmentCountAndGameCount()
    {
        // key:movement type, value:movement times
        GameInfo.movement_count = new Dictionary<DetectSkeleton, int>();

        Array array = Enum.GetValues(typeof(DetectSkeleton));
        DetectSkeleton skeleton;

        for (int i = 0; i < array.Length; i++)
        {
            // 取得動作名稱
            skeleton = (DetectSkeleton)array.GetValue(i);

            GameInfo.movement_count.Add(skeleton, 0);
        }

        // key:game, value: game score
        GameInfo.game_count = new Dictionary<int, int>();

        for (int i = 1; i <= 6; i++)
        {
            GameInfo.game_count.Add(i, 0);
        }

        print("初始化記錄動作次數與各關卡得分");
    }
}
