using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FillPass : ScriptableRenderPass
{
    // Private memmbers
    string _profilerTag;
    [SerializeField, HideInInspector]
    Shader _fillShader;
    Material _fillMaterial;
    Texture _textureToFillCamera;

    // Properties
    public Texture TextureToFillCamera
    {
        set { _textureToFillCamera = value; }
    }

    // Public methods
    public FillPass(string profilerTag)
    {
        _profilerTag = profilerTag;

        // Setup the shader and the material
        _fillShader = Shader.Find("Hidden/FillScreen");
        _fillMaterial = CoreUtils.CreateEngineMaterial(_fillShader);

        // We revert camera output in the x-axis 
        _fillMaterial.SetVector("_InvertUVs", new Vector4(1, 0, 0, 0));
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var renderer = renderingData.cameraData.renderer;

        CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);

        cmd.Blit(_textureToFillCamera, renderer.cameraColorTargetHandle, _fillMaterial);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // called after Execute, use it to clean up anything allocated in Configure
    public override void FrameCleanup(CommandBuffer cmd)
    {
    }
}
