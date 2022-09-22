using System.Collections.Generic;

namespace Data
{
    public class RoomInfo
    {
        public short roomId;
        public short currentCycle;
        public short totalCycle;
        public List<PlayerInfo> players;
    }
}