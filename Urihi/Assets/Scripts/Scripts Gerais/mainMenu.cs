using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void ControleDeCenas()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //serve para chamar outra cena ou 
        //ativar um canvas
    }
}
