using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.VideoioModule;
using UnityEngine;
using UnityEngine.UI;

namespace OpenCVForUnityExample
{
    public class VideoCaptureExample : MonoBehaviour
    {
        VideoCapture capture;
        Mat rgbMat;
        Texture2D texture;

        bool isPlaying = false;


        Canvas canvas;
        Image video;


        private void Awake()
        {
            capture = new VideoCapture();
            rgbMat = new Mat();

            canvas = GetComponent<Canvas>();

            video = canvas.GetComponentInChildren<Image>();


            OnConnect();   
        }

        private void OnConnect()
        {
            capture.open("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4", Videoio.CAP_ANDROID);

           

            if (!capture.isOpened())
                Debug.LogError("Failed to open link " + "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4");
            else
                Debug.LogError("Connected to " + "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4");

            capture.grab();
            capture.retrieve(rgbMat);
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
            capture.set(Videoio.CAP_PROP_POS_FRAMES, 0);

            //video.image = texture;
            
            isPlaying = true;
        }

        void Update()
        {
            if (isPlaying)
            {
                if (capture.grab())
                {
                    Debug.Log("Grabbed image.");
                    capture.retrieve(rgbMat);
                    Imgproc.cvtColor(rgbMat, rgbMat, Imgproc.COLOR_BGR2RGB);
                    Utils.matToTexture2D(rgbMat, texture);
                }
            }
        }

        void OnDestroy()
        {
            capture.release();

            if (rgbMat != null)
                rgbMat.Dispose();
        }
    }
}