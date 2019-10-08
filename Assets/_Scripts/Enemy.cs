using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f; // the speed in m/s
    public float fireRate = 0.3f; // seconds/shot (unsused)
    public float health = 10;
    public int score = 100; // points earned for destroying this

    public bool __________;

    public Bounds bounds; // the bounds of this and its children
    public Vector3 boundsCenterOffset; // distance of bounds.center from position


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    
    //this is a Propoerty: a method that acts like a field
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    void CheckOffscreen()
    {
        // if bouns are still their default value...
        if (bounds.size == Vector3.zero)
        {
            // then set them
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            // also find the diff between bounds.center & transform.position
            boundsCenterOffset = bounds.center - transform.position;
        }
        
        // every time, update the bounds to the current position
        bounds.center = transform.position + boundsCenterOffset;
        //check to see whether the bounds are completely offscreen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
        {
            // if this enemy has gone off the bottom edge of the screen
            if (off.y < 0)
            {
                // then destroy it
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                // enemies don't take damage unless their on screen
                // this stops the player from shooting them before they're visible
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero ||
                    Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }
                // hurt this enemy
                // get the damage amount from the Projectile.type & Main.W_DEFS
                health -= Main.W_DEFS[p.type].damageOnHit;
                if (health <= 0)
                {
                    // destroy this enemy
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }

}
