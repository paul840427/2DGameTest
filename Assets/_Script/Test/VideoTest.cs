using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoTest : MonoBehaviour
{
    public HintManager hint;

    // Start is called before the first frame update
    void Start()
    {
        print(PlayerPrefs.GetString("id"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            hint.stopVideo();
            hint.selectHint(DetectSkeleton.Game6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            hint.stopVideo();
        }
    }
}
