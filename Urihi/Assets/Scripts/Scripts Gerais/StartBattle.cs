using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    #region Variáveis
    [SerializeField] GameObject secondWave;
    [SerializeField] GameObject blockedPass;
   
    public static int points;
    #endregion

    #region Start, Update

    void Start() 
    {
        points = 0;
    }

    void Update() 
    {
        if(points == 180) 
        {
            blockedPass.SetActive(false);
            secondWave.SetActive(true);
            print(points);
        }
        if(points == 360) 
        {
           SceneManager.LoadScene(5);
        }
        print(points);
    }
    #endregion
}
