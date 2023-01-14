using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableDialog : MonoBehaviour
{
    public float disable;
    public GameObject dialog;
    void Start()
    {
        
    }

  
    void Update()
    {
        disable -= Time.deltaTime;
        if (disable <= 0)
            dialog.SetActive(false);
    }
}
