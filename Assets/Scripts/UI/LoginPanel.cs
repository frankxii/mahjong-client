using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public static LoginPanel panel;

    public InputField inputUsername;
    public InputField inputPassword;
    public Button btnLogin;

    public static void ShowPanel()
    {
        if (panel is null)
        {
            // 获取Prefab
            GameObject panelPrefab = Resources.Load<GameObject>("UI/LoginPanel");
            // 实例化panel
            GameObject loginPanel = Instantiate(panelPrefab);
            // 挂载到UI canvas
            loginPanel.transform.SetParent(GameObject.Find("UICanvas").transform, false);
            // 绑定实例
            panel = loginPanel.GetComponent<LoginPanel>();
        }

        panel.gameObject.SetActive(true);
    }

    public static void HidePanel()
    {
        if (panel is not null)
            panel.gameObject.SetActive(false);
    }

    private void Start()
    {
        btnLogin.onClick.AddListener(() =>
        {
            string username = inputUsername.text;
            string password = inputPassword.text;
            Debug.Log(username + " " + password);
            HidePanel();
            LobbyPanel.ShowPanel();
        });
    }
}