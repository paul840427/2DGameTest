using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class GameConfig
{
    public float x_distance_threshold = 15f;
    public float y_distance_threshold = 5f;
    public float z_distance_threshold = 50f;

    #region  第一關設定
    public int round1 = 3;
    public int number1 = 2;
    public float round_time1 = 15f;
    public float question_buffer1 = 3f;
    public float detect_interval_time1 = 0.8f;
    public float yesno_display_time1 = 1f;
    public float feedback_display_time1 = 1f;
    public float round_interval_time1 = 2f;
    public float score_display_time1 = 5f;
    public float bg_vol1 = 60.0f;
    public float correct_vol1 = 40.0f;
    public float wrong_vol1 = 100.0f;
    public float success_vol1 = 70.0f;
    public float fail_vol1 = 70.0f;
    #endregion

    #region  第三關設定
    public int round3 = 6;
    public int number3 = 1;
    public float round_time3 = 15f;
    public float question_buffer3 = 3f;
    public float detect_interval_time3 = 0.8f;
    public float yesno_display_time3 = 1f;
    public float feedback_display_time3 = 1f;
    public float round_interval_time3 = 2f;
    public float score_display_time3 = 5f;
    public float bg_vol3 = 40.0f;
    public float correct_vol3 = 40.0f;
    public float wrong_vol3 = 100.0f;
    public float success_vol3 = 70.0f;
    public float fail_vol3 = 70.0f;
    #endregion

    #region  第五關設定
    public int round5 = 6;
    public int number5 = 1;
    public float round_time5 = 15f;
    public float question_buffer5 = 3f;
    public float detect_interval_time5 = 0.8f;
    public float yesno_display_time5 = 1f;
    public float feedback_display_time5 = 1f;
    public float round_interval_time5 = 2f;
    public float score_display_time5 = 5f;
    public float bg_vol5 = 40.0f;
    public float correct_vol5 = 40.0f;
    public float wrong_vol5 = 100.0f;
    public float success_vol5 = 70.0f;
    public float fail_vol5 = 70.0f;
    #endregion


    public void saveGameConfig()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "GameConfig.txt");
        // 寫入檔案
        StreamWriter writer = new FileInfo(path).CreateText();
        string save_config = JsonConvert.SerializeObject(this);
        writer.Write(save_config);
        writer.Close();
        writer.Dispose();
    }

    public static GameConfig loadGameConfig()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "GameConfig.txt");
        //讀取json檔案並轉存成文字格式
        StreamReader file = new StreamReader(path);
        string load_config = file.ReadToEnd();
        load_config = load_config.Trim();
        file.Close();

        GameConfig config = new GameConfig();
        config = JsonConvert.DeserializeObject<GameConfig>(load_config);
        return config;
    }

    public static Dictionary<string, float[]> loadThreshold()
    {
        Dictionary<string, float[]> threshold_config = new Dictionary<string, float[]>();

        string path = Path.Combine(Application.streamingAssetsPath, "threshold_config.csv");
        string line;
        string[] row;
        string _type;
        int i, len;

        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                row = line.Split(',');
                _type = row[0];
                len = row.Length;
                float[] thresholds = new float[len - 1];
                for (i = 1; i < len; i++)
                {
                    thresholds[i - 1] = float.Parse(row[i]) / 100f;
                }

                threshold_config.Add(_type, thresholds);
            }
        }

        return threshold_config;
    }
}


