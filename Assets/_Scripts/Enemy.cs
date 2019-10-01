using System.Collections;
using System.Collections.Generic;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = Vector3.positiveInfinity;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    
    //this is a Propoerty: a method that acts like a field
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }
}
