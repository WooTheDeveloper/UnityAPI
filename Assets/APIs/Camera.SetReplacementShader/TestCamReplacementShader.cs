// jave.lin 2019.08.29
using System.Collections;
using UnityEditor;
using UnityEngine;

public class TestCamReplacementShader : MonoBehaviour
{
    public Shader replaceShader;

    // 公开给外部Inspector中双击RT可查看内容
    public RenderTexture replaceColorRT;

    void Start()
    {
        if (replaceColorRT == null || replaceColorRT.width != Screen.width || replaceColorRT.height != Screen.height)
        {
            replaceColorRT = RenderTexture.GetTemporary(Screen.width, Screen.height);
        }

        var cam = GetComponent<Camera>();

        var srcTT = cam.targetTexture;
        var srcClearFlags = cam.clearFlags;
        var srcBgColor = cam.backgroundColor;

        // 测试RenderWithShader
        // RenderWithShader是调用时就会去执行替换，并渲染相机的内容
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.Color;
        cam.targetTexture = replaceColorRT;                     // 渲染目标到rt
        cam.RenderWithShader(replaceShader, "RenderWithShader");// 立刻渲染

        cam.targetTexture = srcTT;                              // 取消渲染目标
        cam.clearFlags = srcClearFlags;
        cam.backgroundColor = srcBgColor;
        
        // 仅替换SubShader中Tags带有Color属性定义的
        // 且replaceShader中也得有Color属性的Tags，否则啥都不显示，因为没有可使用的SubShader
        // 或是替换之前的渲染用的所有对象使用的shader中，也没有一个Color属性的SubShader，也不会显示任何内容
        // 假设：replaceShader中有3个subShader，Tags中的Color分别为："1","2","3"，3个
        // 如果替换之前的所有对象渲染用的shader中的subshader有对应Color属性的，且Color值为1、2、3之一，都会替换成，否则该替换失败的shader就不会显示
        cam.SetReplacementShader(replaceShader, "Color");
        // 替换所有渲染时用的shader
        //GetComponent<Camera>().SetReplacementShader(replaceShader, null);

        StartCoroutine(DelayResetReplacementShader(3, cam)); // 3秒后恢复
    }

    private IEnumerator DelayResetReplacementShader(float s, Camera cam)
    {
        yield return new WaitForSeconds(s);
        // 恢复的API
        cam.ResetReplacementShader();
    }
}

