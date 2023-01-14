using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume2D : MonoBehaviour
{
    #region Variáveis
    [SerializeField] Transform listenerTransform;
    [SerializeField] AudioSource[] audioSource;
    [SerializeField] float minDist = 1;
    [SerializeField] float maxDist = 100;
    #endregion

    #region Start, Update
    void Update()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            float dist = Vector3.Distance(audioSource[i].transform.position, listenerTransform.position);
            if (dist < minDist)
            {
                audioSource[i].volume = 1;
            }
            else if (dist > maxDist)
            {
                audioSource[i].volume = 0;
            }
            else
            {
                audioSource[i].volume = 1 - ((dist - minDist) / (maxDist - minDist));
            }
        }
    }
    #endregion
}
