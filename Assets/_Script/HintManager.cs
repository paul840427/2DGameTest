using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HintManager : MonoBehaviour
{
    [SerializeField] GameObject main_camera;
    [SerializeField] GameObject left_hint;
    [SerializeField] GameObject right_hint;
    Vector3 camera_pos;
    VideoPlayer left_video;
    VideoPlayer right_video;

    // Start is called before the first frame update
    void Start()
    {
        camera_pos = main_camera.transform.position;
        transform.position = new Vector3(camera_pos.x, camera_pos.y, camera_pos.z + 17f);
        left_video = left_hint.GetComponent<VideoPlayer>();
        right_video = right_hint.GetComponent<VideoPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // 若遊戲進行中"攝影機"會移動位置，才需要時時修正"動作提示"呈現位置
        //camera_pos = main_camera.transform.position;
        //transform.position = new Vector3(camera_pos.x, camera_pos.y, camera_pos.z + 40f);
        
    }

    public void selectHint(DetectSkeleton _type)
    {
        DetectSkeleton _left_type = _type, _right_type = _type;
        left_hint.SetActive(true);
        right_hint.SetActive(true);

        switch (_type)
        {
            case DetectSkeleton.Game1:
                //return whichPass(DetectSkeleton.LeftSingleJump, DetectSkeleton.RightSingleJump, DetectSkeleton.RaiseTwoHand);
                _left_type = DetectSkeleton.SingleJump;
                _right_type = DetectSkeleton.RaiseTwoHand;
                break;
            case DetectSkeleton.Game2:
                //return whichPass(DetectSkeleton.SlidingSideShift, DetectSkeleton.LeftThrowBall, DetectSkeleton.RightThrowBall);
                _left_type = DetectSkeleton.ThrowBall; 
                _right_type = DetectSkeleton.SlidingSideShift;
                break;
            case DetectSkeleton.Game3:
                //return whichPass(DetectSkeleton.Sit, DetectSkeleton.LeftKick, DetectSkeleton.RightKick);
                _left_type = DetectSkeleton.Sit;
                _right_type = DetectSkeleton.Kick;
                break;
            case DetectSkeleton.Game4:
                //return whichPass(DetectSkeleton.Run, DetectSkeleton.LeftCrossJump, DetectSkeleton.RightCrossJump);
                _left_type = DetectSkeleton.Run;
                _right_type = DetectSkeleton.CrossJump;
                break;
            case DetectSkeleton.Game5:
                //return whichPass(DetectSkeleton.LeftHit, DetectSkeleton.RightHit, DetectSkeleton.DoubleJump);
                _left_type = DetectSkeleton.Hit;
                _right_type = DetectSkeleton.DoubleJump;
                break;
            case DetectSkeleton.Game6:
                //return whichPass(DetectSkeleton.LeftDribble, DetectSkeleton.RightDribble, DetectSkeleton.LeftSingleStand, DetectSkeleton.RightSingleStand);
                _left_type = DetectSkeleton.Dribble;
                _right_type = DetectSkeleton.SingleStand;
                break;            
        }

        setVideo("left", _left_type);
        setVideo("right", _right_type);
    }

    void setVideo(string _place, DetectSkeleton _type)
    {
        string _video_path = string.Format("_Video/{0}", _type);

        if (_place.Equals("left"))
        {
            left_video.clip = Resources.Load<VideoClip>(_video_path);
        }
        else
        {
            right_video.clip = Resources.Load<VideoClip>(_video_path);
        }
    }

    public void stopVideo()
    {
        left_video.Stop();
        right_video.Stop();

        left_hint.SetActive(false);
        right_hint.SetActive(false);
    }
}
