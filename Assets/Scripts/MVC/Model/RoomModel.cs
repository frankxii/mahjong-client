using System.Collections.Generic;
using Data;
using MVC.Base;

namespace MVC.Model
{
    public class RoomModel : BaseModel<RoomModel>
    {
        public int RoomId { get; private set; } // 房间ID
        public short CurrentCycle { get; private set; } // 当前圈数
        public short TotalCycle { get; private set; } // 总圈数
        public byte DealerWind { get; private set; } // 门风
        public List<PlayerInfo> Players { get; private set; } // 玩家信息


        public void UpdateRoomInfo(RoomInfo info, UserModel user)
        {
            RoomId = info.roomId;
            CurrentCycle = info.currentCycle;
            TotalCycle = info.totalCycle;
            Players = info.players;
            // 遍历玩家信息，找出本家的门风
            foreach (PlayerInfo player in info.players)
            {
                if (player.userId == user.UserId)
                {
                    DealerWind = player.dealerWind;
                    break;
                }
            }
        }

        public void UpdatePlayers(in List<PlayerInfo> players)
        {
            Players = players;
        }
    }
}