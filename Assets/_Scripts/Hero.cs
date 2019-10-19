using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;    // Singleton

    public float gameRestartDelay = 2f;

    // these fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    
    // ship status information
    public float _shieldLevel = 1;
    public bool __________________;

   public Bounds bounds;

   // declare a new delegate type WeaponFireDelegate
   public delegate void WeaponFireDelegate();
   
   // create a WeaponFireDelegate field named fireDelegate
   public WeaponFireDelegate fireDelegate;

   public GameObject bullet1;
    
    
   
   
    // Start is called before the first frame update
    void Awake()
    {
        S = this;    //set the Singleton
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }

    
    
    // Update is called once per frame
    void Update()
    {
        // pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

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
        
        // A button is red firing, S is blue firing, D is green firing, F is purple firing
        
        /*
        if (Input.GetKey(KeyCode.LeftShift) == true && fireDelegate != null)
        {
            print("shift");
            fireDelegate();
        } 
        else if (Input.GetKey(KeyCode.Z) == true && fireDelegate != null)
        {
            print("Z");
            fireDelegate();
        }
        else if (Input.GetKey(KeyCode.X) == true && fireDelegate != null)
        {
            print("X");
            fireDelegate();
        }
        */

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
            
        }
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift) == true && fireDelegate != null)
        {
            print("shift");
           // Weapon.type = WeaponType.purpleBlaster;
            fireDelegate();
        } 
        else if (Input.GetKey(KeyCode.Z))
        {
            print("Z");
            fireDelegate();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            print("X");
            fireDelegate();
        }

    }
    
    
    
    // this variable holds a reference to the last triggering GameObject
    public GameObject lastTriggerGo = null;

    private void OnTriggerEnter(Collider other)
    {
        // find the tag of other.gameObject or its parent GameObjects
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        // if there is a parent with a tag
        if (go != null)
        {
            // make sure it's not the same triggering go as last time
            if (go == lastTriggerGo)
            {
                return;
            }
            lastTriggerGo = go;

            if (go.tag == "Enemy")
            {
                
                shieldLevel--;        // if the shield was triggered by an enemy, Decrease the level of the shield by 1
                Destroy(go);          // destroy the enemy
                
            }
            else
            {
                // announce it
                print("Triggered: " + go.name);
            }
        }
        else
        {
            print("Triggered: " + other.gameObject.name);
        }
    }

    
    
    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            
            //restart the game
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
