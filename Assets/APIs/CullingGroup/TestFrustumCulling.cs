using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TestFrustumCulling : MonoBehaviour
{
    public bool UseImplovedWay = true;
    private bool isCull;
    public Vector3 center;
    public Vector3 size;

    
    private void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }
    
    private void Update()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(SceneView.lastActiveSceneView.camera);
        if (UseImplovedWay)
        {
            isCull = BetterCamCull(planes);
        }
        else
        {
            isCull = CamCull(planes);    //该剔除方案有缺陷，实际上存在bounds 与视锥相交，但8个顶点都在视锥外的情况
        }
    }

    private bool IsPointInFrustum(Vector3 point,Plane[] planes)
    {
        for (int i = 0; i < planes.Length; i++)
        {
            if (!planes[i].GetSide(point))
            {
                return false;
            }
        }
        return true;
    }
    
    private bool IsPointInFrustum(Vector3 point,Plane[] planes,float extent)
    {
        for (int i = 0; i < planes.Length; i++)
        {
            if (!planes[i].GetSide(point + planes[i].normal * extent))
            {
                return false;
            }
        }
        return true;
    }

    
    private bool CamCull(Plane[] cullingPlanes)
    {
        Bounds bounds = new Bounds(center,size);
        var corners = new Vector3[8];
        corners[0] = bounds.min;
        corners[6] = bounds.max;
        corners[1] = new Vector3(corners[6].x,corners[0].y,corners[0].z);
        corners[2] = new Vector3(corners[6].x,corners[0].y,corners[6].z);
        corners[3] = new Vector3(corners[0].x,corners[0].y,corners[6].z);
        corners[4] = new Vector3(corners[0].x,corners[6].y,corners[0].z);
        corners[5] = new Vector3(corners[6].x,corners[6].y,corners[0].z);
        corners[7] = new Vector3(corners[0].x,corners[6].y,corners[6].z);
        for (int i = 0; i < 8; i++)
        {
            if (IsPointInFrustum(corners[i],cullingPlanes))
            {
                return false;
            }
        }

        return true;
    }

    private bool BetterCamCull(Plane[] cullingPlanes)
    {
        Bounds bounds = new Bounds(center,size);
        if (IsPointInFrustum(bounds.center,cullingPlanes,bounds.size.magnitude/2.0f))
        {
            return false;
        }

        return true;
    }
    

  
    private void OnDrawGizmos()
    {
        var origCol = Gizmos.color;
        if (isCull)
        {
            Gizmos.color = new Color(1,0,0,0.5f);    
            Gizmos.DrawCube(center,size);
        }
        else
        {
            Gizmos.color = new Color(0,1,0,0.5f);    
            Gizmos.DrawCube(center,size);
        }

        Gizmos.color = origCol;
        Gizmos.DrawSphere(SceneView.lastActiveSceneView.camera.transform.position,0.5f);
    }
}
