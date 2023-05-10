using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.VideoioModule;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenCVForUnityExample
{
    public class VideoController : MonoBehaviour
    {
        public RTSPVideo videoCapture;
        Texture2D texture;
        bool isTextureUninitalized = true;

        Canvas canvas;
        RawImage video;

        private Queue<Action> actions = new();

        private void Awake()
        {
            canvas = GetComponent<Canvas>();

            video = canvas.GetComponentInChildren<RawImage>();

            videoCapture.Connect("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4");
            //videoCapture.ReceivedFrame += (object sender, Mat rgbMat) => actions.Enqueue(() => ShowFrame(rgbMat));
        }

        private void ShowFrame(Mat rgbMat)
        {
            if (isTextureUninitalized)
            {
                int frameWidth = rgbMat.cols();
                int frameHeight = rgbMat.rows();
                texture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);
                gameObject.transform.localScale = new Vector3((float)frameWidth, (float)frameHeight, 1);
                float widthScale = (float)Screen.width / (float)frameWidth;
                float heightScale = (float)Screen.height / (float)frameHeight;
                if (widthScale < heightScale)
                {
                    Camera.main.orthographicSize = ((float)frameWidth * (float)Screen.height / (float)Screen.width) / 2;
                }
                else
                {
                    Camera.main.orthographicSize = (float)frameHeight / 2;
                }
                video.texture = texture;
                isTextureUninitalized = false;
            }

            Imgproc.cvtColor(rgbMat, rgbMat, Imgproc.COLOR_BGR2RGB);
            Utils.matToTexture2D(rgbMat, texture);
        }


        void Update()
        {
            //while (actions.Count > 0)
                //actions.Dequeue()();
        }
    }
}