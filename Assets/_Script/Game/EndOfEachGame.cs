using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfEachGame : MonoBehaviour
{
    public Button restart_button;
    public Button menu_button;
    public Button next_stage_button;
    public GameObject menu;

    // 動作偵測腳本
    //DetectManager detect_manager;

    // kinect
    //GameObject kinect;

    // 音效管理
    //AudioManager audio_manager;

    // 當前場景的 index
    int sence_index;

    // 預載 Scene
    AsyncOperation scene_restart;

    // Start is called before the first frame update
    void Start()
    {
        // 取得目前場景的 index
        sence_index = SceneManager.GetActiveScene().buildIndex;
        print("sence_index:" + sence_index);

        #region 再玩一次
        // 預載 Scene
        scene_restart = SceneManager.LoadSceneAsync(sence_index);
        // 暫不開啟 預載 Scene
        scene_restart.allowSceneActivation = false;
        #endregion

        // 點擊後，重新載入此場景
        restart_button.onClick.AddListener(()=> {
            // 確保遊戲物件被刪除
            //destoryManager();

            scene_restart.allowSceneActivation = true;
            print("scene_restart.allowSceneActivation: " + scene_restart.allowSceneActivation);
        });

        // 開啟選單，選擇關卡
        menu_button.onClick.AddListener(()=> {
            menu.SetActive(true);
            // 確保遊戲物件被刪除
            //destoryManager();
            print("Open menu");

            gameObject.SetActive(false);
        });

        // 點擊後，載入下一關場景
        next_stage_button.onClick.AddListener(()=> {
            // 確保遊戲物件被刪除
            //destoryManager();

            if (sence_index < 6)
            {
                SceneManager.LoadScene(sence_index + 1);
            }
            else
            {
                print("Go to game end scene.");
            }            
        });

        if(sence_index == 6)
        {
            next_stage_button.gameObject.SetActive(false);
        }
    }

    //public void setDetectManager(DetectManager detect_manager)
    //{
    //    this.detect_manager = detect_manager;
    //}

    //public void setKinect(GameObject kinect)
    //{
    //    this.kinect = kinect;
    //}

    //public void setAudioManager(AudioManager audio_manager)
    //{
    //    this.audio_manager = audio_manager;
    //}

    //public void destoryManager()
    //{
    //    if (detect_manager != null)
    //    {
    //        Destroy(detect_manager);
    //    }
    //    else
    //    {
    //        print("[end] detect_manager is null");
    //    }

    //    if (kinect != null)
    //    {
    //        Destroy(kinect);
    //    }
    //    else
    //    {
    //        print("[end] kinect is null");
    //    }

    //    if (audio_manager != null)
    //    {
    //        Destroy(audio_manager);
    //    }
    //    else
    //    {
    //        print("[end] audio_manager is null");
    //    }
    //}
}
