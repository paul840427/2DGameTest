using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScreenSizeTest
{
    float base_width;
    float base_height;
    Canvas canvas;
    Camera camera;

    public ScreenSizeTest(Canvas _canvas, Camera _camera)
    {
        #region For non-UI
        base_width = 1920f;
        base_height = 1080f;
        canvas = _canvas;
        camera = _camera;
        Debug.Log("camera.aspect:" + camera.aspect);
        Debug.Log(string.Format("base_width/base_height = {0:F4}", base_width / base_height));

        float orthographic_size = camera.orthographicSize;
        Debug.Log(string.Format("orthographicSize: {0:F4}", orthographic_size));
        float new_orthographic_size = (float)Screen.height / (float)Screen.width * base_width / base_height * orthographic_size;
        camera.orthographicSize = Mathf.Max(new_orthographic_size, orthographic_size);
        Debug.Log(string.Format("curr_size:({0:F4}, {1:F4}), size: {2:F4}",
            (float)Screen.width, (float)Screen.height, camera.orthographicSize));
        #endregion
    }

    ~ScreenSizeTest()
    {
        Debug.Log("ScreenSizeTest Destructor");
    }

    float getFactor()
    {
        // modify size by orthographicSize 
        float factor = base_height / (camera.orthographicSize * 2f);
        return factor;
    }
}

public class DestructorTest : MonoBehaviour
{
    public Canvas m_canvas;
    public Camera m_camera;
    ScreenSizeTest screen_size_test;

    // Start is called before the first frame update
    void Start()
    {
        screen_size_test = new ScreenSizeTest(m_canvas, m_camera);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
