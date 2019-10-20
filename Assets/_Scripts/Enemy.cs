using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using _Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f; // the speed in m/s
    public float fireRate = 0.3f; // seconds/shot (unsused)
    public float health = 10;
    public int score = 100; // points earned for destroying this
    public float showDamageDuration = 0.1f;


    [Header("Set Dynamically: Enemy")] 
    public Color[] originalColors;
    public Material[] materials;    // all of the materials of this and its children
    public bool showingDamage = false;
    public float damageDoneTime;    // time to stop showing damage
    public bool notifiedOfDestruction = false;
    

    // this is a Property: a method that acts like a field
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    protected BoundsCheck bndCheck;


    
    
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        
        // get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }


    private void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }


    
    public virtual void Move()
    {
        Vector3 tempPos = pos;

        tempPos.y -= speed * Time.deltaTime;

        pos = tempPos;
    }


    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // if this enemy is off screen, don't damage it
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                
                // THIS IS WHERE YOU SHOULD ADD DIFFERENT DAMAGE BASED ON WEAPON USED
                
                // hurt this enemy
                ShowDamage();
                // get the damage amount from the Main WEAP_DICT
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero" + otherGO.name);
                break;
        }
    }




    // === Functions for making Enemies flash Red when Damaged === \\
    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }

        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }

        showingDamage = false;
    }
}
