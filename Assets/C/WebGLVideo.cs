using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class WebGLVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public string fileName = "intro.mp4";

    void Start()
    {
        string url = Path.Combine(
            Application.streamingAssetsPath,
            fileName
        );

        vp.source = VideoSource.Url;
        vp.url = url;

        vp.Prepare();
        vp.prepareCompleted += (v) =>
        {
            v.Play();
        };
    }
}