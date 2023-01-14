using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPos;

    [SerializeField] float spawnRate;
    private float nextSpawn = 0;
    void Start()
    {
    }

    void Update()
    {
        if(Time.time > nextSpawn) 
        {
            nextSpawn = Time.time + spawnRate;
            Instantiate(enemy, spawnPos.position, enemy.transform.rotation);
        }
    }

    IEnumerator SpawnEnemys() 
    {
        
        Instantiate(enemy, spawnPos);
        yield return new WaitForSeconds(2f);

    }
}
