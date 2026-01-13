using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

[ExecuteInEditMode]
public class BlackHole : MonoBehaviour
{
    public ComputeShader shader;

    public Cubemap envSpaceTexture = null;

    public Texture2D accretionDiskTexture = null;

    private RenderTexture output = null;

    private Texture2D accretionDiskGradient = null;

    private uint cameraWidth = 0;
    private uint cameraHeight = 0;

    void Start()
    {
    }
    
    void Update()
    {
    }

    void OnDestroy()
    {
        output?.Release();
    }
    
    void AllocateResources()
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

        if (accretionDiskGradient == null)
        {
            const uint size = 256;
            accretionDiskGradient = new Texture2D((int)size, 1, TextureFormat.RGBAFloat, false);
            accretionDiskGradient.hideFlags = HideFlags.HideAndDontSave;
            Color[] pixels = new Color[size];
            for (int i = 0; i < pixels.Length; i++)
            {
                float t = (float)i / (float)(pixels.Length - 1);
                float r = 3.5f;
                float g = 2.8f * Mathf.Exp(-2.0f * t);
                float b = 2.4f * Mathf.Exp(-5.0f * t);

                // Alpha gradient
                // Increase alpha sharply from 0 outwards
                float a = 1 - Mathf.Exp(-30.0f * t);

                // Decrease alpha smoothly from 0.2 to 1.0
                a *= 1 - Mathf.SmoothStep(0.2f, 1, t);

                pixels[i] = new Color(r, g, b, a);
            }
            accretionDiskGradient.SetPixels(pixels);
            accretionDiskGradient.Apply();
        }
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        AllocateResources();

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

        shader.SetFloat(Shader.PropertyToID("g_Time"), Time.realtimeSinceStartup);

        shader.SetVector(Shader.PropertyToID("g_CameraPos"), Camera.main.transform.position);

        shader.SetTexture(kernelIndex, Shader.PropertyToID("g_EnvTex"), envSpaceTexture);

        shader.SetTexture(kernelIndex, Shader.PropertyToID("g_AccretionDiskTex"), accretionDiskTexture);
        shader.SetTexture(kernelIndex, Shader.PropertyToID("g_AccretionDiskGradient"), accretionDiskGradient);

        // Output
        shader.SetTexture(kernelIndex, Shader.PropertyToID("g_Output"), output);

        int threadGroupCountX = (int)((cameraWidth + theadGroupSizeX - 1) / theadGroupSizeX);
        int threadGroupCountY = (int)((cameraHeight + theadGroupSizeY - 1) / theadGroupSizeY);
        int threadGroupCountZ = 1;

        shader.Dispatch(kernelIndex, threadGroupCountX, threadGroupCountY, threadGroupCountZ);

        Graphics.Blit(output, dest); 
    }
}
