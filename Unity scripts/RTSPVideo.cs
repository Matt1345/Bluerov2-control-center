using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.VideoioModule;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RTSPVideo : MonoBehaviour
{
    public event EventHandler<Mat> ReceivedFrame;

    VideoCapture capture;
    Mat rgbMat;

    bool isPlaying = false;

    private void Start()
    {
        capture = new VideoCapture();
        rgbMat = new Mat();
  
    }

    public void Connect(string videoAddress)
    {
        capture = new VideoCapture();
        rgbMat = new Mat();

        capture.open(videoAddress, Videoio.CAP_FFMPEG);

        if (!capture.isOpened())
        {
            Debug.LogError("Failed to open link " + videoAddress);
            return;
        }

        Debug.Log("Connected to " + videoAddress);
        capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);
        isPlaying = true;
    }

    void Update()
    {
        if (isPlaying && capture.grab())
        {
            capture.retrieve(rgbMat);
            OnReceivedFrame(rgbMat);
        }
    }

    protected virtual void OnReceivedFrame(Mat rgbMat)
    {
        ReceivedFrame?.Invoke(this, rgbMat);
    }

    void OnDestroy()
    {
        capture.release();

        if (rgbMat != null)
            rgbMat.Dispose();
    }
}