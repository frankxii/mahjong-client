using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect : MonoBehaviour
{
    public Button btnConnect;
    public Button btnSend;

    private const string Host = "127.0.0.1";
    private const int Port = 8000;
    private Peer _peer;

    private void Start()
    {
        btnConnect.onClick.AddListener(OnConnectButtonClick);
        btnSend.onClick.AddListener(OnSendButtonClick);
    }

    private void OnConnectButtonClick()
    {
        _peer = new Peer();
        _peer.Connect(Host, Port);
    }

    private void OnSendButtonClick()
    {
        Dictionary<short, object> dict = new();
        _peer.SendRequest(1, dict);
    }

    private void Update()
    {
        if (_peer != null)
        {
            _peer.Service();
        }
    }
}