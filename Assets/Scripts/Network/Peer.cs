using System;
using System.Collections;
using System.Collections.Generic;
using TGClient;
using UnityEngine;

public class Peer : PeerBase
{
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
        Debug.Log("OnOperationResponse");
        Debug.Log(response.returnCode);
        Dictionary<short, object> data = response.parameters;
        string username = data[0].ToString();
        int coin = (int) Convert.ToInt64(data[1]);
        Debug.Log(data[2].GetType());
        Debug.Log($"username {username}, coin {coin}");
    }
}