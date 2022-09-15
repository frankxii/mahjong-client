using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Protocol;
using UnityEngine;

public struct Message
{
    public MessageId messageId;
    public string jsonString;
}

public delegate void View(string str);

public class NetManager
{
    private Socket _socket;
    private Queue<Message> _messageQueue = new(); // 网络读取写入到消息队列，主线程再进行消费
    public const short BUFFER_SIZE = 1024; // 接收缓冲区大小，1k
    private byte[] _readBuffer = new byte[BUFFER_SIZE]; //接收缓冲区
    private short _bufferCount; //有效字节数

    private static Dictionary<MessageId, Action<Message>> router = new();


    // 添加响应和时间回调
    public static void AddListener(MessageId cmd, Action<Message> view)
    {
        router.Add(cmd, view);
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
        if (_socket is null)
        {
            return;
        }

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
            // 定义接收缓冲区片段，用于接收新的字节
            ArraySegment<byte> buffer = new(_readBuffer, _bufferCount, BUFFER_SIZE - _bufferCount);
            int count = await _socket.ReceiveAsync(buffer, SocketFlags.None);
            // 记录缓冲区有效字节数
            _bufferCount += Convert.ToInt16(count);

            // count为0表示断开连接
            if (count == 0)
            {
                Debug.Log("服务端断开连接");
                break;
            }

            // 字节数小于2说明解析消息长度都不够，等待下次接收
            if (_bufferCount < 2)
                continue;

            while (true)
            {
                short length = ProtoUtil.DecodeLength(_readBuffer);
                // 当前buffer有效数据小于消息包体长度，属于半包，不处理
                if (_bufferCount < length)
                    break;

                MessageId id = ProtoUtil.DecodeId(_readBuffer);

                string json = ProtoUtil.DecodeJsonBody(_readBuffer);
                // 组装消息id和json字符数据，加入消息队列等待消费
                _messageQueue.Enqueue(new Message() {messageId = id, jsonString = json});
                // count等于0表示所有数据已处理完

                if (length == _bufferCount)
                {
                    _bufferCount = 0;
                    break;
                }
                else
                {
                    _bufferCount -= length;
                    // 数组移位
                    Array.Copy(_readBuffer, length, _readBuffer, 0, _bufferCount);
                }

                await Task.Delay(10);
            }
        }
    }

    public void Serve()
    {
        // Debug.Log(_messageQueue.Count);
        if (_socket is null || !_socket.Connected) return;
        if (_messageQueue.Count < 1) return;
        Message message = _messageQueue.Dequeue();
        // 分配路由
        Action<Message> view = router[message.messageId];
        view.Invoke(message);
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
        NetManager.AddListener(MessageId.Login, OnLogin);
    }

    private void Update()
    {
        if (_client is null)
        {
            return;
        }

        if (Time.frameCount % 5 == 1)
        {
            LoginReq data = new() {userId = 1, password = "password"};
            _ = _client.Send(MessageId.Login, data);
        }

        // 消费消息
        _client.Serve();
    }

    private void OnLogin(Message message)
    {
        Debug.Log(message.jsonString);
    }

    private void OnDestroy()
    {
        _client.Close();
    }
}