using System;
using System.Collections;
using System.Collections.Generic;
using TGClient;
using UnityEngine;

public delegate void callBall(ReceiveResponse response);

public class Peer : PeerBase
{
    private Dictionary<OpCode, callBall> _router = null;

    public override void OnConnected(string message)
    {
        Debug.Log("服务器已连接");
    }

    public override void OnDisConnect(Exception connectException)
    {
        Debug.Log("服务器已断开");
    }

    public override void OnEvent(short eventCode, Dictionary<short, object> dict)
    {
        Debug.Log("OnEvent");
        Debug.Log("event: " + eventCode);
    }

    public override void OnException(Exception exception)
    {
        Debug.Log("OnException");
        Debug.Log(exception);
    }

    public override void OnOperationResponse(short opreationCode, ReceiveResponse response)
    {
        if (_router is not null)
        {
            _router[(OpCode) opreationCode](response);
        }
    }

    public void RegisterRouter(Dictionary<OpCode, callBall> router)
    {
        _router = router;
    }
}