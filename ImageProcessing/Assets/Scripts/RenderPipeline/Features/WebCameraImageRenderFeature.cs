using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WebCameraImageRenderFeature : ScriptableRendererFeature
{
    // Private variables
    private Texture _cameraTexture;
    // Currently it's under investigation, when we actually want to copy our texture.
    // In general we should have the possibility to render transparent objects on top of the image.
    private RenderPassEvent _renderFeatureOrder = RenderPassEvent.BeforeRenderingTransparents;
    private FillPass _fillPass;

    // Properties
    public Texture CameraTexture
    {
        get { return _cameraTexture; }
        set
        {
            Debug.Assert(value != null);
            _cameraTexture = value;
            _cameraTexture.filterMode = FilterMode.Bilinear;
        }
    }

    // Public methods
    public override void Create()
    {
        _fillPass = new FillPass("Fill Screen Pass");
    }

    // called every frame once per camera
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Gather up and pass any extra information our pass will need.
        // Setup our passes
        _fillPass.TextureToFillCamera = _cameraTexture;

        // Ask the renderer to add our pass.
        // Could queue up multiple passes and/or pick passes to use
        renderer.EnqueuePass(_fillPass);
    }
}
