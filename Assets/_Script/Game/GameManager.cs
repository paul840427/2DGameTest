using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Canvas

    #region GameLayout
    public GameObject game_layout;
    public GameLayout layout;
    #endregion

    public GameObject score_board_go;
    public GameObject end_of_game_go;
    public EndOfEachGame end_of_game;
    #endregion

    // 根據 flower_number 取得花朵模型，編號由 1 開始
    public string selection_messege = "";

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 60;
        GUI.Label(new Rect(100, 920, Screen.width, 200), string.Format("{0}", selection_messege));
    }
}
