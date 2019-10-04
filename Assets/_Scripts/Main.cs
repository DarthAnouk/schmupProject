using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    static public Main S;

    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float eneySpawnPadding = 1.5f; // padding for position

    public bool _______;
    
    public float enemySpawnRate; // delay between Enemy spawns
    
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        // set Utils.camBounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        // 0.5 enemies/second - enemySpawnRate of 2
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        // pick a random enemy prefab to instatiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
        // position the enemy above the screen with a random x position
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + eneySpawnPadding;
        float xMax = Utils.camBounds.max.x + eneySpawnPadding;
        go.transform.position = pos;
        //call spawnenemy() again in a couple of seconds
        Invoke("SpawnEnemy", enemySpawnRate);
    }
}
