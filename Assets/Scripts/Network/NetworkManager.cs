using System;
using System.Collections;
using System.Collections.Generic;
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
        Login(123);
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

    public void Login(int id)
    {
        Dictionary<short, object> param = new();
        param[0] = id;
        SendRequest(OpCode.Login, param);
    }
}