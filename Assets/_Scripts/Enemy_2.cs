using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    public float sinEccentricity = 0.6f;        // determines how much the Sine wave will affect movement
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")] 
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

        
    // Start is called before the first frame update
    void Start()
    {
        // pick any point on the left side of the screen
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        
        
        // pick any point on the right side of the screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        
        //possibly swap sides
        if (Random.value > 0.5f)
        {
            // setting .x of each o[point to its negative will move it to the other side of the screen
            p0.x *= -1;
            p1.x *= -1;
        }
        
        // set the birthTime to the current time
        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        
        // if u>1, then it has been longer that lifeTime since birthTime
        if (u > 1)
        {
            // this Enemy_2 has finished its life
            Destroy(this.gameObject);
            return;
        }
        
        // adjust u by adding an easing curve based on the Sine wave
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        
        // interpolate the two linear interpolation points
        pos = (1-u)*p0 + u*p1;
    }
}
