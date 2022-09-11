using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Protocol;
using UnityEngine;


public class NetManager
{
    private Socket _socket;
    private Queue<string> _msgQueue = new();

    public delegate void View(string str);

    private static Dictionary<MessageId, View> router = new();


    // 添加响应和时间回调
    public static void AddListener(MessageId cmd, View view)
    {
        router[cmd] = view;
    }


    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="host">ip地址</param>
    /// <param name="port">端口</param>
    public async Task Connect(string host = "127.0.0.1", int port = 8000)
    {
        try
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await _socket.ConnectAsync(host, port);
            Debug.Log("服务器连接成功");
        }
        catch (Exception e)
        {
            Debug.Log("服务器连接失败: " + e.Message);
        }
    }

    /// <summary>
    /// 发送异步消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public async Task Send(MessageId id, object data)
    {
        try
        {
            byte[] message = ProtoUtil.Encode(id, data);
            await _socket.SendAsync(message, SocketFlags.None);
            Debug.Log("消息发送成功");
        }
        catch (Exception e)
        {
            Debug.Log("消息发送失败: " + e.Message);
        }
    }

    /// <summary>
    /// 异步线程接收服务端的消息
    /// </summary>
    public async Task Receive()
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            int count = await _socket.ReceiveAsync(buffer, SocketFlags.None);
            if (count == 0)
            {
                Debug.Log("服务端断开连接");
                break;
            }

            LoginAck ack = ProtoUtil.DecodeBody<LoginAck>(buffer);
            Debug.Log(ack.username);
        }
    }

    public void Serve()
    {
        if (_msgQueue.Count < 0) return;
        if (!_socket.Connected) return;
        string resp = _msgQueue.Dequeue();
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        _socket.Close();
    }
}

public class Client : MonoBehaviour
{
    private NetManager _client;

    private async void Start()
    {
        _client = new NetManager();
        await _client.Connect();
        _ = _client.Receive();
    }

    private void Update()
    {
        LoginReq data = new() {userId = 1, password = "password"};
        if (Time.frameCount % 180 == 1)
        {
            _ = _client.Send(MessageId.Login, data);
        }
    }

    private void OnDestroy()
    {
        _client.Close();
    }
}