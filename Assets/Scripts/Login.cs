using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField username;
    public Button loginBtn;

    private void Start()
    {
        loginBtn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        int id = Convert.ToInt32(username.text);
        NetworkManager.Instance.Login(id);
    }
}