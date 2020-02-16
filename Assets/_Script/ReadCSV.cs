using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/000.json");
        QuestData loadedQuestData = JsonUtility.FromJson<QuestData>(json);
        Debug.Log("QuestionNumber: " + loadedQuestData.QuestionNumber);
        Debug.Log("Quest: " + loadedQuestData.Quest);
    }
    private class QuestData
    {
        public string QuestionNumber;
        public string Quest;
    }
}
