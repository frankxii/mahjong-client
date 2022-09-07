using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        AutoLogin();
    }

    private void AutoLogin()
    {
        NetworkManager.Instance.Login(1);
    }
}