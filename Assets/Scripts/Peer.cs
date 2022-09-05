using System;
using System.Collections;
using System.Collections.Generic;
using TGClient;
using UnityEngine;

public class Peer : PeerBase
{
    public override void OnConnected(string message)
    {
        Debug.Log("OnConnected");
    }

    public override void OnDisConnect(Exception connectException)
    {
        Debug.Log("OnDisConnect");
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
    }
}