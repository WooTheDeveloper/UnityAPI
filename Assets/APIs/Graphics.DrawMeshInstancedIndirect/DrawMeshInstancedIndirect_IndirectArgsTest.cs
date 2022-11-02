using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMeshInstancedIndirect_IndirectArgsTest : MonoBehaviour
{
    private int instanceCount;
    private int subMeshIndex = 0;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[15];
    private Bounds bounds;
    public ComputeShader computeShader;
    private int kernelIdx;
    void Start() {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        bounds = new Bounds(Vector3.zero, new Vector3(100000.0f, 100000.0f, 100000.0f));
        List<MatrixData> bufferData= new List<MatrixData>();
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x,0,z) * size;
                Matrix4x4 o2w = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 0.23f);
                Matrix4x4 w2o = o2w.inverse;
                bufferData.Add(new MatrixData(){o2w = o2w,w2o = w2o});
            }
        }
        kernelIdx = computeShader.FindKernel("CSMain");
        instanceCount = width * height;

        if (lod0Mesh != null)
            subMeshIndex = Mathf.Clamp(subMeshIndex, 0, lod0Mesh.subMeshCount - 1);
        
        matrixDataComputeBufferIn = new ComputeBuffer(instanceCount, 4 * 32);
        matrixDataComputeBufferIn.SetData(bufferData);
        computeShader.SetBuffer(kernelIdx,"MatrixDataBufferIN",matrixDataComputeBufferIn);
        matrixDataComputeBufferLOD0 = new ComputeBuffer(instanceCount, 4 * 32);
        computeShader.SetBuffer(kernelIdx,"MatrixDataBufferLOD0",matrixDataComputeBufferLOD0);
        matrixDataComputeBufferLOD1 = new ComputeBuffer(instanceCount, 4 * 32);
        computeShader.SetBuffer(kernelIdx,"MatrixDataBufferLOD1",matrixDataComputeBufferLOD1);
        matrixDataComputeBufferLOD2 = new ComputeBuffer(instanceCount, 4 * 32);
        computeShader.SetBuffer(kernelIdx,"MatrixDataBufferLOD2",matrixDataComputeBufferLOD2);
        
        computeShader.SetInt("instanceCount",instanceCount);
        
        mat0.SetBuffer("MatrixDataBuffer", matrixDataComputeBufferLOD0);
        mat1.SetBuffer("MatrixDataBuffer", matrixDataComputeBufferLOD1);
        mat2.SetBuffer("MatrixDataBuffer", matrixDataComputeBufferLOD2);
        
        args[0] = (uint)lod0Mesh.GetIndexCount(subMeshIndex);
        args[1] = (uint)0;
        args[2] = (uint)lod0Mesh.GetIndexStart(subMeshIndex);
        args[3] = (uint)lod0Mesh.GetBaseVertex(subMeshIndex);
            
        args[5] = (uint)lod1Mesh.GetIndexCount(subMeshIndex);
        args[6] = (uint)0;
        args[7] = (uint)lod1Mesh.GetIndexStart(subMeshIndex);
        args[8] = (uint)lod1Mesh.GetBaseVertex(subMeshIndex);
            
        args[10] = (uint)lod2Mesh.GetIndexCount(subMeshIndex);
        args[11] = (uint)0;
        args[12] = (uint)lod2Mesh.GetIndexStart(subMeshIndex);
        args[13] = (uint)lod2Mesh.GetBaseVertex(subMeshIndex);
      
        argsBuffer.SetData(args);
        computeShader.SetBuffer(kernelIdx,"argsBuffer",argsBuffer);
    }

    private void ResetArgs()
    {
        args[0] = args[0] = args[0] = 20;
        argsBuffer.SetData(args);
        computeShader.SetBuffer(kernelIdx,"argsBuffer",argsBuffer);
    }


    public int width = 10;
    public int height = 10;
    public float size = 1;

    public Mesh lod0Mesh;
    public Mesh lod1Mesh;
    public Mesh lod2Mesh;
    public Material mat0;
    public Material mat1;
    public Material mat2;

    private ComputeBuffer matrixDataComputeBufferIn;
    private ComputeBuffer matrixDataComputeBufferLOD0;
    private ComputeBuffer matrixDataComputeBufferLOD1;
    private ComputeBuffer matrixDataComputeBufferLOD2;

    public struct MatrixData
    {
        public Matrix4x4 o2w;
        public Matrix4x4 w2o;
    }
    

    void Update()
    {
        ResetArgs();
        
        computeShader.Dispatch(kernelIdx,Mathf.CeilToInt( instanceCount/64.0f),1,1);
       
        
        Graphics.DrawMeshInstancedIndirect(lod0Mesh, subMeshIndex, mat0,bounds, argsBuffer,0);
        Graphics.DrawMeshInstancedIndirect(lod1Mesh, subMeshIndex, mat1,bounds, argsBuffer,4*5);
        Graphics.DrawMeshInstancedIndirect(lod2Mesh, subMeshIndex, mat2,bounds, argsBuffer,4*10);
        
        argsBuffer.GetData(outArgs);
        Debug.Log(outArgs[1]);
    }

    private uint[] outArgs = new uint[15];

    void OnDisable() {
        if (matrixDataComputeBufferIn != null)
            matrixDataComputeBufferIn.Release();
        matrixDataComputeBufferIn = null;
        
        if (matrixDataComputeBufferLOD0 != null)
            matrixDataComputeBufferLOD0.Release();
        matrixDataComputeBufferLOD0 = null;
        
        if (matrixDataComputeBufferLOD1 != null)
            matrixDataComputeBufferLOD1.Release();
        matrixDataComputeBufferLOD1 = null;
        
        if (matrixDataComputeBufferLOD2 != null)
            matrixDataComputeBufferLOD2.Release();
        matrixDataComputeBufferLOD2 = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }
}
