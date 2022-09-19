using MVC.Base;
using UnityEngine.UI;

namespace MVC.View
{
    public class RoomView : BaseView
    {
        public Text txtRoomId; // 房间ID
        public Text txtCycle; // 当前圈数
        public Image imgDicePanel; // 中控

        public Image imgSelfAvatar;
        public Text txtSelfUsername;
        public Text txtSelfCoinNumber;

        public Image imgOppositeAvatar;
        public Text txtOppositeUsername;
        public Text txtOppositeCoinNumber;

        public Image imgLeftAvatar;
        public Text txtLeftUsername;
        public Text txtLeftCoinNumber;

        public Image imgRightAvatar;
        public Text txtRightUsername;
        public Text txtRightCoinNumber;
    }
}