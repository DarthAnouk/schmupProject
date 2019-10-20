using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// 
/// Enemy_4 will start offscreen and the pick a random point on screen to move to
/// once it has arrived, it will pick another random point and continue until shot down
/// 
/// </summary>


[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector] public GameObject go;
    [HideInInspector] public Material mat;
}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")] 
    
    public Part[] parts;
    
    private Vector3 p0, p1;    // the two point to interpolate
    private float timeStart;    // birth time for this Enemy_4
    private float duration = 4;    // duration of movement

    
    void Start()
    {
        p0 = p1 = pos;
        
        InitMovement();
        
        // cache GameObject and Material of each Part in parts
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }



    void InitMovement()
    {
        p0 = p1;    // set p0 to old p1
        
        // assign a new location on-screen to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;

        p1.x = UnityEngine.Random.Range(-widMinRad, widMinRad);
        p1.y = UnityEngine.Random.Range(-hgtMinRad, hgtMinRad);

        timeStart = Time.time;    // reset the time
    }


    public override void Move()
    {
        // this completely overrides Enemy.Move() with a linear interpolation
        
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);    // apply ease out easing to u
        
        pos = (1 - u) * p0 + u * p1;    // simple linear interpolation
    }


    // Find Part based on Name
    Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }

        return (null);
    }

    // Find Part based on GameObject
    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }

        return (null);
    }
    
    
    
    // These functions return true if the Part has been destroyed
    bool Destroyed (GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            return (true);    // yes it was destroyed
        }

        return (prt.health <= 0);
    }




    void ShowLocalizedDamage(Material m)    // change color of just one Part instead of whole ship
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    
    
    void OnCollisionEnter(Collider coll)
    {
        GameObject other = coll.gameObject;
        
    }
}
