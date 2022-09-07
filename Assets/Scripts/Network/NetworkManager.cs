using System;
using System.Collections;
using System.Collections.Generic;
using TGClient;
using UnityEngine;
using UnityEngine.UI;


public enum OpCode : short
{
    Login = 1
}


public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    private Peer _peer;

    public GameObject lobby;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        string host = "127.0.0.1";
        int port = 8000;
        _peer = new Peer();
        _peer.Connect(host, port);
        Dictionary<OpCode, callBall> router = new()
        {
            {OpCode.Login, OnLogin}
        };
        _peer.RegisterRouter(router);
    }

    private void Update()
    {
        if (_peer != null)
        {
            _peer.Service();
        }
    }

    private void SendRequest(OpCode code, Dictionary<short, object> dict = null)
    {
        _peer.SendRequest((short) code, dict);
    }

    // 发起登录请求
    public void Login(int id)
    {
        Dictionary<short, object> param = new();
        param[0] = id;
        SendRequest(OpCode.Login, param);
    }

    // 登录成功回调
    private void OnLogin(ReceiveResponse response)
    {
        Dictionary<short, object> data = response.parameters;
        string username = data[0].ToString();
        int coin = (int) Convert.ToInt64(data[1]);
        int diamond = (int) Convert.ToInt64(data[2]);
        int gender = (int) Convert.ToInt64(data[3]);
        Debug.Log($"username {username}, coin {coin}");

        // 关闭登录页面，调起大厅，同时更新大厅数据
        GameObject.Find("Login").SetActive(false);
        lobby.SetActive(true);
        lobby.GetComponent<Lobby>().UpdateProfile(coin, diamond);
    }
}