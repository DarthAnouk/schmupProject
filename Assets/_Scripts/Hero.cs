using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;    // Singleton

    public float gameRestartDelay = 2f;

    
    [Header("Set in Inspector")]
    
    // these fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    
    [Header("Set Dynamically")]
    // ship status information
    private float shieldLevel = 1;
    
    // weapons fields
    public Weapon[] weapons;
    
    public bool __________________;

   public Bounds bounds;

   /* WEAPON DELEGATE SETUP */
   public delegate void WeaponFireDelegate();    // declare a new delegate type WeaponFireDelegate
   public WeaponFireDelegate fireDelegate;    // create a WeaponFireDelegate field named fireDelegate, calls on Fire() method in Weapon.cs

   
    void Awake()
    {
        if (S == null)
        {
            S = this;    //set the Singleton
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }

    private void Start()
    {
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }


    // Update is called once per frame
    void Update()
    {
        /* MOVEMENT CONTROL */
        
        // pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        bounds.center = transform.position;
        
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);    // keep the ship constrained to the screen bounds
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }
        
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);    // rotate the ship to make it feel more dynamic

        
        
        /* FIRING CONTROL */
        
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
            
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) == true && fireDelegate != null)
        {
            print("shift");
           // Weapon.type = WeaponType.purpleBlaster;
          // GetComponent<Weapon>().SetType(WeaponType.blueBlaster);
           fireDelegate();
            
        } 
        else if (Input.GetKey(KeyCode.Z) == true && fireDelegate != null)
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
        GameObject go = Utils.FindTaggedParent(other.gameObject);    // find the tag of other.gameObject or its parent GameObjects
        
        if (go != null)    // if there is a parent with a tag
        {
            if (go == lastTriggerGo)    // make sure it's not the same triggering go as last time
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


    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }

        return (null);
    }

    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
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
