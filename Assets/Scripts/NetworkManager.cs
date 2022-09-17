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


public class Client
{
    private TcpClient _client;
    private NetworkStream _stream;
    private Queue<Message> _messageQueue = new(); // 网络读取写入到消息队列，主线程再进行消费
    public const short BUFFER_SIZE = 1024; // 接收缓冲区大小，1k
    private byte[] _readBuffer = new byte[BUFFER_SIZE]; //接收缓冲区
    private short _bufferCount; //有效字节数

    private Dictionary<MessageId, Action<Message>> _router = new();


    // 添加响应和时间回调
    public void AddListener(MessageId cmd, Action<Message> callback)
    {
        // 消息ID已绑定过回调
        if (_router.ContainsKey(cmd))
        {
            bool hasBeenRegistered = false;
            // 遍历委托方法列表，如果回调方法已绑定过，就不绑定
            foreach (Delegate @delegate in _router[cmd].GetInvocationList())
            {
                if (@delegate.Method.Equals(callback.Method))
                {
                    hasBeenRegistered = true;
                    break;
                }
            }

            if (!hasBeenRegistered)
                _router[cmd] += callback;
        }
        else
        {
            // 消息ID未注册过回调，直接绑定
            _router.Add(cmd, callback);
        }
    }


    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="host">ip地址</param>
    /// <param name="port">端口</param>
    public async Task ConnectAsync(string host = "127.0.0.1", int port = 8000)
    {
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);
            _stream = _client.GetStream();
            Debug.Log("服务器连接成功");
        }
        catch (Exception e)
        {
            Debug.Log("服务器连接失败: " + e.Message);
        }
    }

    public void Send(MessageId id, object data)
    {
        if (_client is null || !_client.Connected)
            return;

        try
        {
            byte[] message = ProtoUtil.Encode(id, data);
            _ = _stream.WriteAsync(message);
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
    public async Task ReceiveAsync()
    {
        while (true)
        {
            // 定义接收缓冲区片段，用于接收新的字节
            ArraySegment<byte> buffer = new(_readBuffer, _bufferCount, BUFFER_SIZE - _bufferCount);
            int bytesCount = await _stream.ReadAsync(buffer);
            // 记录缓冲区有效字节数
            _bufferCount += Convert.ToInt16(bytesCount);

            // count为0表示断开连接
            if (bytesCount == 0)
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

                _bufferCount -= length;
                // count等于0表示所有数据已处理完
                if (_bufferCount == 0)
                    break;
                else
                    // 数组移位
                    Array.Copy(_readBuffer, length, _readBuffer, 0, _bufferCount);
            }
        }
    }

    // 处理服务端推送消息
    public void Serve()
    {
        if (_messageQueue.Count < 1) return;
        Message message = _messageQueue.Dequeue();
        // 分配路由
        Action<Message> callback = _router[message.messageId];
        callback(message);
    }

    // 关闭连接
    public void Close()
    {
        _stream.Close();
        _client.Close();
    }
}

public class NetworkManager : MonoBehaviour
{
    private Client _client;
    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private async void Start()
    {
        _client = new Client();
        await _client.ConnectAsync();
        _ = _client.ReceiveAsync();
    }

    private void Update()
    {
        if (_client is null)
        {
            return;
        }

        // 消费消息
        _client.Serve();
    }

    public void AddListener(MessageId cmd, Action<Message> callback)
    {
        _client.AddListener(cmd, callback);
    }

    public void Send(MessageId cmd, object req)
    {
        _client.Send(cmd, req);
    }

    private void OnDestroy()
    {
        _client.Close();
        Instance = null;
    }
}