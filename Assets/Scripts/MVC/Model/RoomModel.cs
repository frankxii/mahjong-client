using MVC.Base;

namespace MVC.Model
{
    public class RoomModel : BaseModel<RoomModel>
    {
        public short CurrentCycle;
        public short TotalCycle;
        public short RoomID;
    }
}