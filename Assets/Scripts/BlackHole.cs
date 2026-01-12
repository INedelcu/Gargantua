using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

[ExecuteInEditMode]
public class BlackHole : MonoBehaviour
{
    public ComputeShader shader;

    private RenderTexture output = null;

    private uint cameraWidth = 0;
    private uint cameraHeight = 0;

    void Start()
    {
    }

    void OnDestroy()
    {
        output?.Release();
    }

    void Update()
    {
        if (cameraWidth != Camera.main.pixelWidth || cameraHeight != Camera.main.pixelHeight)
        {
            if (output)
                output.Release();

            RenderTextureDescriptor rtDesc = new RenderTextureDescriptor()
            {
                dimension = TextureDimension.Tex2D,
                width = Camera.main.pixelWidth,
                height = Camera.main.pixelHeight,
                depthBufferBits = 0,
                volumeDepth = 1,
                msaaSamples = 1,
                vrUsage = VRTextureUsage.OneEye,
                graphicsFormat = GraphicsFormat.R32G32B32A32_SFloat,
                enableRandomWrite = true,
            };

            output = new RenderTexture(rtDesc);
            output.Create();

            cameraWidth = (uint)Camera.main.pixelWidth;
            cameraHeight = (uint)Camera.main.pixelHeight;
        }
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        int kernelIndex = shader.FindKernel("CSMain");
        if (kernelIndex == -1)
        {
            Debug.LogWarning("Kernel CSMain not found!");
            Graphics.Blit(src, dest);
            return;
        }

        uint theadGroupSizeX = 0;
        uint theadGroupSizeY = 0;
        uint theadGroupSizeZ = 0;

        shader.GetKernelThreadGroupSizes(kernelIndex, out theadGroupSizeX, out theadGroupSizeY, out theadGroupSizeZ);

        // Input
        shader.SetInt(Shader.PropertyToID("g_CameraResX"), (int)cameraWidth);
        shader.SetInt(Shader.PropertyToID("g_CameraResY"), (int)cameraHeight);

        shader.SetFloat(Shader.PropertyToID("g_Zoom"), Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView * 0.5f));
        shader.SetFloat(Shader.PropertyToID("g_AspectRatio"), cameraWidth / (float)cameraHeight);

        // Output
        shader.SetTexture(kernelIndex, Shader.PropertyToID("g_Output"), output);

        int threadGroupCountX = (int)((cameraWidth + theadGroupSizeX - 1) / theadGroupSizeX);
        int threadGroupCountY = (int)((cameraHeight + theadGroupSizeY - 1) / theadGroupSizeY);
        int threadGroupCountZ = 1;

        shader.Dispatch(kernelIndex, threadGroupCountX, threadGroupCountY, threadGroupCountZ);

        Graphics.Blit(output, dest); 
    }
}
