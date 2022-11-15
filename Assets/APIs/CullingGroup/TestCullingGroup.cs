using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCullingGroup : MonoBehaviour
{
    public Transform SphereParent;
    public Camera cam;
    public BoundingSphere[] allBounds;
    private CullingGroup cg;
    private Transform[] allSpheres;
    private void OnEnable()
    {
        allSpheres = SphereParent.GetComponentsInChildren<Transform>();
        allBounds = new BoundingSphere[allSpheres.Length];
        for (int i = 0; i < allSpheres.Length; i++)
        {
            var tr = allSpheres[i];
            allBounds[i] = new BoundingSphere(tr.position,tr.localScale.x * 0.5f);
        }
        cg = new CullingGroup();
        cg.targetCamera = cam;
        cg.SetBoundingSpheres(allBounds);
        cg.onStateChanged = OnStateChanged; 
    }

    private void OnStateChanged(CullingGroupEvent cgEvent)
    {
        var index = cgEvent.index;
        var isVisible = cgEvent.isVisible;
        var curDistance = cgEvent.currentDistance;
        var preDistance = cgEvent.previousDistance;
        var wasVisible = cgEvent.wasVisible;
        var hasBecomeVisible = cgEvent.hasBecomeVisible;
        Debug.Log($"index{index}  isvisible{isVisible}  curDistance{curDistance}  preDistance{preDistance}  wasVisible{wasVisible}  hasBecomeVisible{hasBecomeVisible}");
    }

    private void Update()
    {
        for (int i = 0; i < allSpheres.Length; i++)
        {
            var tr = allSpheres[i];
            allBounds[i].position = tr.position;
            allBounds[i].radius = tr.localScale.x * 0.5f;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"isvisible{cg.IsVisible(1)}  distance{cg.GetDistance(1)}" );
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var tr = allSpheres[2];
            Debug.Log(tr.position);
            var bss = new BoundingSphere[1];
            bss[0].position = tr.position;
            bss[0].radius = tr.localScale.x / 2f;
            cg.SetBoundingSpheres(bss);
            var visible = cg.IsVisible(0);
            Debug.Log(visible);

        }
    }

    private void OnDisable()
    {
        cg.Dispose();
        cg = null;
    }
}
