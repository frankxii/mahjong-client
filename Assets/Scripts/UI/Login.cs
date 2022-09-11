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


    private void Awake()
    {
        loginBtn.onClick.AddListener(OnClick);
    }


    // 登录按钮回调
    private void OnClick()
    {
        int id = Convert.ToInt32(username.text);
        
    }
}