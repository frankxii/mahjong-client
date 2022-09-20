using MVC.Base;
using MVC.Model;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class RoomView : BaseView
    {
        public Text txtRoomId; // 房间ID
        public Text txtCycle; // 当前圈数
        public Image imgDealerWind; // 中控
        public Sprite eastWind;
        public Sprite southWind;
        public Sprite westWind;
        public Sprite northWind;

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

        public void UpdateRoomInfo(RoomModel model)
        {
            txtRoomId.text = model.RoomID.ToString();
            txtCycle.text = model.CurrentCycle.ToString() + "/" + model.TotalCycle.ToString();
            // 设置门风
            imgDealerWind.sprite = model.DealerWind switch
            {
                1 => eastWind,
                2 => southWind,
                3 => westWind,
                _ => northWind
            };
        }

        public void UpdateSelfInfo(UserModel model)
        {
            txtSelfUsername.text = model.Username;
            txtSelfCoinNumber.text = model.Coin.ToString();
        }
    }
}