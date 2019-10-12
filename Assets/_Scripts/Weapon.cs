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

