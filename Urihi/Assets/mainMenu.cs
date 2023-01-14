using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class MainMenu : MonoBehaviour
    {
        public void controleDeCenas()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //serve para chamar outra cena ou 
                                                                                  //ativar um canvas
        }
    }
}