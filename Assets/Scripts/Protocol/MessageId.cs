using System.Collections.Generic;

namespace Protocol
{
    // 通信协议消息ID
    public enum MessageId : short
    {
        Login = 1000, // 登录
        CreateRoom = 1001, // 创建房间
        JoinRoom = 1002 // 加入房间
    }


    public class LoginReq
    {
        public string username;
        public string password;
    }
    
    public class Response<T>
    {
        public short code = 0;
        public string message = "OK";
        public T data;
    }

    public class LoginAck
    {
        public string username;
        public short id;
        public short gender;
        public int coin;
        public int diamond;
    }

    public class CreateRoomReq
    {
        public short userId;
        public short totalCycle;
    }

    public class PlayerInfo
    {
        public string username;
        public short id;
        public short dealerWind;
        public int coin;
    }

    public class CreateRoomAck
    {
        public short roomId;
        public short currentCycle;
        public short totalCycle;
        public byte dealerWind;
        public List<PlayerInfo> players;
    }
}