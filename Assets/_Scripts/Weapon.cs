using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


// this is an enum of the various possible weapon types
// it also includes a "shield" type to allow a shield power-up
public enum WeaponType
{
    none,
    blaster,
    spread,
    shield
}


// the WeaponDefinition class allows you to set the properties of a specific weapon in the Inspector
// Main has an array of WeaponsDefinitions

[System.Serializable] public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

// Note: Weapon prefabs, colors, etc. are set in Main.cs

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    public bool ________;
    private WeaponType _type = WeaponType.blaster;
    public  WeaponDefinition def;
    public GameObject collar;
    public float lastShot; // time last shot was fired

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        // call SetType() properly for the default _type
        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        
        // find the fireDelegate of the parent
        GameObject parentGO = transform.parent.gameObject;
        if (parentGO.tag == "Hero")
        {
            Hero.S.fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType (WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            print("Not active in hierarchy");
            return;
        }

        if (Time.time - lastShot < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-0.2f, 0.9f,0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(0.2f, 0.9f, 0) * def.velocity;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
    }
}
