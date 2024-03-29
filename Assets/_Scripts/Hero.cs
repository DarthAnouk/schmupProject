﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;    // Singleton

    [Header("Set in Inspector")]
    // these fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    
    public float gameRestartDelay = 2f;

    // set up for shooting
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    
    
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;        // ship status information


    private GameObject lastTriggerGo = null;    // holds a reference to the last triggered GameObject
    
   
   
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
        
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);    // rotate the ship to make it feel more dynamic
        
        
        
        
        //====== FIRING COMMANDS ======\\
        
        // Regular Blaster
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.transform.position = transform.position;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
            rigidB.velocity = Vector3.up * projectileSpeed;

            Projectile proj = projGO.GetComponent<Projectile>();
            proj.type = WeaponType.blaster;
            float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
            rigidB.velocity = Vector3.up * tSpeed;
        }

        // Red Blaster
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.transform.position = transform.position;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
            rigidB.velocity = Vector3.up * projectileSpeed;

            Projectile proj = projGO.GetComponent<Projectile>();
            proj.type = WeaponType.purpleBlaster;
            float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
            rigidB.velocity = Vector3.up * tSpeed;
        }
        
        // Blue Blaster
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K))
        {
            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.transform.position = transform.position;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
            rigidB.velocity = Vector3.up * projectileSpeed;

            Projectile proj = projGO.GetComponent<Projectile>();
            proj.type = WeaponType.blueBlaster;
            float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
            rigidB.velocity = Vector3.up * tSpeed;
        }
        
        // Green Blaster
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.L))
        {
            GameObject projGO = Instantiate<GameObject>(projectilePrefab);
            projGO.transform.position = transform.position;
            Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
            rigidB.velocity = Vector3.up * projectileSpeed;

            Projectile proj = projGO.GetComponent<Projectile>();
            proj.type = WeaponType.greenBlaster;
            float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
            rigidB.velocity = Vector3.up * tSpeed;
        }
    }


    // currently unused
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }


    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        print("Triggered " + other.gameObject.name);
        
        // make sure it's not the same triggering go as last time
        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if (go.layer == 9) // if the shield was triggered by an enemy, Enemy Layer is 9
        {
            shieldLevel--;     // decrease the level of the shield by 1
            Destroy(go);       // and destroy the enemy
        }
        else
        {
            print("Triggered by non-enemy: " + go.name);
        }
    }


    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            // if the shield is going to be set less than zero, destroy the hero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
