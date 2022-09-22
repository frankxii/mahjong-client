using Data;
using MVC.Base;

namespace MVC.Model
{
    public class RoomModel : BaseModel<RoomModel>
    {
        public short RoomID { get; private set; } // 房间ID
        public short CurrentCycle { get; private set; } // 当前圈数
        public short TotalCycle { get; private set; } // 总圈数
        public byte DealerWind { get; private set; } // 门风


        public void UpdateRoomInfo(RoomInfo info, UserModel user)
        {
            RoomID = info.roomId;
            CurrentCycle = info.currentCycle;
            TotalCycle = info.totalCycle;
            foreach (PlayerInfo player in info.players)
            {
                if (player.id == user.Id)
                {
                    DealerWind = player.dealerWind;
                    break;
                }
            }
        }
    }
}