using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

#region SubClasse
[System.Serializable]
public class Wave 
{
    public string waveName;
    public int numberEnemies;
    public GameObject[] typeEnemies;
    public float spawnInterval;
}
#endregion

public class WaveSpawner : MonoBehaviour
{
    #region Variáveis
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] Text waveName;

    private Animator anim;
    private Wave currentWave;
    private int currentWaveNumber;
    private bool canSpawn = true;
    private bool canAnimate;
    private float nextSpawnTime;

    #endregion

    #region Start, Update
    void Start() 
    {
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        if(totalEnemies.Length == 0) 
        {
            if(currentWaveNumber + 1 != waves.Length) 
            {
                if (canAnimate)
                {
                    waveName.text = waves[currentWaveNumber + 1].waveName;
                    anim.SetTrigger("OrdaCompleta");
                    canAnimate = false;
                }
            }
        }
    }
    #endregion

    #region Spawns
    void SpawnNextWave() //Ela está sendo usada na animação através de AnimationEvent.
    {
        currentWaveNumber++;
        canSpawn = true;
    }

    void SpawnWave() 
    {
        if (canSpawn && nextSpawnTime < Time.time) 
        {
            GameObject randomEnemy = currentWave.typeEnemies[Random.Range(0, currentWave.typeEnemies.Length)];
            Transform randomPositions = spawnPositions[Random.Range(0, spawnPositions.Length)];
            Instantiate(randomEnemy, randomPositions.position, Quaternion.identity);
            currentWave.numberEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            
            if(currentWave.numberEnemies == 0) 
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }
    #endregion
}
