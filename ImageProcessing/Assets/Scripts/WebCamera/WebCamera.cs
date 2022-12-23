using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System;

public class WebCamera : MonoBehaviour
{
    // Hidden variables
    private bool _cameraAvailable = false;
    private WebCamTexture _deviceCameraTexture;
    private WebCameraImageRenderFeature _webCameraFeature;

    // Inspector visible variables
    public AspectRatioFitter screenFitter;
    public Renderer2DData rendererData;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] cameras = WebCamTexture.devices;
        Debug.Assert(cameras.Length != 0);

        // Currently we pick only one camera that we need.
        _deviceCameraTexture = new WebCamTexture(cameras[0].name, Screen.width, Screen.height);
        Debug.Assert(_deviceCameraTexture != null);

        _webCameraFeature = GetMirrorFeature();
        Debug.Assert(_webCameraFeature != null);
        _webCameraFeature.CameraTexture = _deviceCameraTexture;

        // Start playing the texture
        _deviceCameraTexture.Play();

        _cameraAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_cameraAvailable)
        {
            return;
        }

        float ratio = (float)_deviceCameraTexture.width / (float)_deviceCameraTexture.height;
        //screenFitter.aspectRatio = ratio;

        float scaleY = _deviceCameraTexture.videoVerticallyMirrored ? -1f : 1f;
        //background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orientation = -_deviceCameraTexture.videoRotationAngle;
        //background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);

    }

    void Stop()
    {
        if (_cameraAvailable)
        {
            _deviceCameraTexture.Stop();
        }
    }

    WebCameraImageRenderFeature GetMirrorFeature()
    {
        Debug.Assert(rendererData != null);
        List<ScriptableRendererFeature> features = rendererData.rendererFeatures;
        return (WebCameraImageRenderFeature)features.Find(x => x.GetType() == Type.GetType("WebCameraImageRenderFeature"));
    }
}
