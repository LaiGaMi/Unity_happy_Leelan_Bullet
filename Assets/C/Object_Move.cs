using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using EasyTransition;


public class Object_Move : MonoBehaviour
{
    [Header("影片播放器")]
    public VideoPlayer videoPlayer;

    [Header("場景切換設定")]
    public string sceneName;
    public TransitionSettings transitionSettings;
    public float loadDelay = 0f;

    private void Start()
    {
        // 訂閱影片播放完成事件
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 切換場景
        TransitionManager.Instance().Transition(
            sceneName,
            transitionSettings,
            loadDelay
        );
    }

    private void OnDestroy()
    {
        // 解除訂閱，避免記憶體問題
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}