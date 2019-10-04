using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;    // Singleton

    // these fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    
    // ship status information
    public float shieldLevel = 1;
    public bool __________________;

   // public bool;
   public Bounds bounds;
    
    // Start is called before the first frame update
    void Awake()
    {
        S = this;    //set the Singletom
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vetical");

        // change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;
        
        // keep the ship constrained to the screen bounds
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }
        
        // rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Triggered: " + other.gameObject.name);
    }
}
