using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTest : MonoBehaviour {
    public bool save;
	// Use this for initialization
	void Start () {
        if (save)
        {
            GameConfig config = new GameConfig();
            config.saveGameConfig();
            print("config saved.");
        }
        else
        {
            GameConfig config = GameConfig.loadGameConfig();
            print(string.Format("round_interval_time1: {0:F4}", config.round_interval_time1));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
