using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this is outside of the Utils class
public enum BoundsTest
{
    center, // is the center of the GameObject on screen?
    onScreen, // are the bounds entirely on screen?
    offScreen // are the bounds entirely off screen?
}
public class Utils : MonoBehaviour
{
    //======= MATERIALS FUNCTIONS ======\\
    
    // returns a list of all Materials on this GameObject and its children
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        
        List<Material> mats = new List<Material>();

        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }

        return (mats.ToArray());
    }
}
