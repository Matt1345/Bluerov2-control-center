using UnityEngine;
using System.Collections;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;

public class ProcessRTSPStream : MonoBehaviour
{
    public VideoCaptureToMatHelper videoCaptureHelper;
    public MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        videoCaptureHelper.Initialize();
        videoCaptureHelper.Play();
    }

    void Update()
    {
        if (videoCaptureHelper.IsPlaying() && videoCaptureHelper.DidUpdateThisFrame())
        {
            Mat frameMat = videoCaptureHelper.GetMat();

            Texture2D texture = new Texture2D(frameMat.cols(), frameMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(frameMat, texture);

            meshRenderer.material.mainTexture = texture;
        }

        Debug.Log("IsPlaying: " + videoCaptureHelper.IsPlaying() + ", DidUpdateThisFrame: " + videoCaptureHelper.DidUpdateThisFrame());
    }

    void OnDisable()
    {
        videoCaptureHelper.Dispose();
    }
}