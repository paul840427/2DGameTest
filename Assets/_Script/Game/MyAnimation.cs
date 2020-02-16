using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MyAnimation : MonoBehaviour
{
    public GameObject this_object;
    public GameObject menu;

    public GameObject animation_menu;
    public Button menu_button;
    // game 1 & 2
    public Button animation1;
    // game 3 & 4
    public Button animation2;
    // game 5 & 6
    public Button animation3;

    public GameObject video;
    public VideoPlayer player;
    public GameObject player_go;

    bool is_playing;

    // 當前場景的 index
    int sence_index;

    // Start is called before the first frame update
    void Start()
    {
        menu_button.onClick.AddListener(()=> {
            menu.SetActive(true);
            this_object.SetActive(false);
        });

        #region animation
        animation1.onClick.AddListener(()=> {
            onAnimationClicked(1);
        });

        animation2.onClick.AddListener(() => {
            onAnimationClicked(2);
        });

        animation3.onClick.AddListener(() => {
            onAnimationClicked(3);
        });
        #endregion

        is_playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_playing)
        {
            print(string.Format("{0}/{1}", player.frame + 1, player.frameCount));
            if (player.frame + 1 == (long)player.frameCount)
            {
                is_playing = false;
                animation_menu.SetActive(true);
                video.SetActive(false);

                // 一到六關，動畫播放完畢，回復玩家模型的呈現
                activePlayer(true);
            }
        }
    }

    void onAnimationClicked(int _num)
    {
        // 一到六關，播放動畫前，隱藏玩家模型
        activePlayer(false);

        video.SetActive(true);

        string path = string.Format("_Video/Animation{0}", _num);
        player.clip = Resources.Load<VideoClip>(path);
        print("player.isPlaying:" + player.isPlaying);

        animation_menu.SetActive(false);
        is_playing = true;
    }

    void onAnimationFinished()
    {
        animation_menu.SetActive(true);
        video.SetActive(false);
    }

    void activePlayer(bool _active)
    {
        // 取得目前場景的 index
        sence_index = SceneManager.GetActiveScene().buildIndex;

        if (sence_index != 0)
        {
            player_go.SetActive(_active);
        }
    }
}
