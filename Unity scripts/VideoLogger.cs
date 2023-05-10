using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.VideoioModule;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class VideoLogger : MonoBehaviour
{
    public RTSPVideo videoCapture;
    private VideoWriter writer;
    private StreamWriter videoLog;

    private int currentFrame = 0;

    private void Start()
    {
        writer = new VideoWriter();
    }

    public void StartLogging(string logDirectory)
    {
        var timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string videoName = "video_" + timestamp + ".avi";
        writer.open(logDirectory + "\\" + videoName, VideoWriter.fourcc('M', 'J', 'P', 'G'), 24, new Size(1920, 1080));

        if (writer.isOpened())
        {
            Debug.Log("Writer opened.");
            videoLog = File.AppendText(logDirectory + "\\" + "video_" + timestamp + ".csv");
        }
        else
        {
            Debug.Log("Writer failed to open.");
        }

        currentFrame = 0;
        videoCapture.ReceivedFrame += OnReceivedFrame;
    }

    public void StopLogging()
    {
        currentFrame = 0;
        videoCapture.ReceivedFrame -= OnReceivedFrame;
        if (writer != null && !writer.IsDisposed)
            writer.release();
    }

    private void OnReceivedFrame(object sender, Mat rgbMat)
    {
        if (writer.isOpened())
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
            videoLog.WriteLineAsync(time.ToString() + "," + ++currentFrame);
            videoLog.FlushAsync();
            //Debug.Log("Size:" + rgbMat.height() + "x" + rgbMat.width());
            writer.write(rgbMat);
        }
    }

    void OnDestroy()
    {
        if (writer != null && !writer.IsDisposed)
            writer.release();
    }
}