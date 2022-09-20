using MVC.Base;
using Protocol;

namespace MVC.Model
{
    public class RoomModel : BaseModel<RoomModel>
    {
        public short RoomID { get; private set; } // 房间ID
        public short CurrentCycle { get; private set; } // 当前圈数
        public short TotalCycle { get; private set; } // 总圈数
        public byte DealerWind { get; private set; } // 门风


        public void UpdateRoomInfo(CreateRoomAck ack)
        {
            RoomID = ack.roomId;
            CurrentCycle = ack.currentCycle;
            TotalCycle = ack.totalCycle;
            DealerWind = ack.dealerWind;
        }
    }
}