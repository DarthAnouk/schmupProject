using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    static public Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyDefaultPadding = 1.5f; // padding for position
    
    // Weapon Setup
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    public WeaponDefinition[] weaponDefinitions;
    
    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;

        bndCheck = GetComponent<BoundsCheck>();
        
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
        
        // a generic Dictionary with weapontype as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
    

    public void SpawnEnemy()
    {
        // pick a random enemy prefab to instatiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        // position the enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        // set the initial position for the spawned Enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        
        //call spawnenemy() again
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    
    
    public void DelayedRestart(float delay)
    {
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    
    
    public void Restart()
    {
        // reload _Scene_0 to restart the game
        SceneManager.LoadScene(("SampleScene"));
    }



    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // check to make sure the key exists in the dictionary
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        return (new WeaponDefinition());    // this returns a new WeaponDefinition with a type of WeaponType
    }
}
