using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeDialog : MonoBehaviour
{
    public float changeTime;
    public GameObject dialog;
    void Start()
    {
        dialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
            dialog.SetActive(true);
    }
}
