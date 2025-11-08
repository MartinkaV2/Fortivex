using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    [NonSerialized] public bool isPlacing = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (isPlacing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = mousePosition;
        }

        if(Input.GetMouseButtonDown(1))
        {
            isPlacing = false;
        }
    }
}
