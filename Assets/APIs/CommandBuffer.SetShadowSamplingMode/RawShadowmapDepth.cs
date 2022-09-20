using UnityEngine;
using UnityEngine.Rendering;

//    Shadowmap render textures are normally set up to be sampled with a comparison filter - the sampler gets a shadow-space depth value of the screen pixel and returns 0 or 1,
//depending on if the depth value in the shadowmap is smaller or larger. That's the default ShadowSamplingMode.CompareDepths mode and is used for rendering shadows.
//    If you'd like to access the shadowmap values as in a regular texture, set the sampling mode to ShadowSamplingMode.RawDepth.
//    Shadowmap's sampling mode will be reverted to default after the the last command in the current CommandBuffer.
//    Check SystemInfo.supportsRawShadowDepthSampling to verify if current runtime platform supports sampling shadows this way. Notably, DirectX9 does not.

[RequireComponent(typeof(Camera))]
public class RawShadowmapDepth : MonoBehaviour
{
    public Light m_Light;
    RenderTexture m_ShadowmapCopy;

    void Start()
    {
        RenderTargetIdentifier shadowmap = BuiltinRenderTextureType.CurrentActive;
        m_ShadowmapCopy = new RenderTexture(1024, 1024, 0);
        CommandBuffer cb = new CommandBuffer();

        // Change shadow sampling mode for m_Light's shadowmap.
        cb.SetShadowSamplingMode(shadowmap, ShadowSamplingMode.RawDepth);

        // The shadowmap values can now be sampled normally - copy it to a different render texture.
        cb.Blit(shadowmap, new RenderTargetIdentifier(m_ShadowmapCopy));

        // Execute after the shadowmap has been filled.
        m_Light.AddCommandBuffer(LightEvent.AfterShadowMap, cb);

        // Sampling mode is restored automatically after this command buffer completes, so shadows will render normally.
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Display the shadowmap in the corner.
        Debug.Log("OnRenderImag");
        Camera.main.rect = new Rect(0, 0, 0.5f, 0.5f);
        Graphics.Blit(m_ShadowmapCopy, dest);
        Camera.main.rect = new Rect(0, 0, 1, 1);
    }
}