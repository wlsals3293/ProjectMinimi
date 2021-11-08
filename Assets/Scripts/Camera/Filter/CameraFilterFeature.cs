using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraFilterFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        public Material blitMaterial = null;
        //public int blitMaterialPassIndex = -1;
        //public BufferType sourceType = BufferType.CameraColor;
        //public BufferType destinationType = BufferType.CameraColor;
        //public string sourceTextureId = "_SourceTexture";
        //public string destinationTextureId = "_DestinationTexture";
    }

    public Settings settings = new Settings();

    public Material BlitMaterial { get => settings.blitMaterial; }

    class RenderPass : ScriptableRenderPass
    {
        public Settings settings;


        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        private string profilerTag;


        public RenderPass(string tag)
        {
            profilerTag = tag;
            tempTexture.Init("_TempRT");
        }

        public void SetSource(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDesc, FilterMode.Bilinear);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

            Blit(cmd, source, tempTexture.Identifier(), settings.blitMaterial, 0);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    RenderPass scriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        scriptablePass = new RenderPass(name);
        scriptablePass.renderPassEvent = settings.renderPassEvent;
        scriptablePass.settings = settings;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blitMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blit Material. {0} pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            return;
        }

        scriptablePass.SetSource(renderer.cameraColorTarget);
        renderer.EnqueuePass(scriptablePass);
    }
}


