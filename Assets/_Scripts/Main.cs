using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemySpawnPadding = 1.5f; // padding for position
    
    public WeaponDefinition[] weaponDefinitions;

    public bool _______;

    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate; // delay between Enemy spawns
    
    //public GameObject text = GameObject.Find("Score");

    void Awake()
    {
        S = this;
        // set Utils.camBounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        enemySpawnRate = 1f / enemySpawnPerSecond;    // 0.5 enemies/second - enemySpawnRate of 2
        Invoke("SpawnEnemy", enemySpawnRate);
        
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();    // a generic Dictionary with WeaponType as the key
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // check to make sure that the key exists in the Dictionary
        // attempting to retrieve a key that didn't exist would throw an error
        if (W_DEFS.ContainsKey(wt))
        {
            return (W_DEFS[wt]);
        }
        // this will return a definition for WeaponType.none which means it has failed to find the WeaponDefinition
        return ( new WeaponDefinition() );
    }

    private void Start()
    {
        // make a list of all of the active weapons
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }
    }

    public void SpawnEnemy()
    {
        // pick a random enemy prefab to instatiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
       
        // position the enemy above the screen with a random x position
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x + enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.camBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;
        
        //call spawnenemy() again in a couple of seconds
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    public void DelayedRestart(float delay)
    {
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        // reload _Scene_0 to restart the game
        Application.LoadLevel("SampleScene");
    }
}
