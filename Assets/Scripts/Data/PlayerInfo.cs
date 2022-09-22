using System;

namespace Data
{
    public class PlayerInfo : UserInfo
    {
        [Obsolete]
        public new short diamond;

        public byte dealerWind; // 门风
    }
}