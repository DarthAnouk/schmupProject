using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;

    private Renderer rend;

    [Header("Set Dynamically")] 
    public Rigidbody rigid;
    
    [SerializeField] 
    private WeaponType _type;


    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }


    public void SetType(WeaponType eType)
    {
        _type = eType;    // set the type
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
