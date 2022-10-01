using System;

namespace Data
{
    public class PlayerInfo : UserInfo
    {
        [Obsolete]
        public new int diamond;
        public bool isReady;
        public byte dealerWind; // 门风
    }
}