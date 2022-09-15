using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            LoginPanel.ShowPanel();
        }
        else if (Input.GetKey(KeyCode.N))
        {
            LoginPanel.HidePanel();
        }
    }
}