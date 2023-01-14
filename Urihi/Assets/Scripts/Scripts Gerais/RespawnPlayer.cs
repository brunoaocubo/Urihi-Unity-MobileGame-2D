using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    Player dead;
    bool isDead;
    public GameObject player;
    public Transform spawnPos;
    void Awake()
    {
        isDead = dead.isDead;
    }
    void Start() 
    {
        isDead = dead.isDead;
        if (isDead == true)
        {
            Instantiate(player, spawnPos.position, Quaternion.identity);
        }
    }
    void Update()
    {
        if(isDead == true) 
        {
            Instantiate(player, spawnPos.position, Quaternion.identity);
        }
    }
}
