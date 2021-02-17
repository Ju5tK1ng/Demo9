using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFrame : MonoBehaviour
{
    //游戏的FPS，可在属性窗口中修改
    public int targetFrameRate = 60;
    public Text fpsText;
    public float deltaTime = 0f;
    void Awake ()
    {
        //修改当前的FPS
        Application.targetFrameRate = targetFrameRate;
    }
    void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil (fps).ToString ();
    }
}