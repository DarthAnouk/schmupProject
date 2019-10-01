﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BoundsTest
{
    center,
    onScreen,
    offScreen
}
public class Utils : MonoBehaviour
{
    // creates bounds that encapsulates the two Bounds passed in
    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        // if the size of one of the bounds is Vector3.zero, ignore that one
        if (b0.size == Vector3.zero && b1.size != Vector3.zero)
        {
            return (b1);
        }
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        
        // stretch b0 to include b1.min and b1.max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        // create an empty Bounds b
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);

        // if this GameObject has a renderer component...
        if (go.GetComponent<Renderer>() != null)
        {
            // expand b to contain the renderer's bounds
            b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        }

        // if this GameObject has a collider component...
        if (go.GetComponent<Collider>() != null)
        {
            // expand b to contain the collider's bounds
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        }

        // recursively iterate through each child of this gameObject.transform
        foreach (Transform t in go.transform)
        {
            // expand b to contain their Bounds as well
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }

        return (b);
    }

    static public Bounds camBounds
    {
        get
        {
            if (_camBounds.size == Vector3.zero)
            {
                SetCameraBounds();
            }

            return (_camBounds);
        }
    }

    static private Bounds _camBounds;

    public static void SetCameraBounds(Camera cam = null)
    {
        if (cam == null) cam = Camera.main;
        
        Vector3 topLeft = new Vector3(0,0,0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height,0);

        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;

        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // checks to see whether the Bounds bnd are within the camBounds
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return (BoundsInBoundsCheck(camBounds, bnd, test));
    }

    // checks to see whether Bounds lilB are within Bounds bigB
    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        // the behavior of this function is different based on the BoundsTest that has been selected
        
        // get the center of lilB
        Vector3 pos = lilB.center;
        
        // initialize the offset at [0,0,0]
        Vector3 off = Vector3.zero;

        switch (test)
        {
            // the center test determines what off (offset) would have to be applied to lilB to move its center back inside bigB
            case BoundsTest.center:
                if (bigB.Contains(pos))
                {
                    return (Vector3.zero);
                }

                if (pos.y > bigB.max.x)
                {
                    off.x = pos.x - bigB.max.x;
                }
                else if (pos.x < bigB.min.x)
                {
                    off.x = pos.x - bigB.min.x;
                }

                if (pos.y > bigB.max.y)
                {
                    off.y = pos.y - bigB.max.y;
                }
                else if (pos.y < bigB.min.y)
                {
                    off.y = pos.y - bigB.min.y;
                }

                if (pos.z > bigB.max.z)
                {
                    off.z = pos.z - bigB.max.z;
                }
                else if (pos.z < bigB.min.z)
                {
                    off.z = pos.z - bigB.min.z;
                }

                return (off);
            
            // the onScreen test determines what off would have to be applied to keep all of lilB inside bigB
            case BoundsTest.onScreen:
                if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max))
                {
                    return (Vector3.zero);
                }

                if (lilB.max.x > bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x < bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                
                if (lilB.max.y > bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y < bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                
                if (lilB.max.z > bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z < bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }

                return (off);
            
            // the offScreen test determines what off would need to be applied to move any tiny part of lilB inside of bigB
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if (cMin || cMax)
                {
                    return (Vector3.zero);
                }
                
                if (lilB.max.x > bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x < bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                
                if (lilB.max.y > bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y < bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                
                if (lilB.max.z > bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z < bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }

                return (off);
        }

        return (Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}