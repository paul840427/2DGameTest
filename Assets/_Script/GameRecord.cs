using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRecord
{
    string data;

    #region Distinguish label
    string distinguish_label;
 
    string[] stage_array;
    string stage;
    #endregion

    #region Question
    string question;

    string first;
    string second;
    string answer;
    #endregion

    #region Player feedback
    string player_feedback;

    string movement;
    string selection;
    string result;
    #endregion

    #region Record summary
    string record_summary;

    int correct;
    int total;
    #endregion

    #region Fuction
    public void start(string _guid, string _stage,
        GameItem[] _first, string _operator_, GameItem[] _second, GameItem[] _answer)
    {
        stage_array = _stage.Split('-');
        stage = string.Join(",", stage_array);

        // last one is start time
        distinguish_label = string.Format("{0},{1},{2},{3}", 
            _guid, GameInfo.id, stage, DateTime.Now.ToString("HH-mm-ss-ffff"));

        first = multiItems(_first);
        second = multiItems(_second);
        answer = multiItems(_answer);

        question = string.Format("{0},{1},{2},{3}", first, _operator_, second, answer);
    }

    public void setMovementSelection(DetectSkeleton _movement, GameItem[] _selection)
    {
        movement = string.Format("{0}", _movement);
        selection = multiItems(_selection);
    }

    public void end(GameItem _result, int _correct, int _total)
    {
        // last one is end time
        distinguish_label = string.Format("{0},{1}", 
            distinguish_label, DateTime.Now.ToString("HH-mm-ss-ffff"));
        
        result = string.Format("{0}", _result);

        player_feedback = string.Format("{0},{1},{2}", movement, selection, result);
        record_summary = string.Format("{0},{1}", _correct, _total);

        // guid, id, story, stage, number, start_time, end_time, first, operator, second, answer, movement, selection, result(Correct/Error), correct, total
        data = string.Format("{0},{1},{2},{3}", 
            distinguish_label, question, player_feedback, record_summary);

        save(data);
    }

    string multiItems(GameItem[] _items)
    {
        StringBuilder sb = new StringBuilder();
        int i, len = _items.Length;

        for(i = 0; i < len; i++)
        {
            sb.Append(string.Format("{0}", _items[i]));

            if (i != len - 1)
            {
                sb.Append("&");
            }
        }

        return sb.ToString();
    }

    public void save(string _data)
    {
        // [我的文件] 資料夾
        string document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // [我的文件] 資料夾 \Somatosensory\GameRecord
        string path = Path.Combine(document, "Somatosensory\\GameRecord");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 檔名：(Date).csv
        // [我的文件] 資料夾 \Somatosensory\GameRecord\(Date).csv
        path = Path.Combine(path, string.Format("{0}.csv", DateTime.Now.ToString("yyyy-MM-dd")));

        // 檢查檔案是否存在，不存在則建立
        StreamWriter writer;
        if (!File.Exists(path))
        {
            writer = new FileInfo(path).CreateText();
            string column_name = "guid, id, story, stage, number, start_time, end_time, first, operator, second, answer, movement, selection, result(Correct/Error), correct, total";
            writer.WriteLine(column_name);
        }
        else
        {
            writer = new FileInfo(path).AppendText();
        }

        writer.WriteLine(_data);
        writer.Close();
        writer.Dispose();
    }
    #endregion
}
