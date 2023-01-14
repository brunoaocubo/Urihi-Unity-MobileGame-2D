using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowEnemy : MonoBehaviour
{

    [SerializeField] Transform objectFollow;
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (objectFollow != null)
            rectTransform.anchoredPosition = objectFollow.localPosition;
    }
}

