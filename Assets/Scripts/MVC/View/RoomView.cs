using System.Collections.Generic;
using Data;
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

        /// <summary>
        /// 更新房间基础信息
        /// </summary>
        /// <param name="model"></param>
        public void UpdateRoomInfo(RoomModel model)
        {
            txtRoomId.text = model.RoomID.ToString();
            txtCycle.text = model.CurrentCycle.ToString() + "/" + model.TotalCycle.ToString();
        }

        /// <summary>
        /// 更新玩家座位与昵称、金币等信息
        /// </summary>
        /// <param name="dealerWind"></param>
        /// <param name="players"></param>
        public void UpdatePlayerInfo(byte dealerWind, List<PlayerInfo> players)
        {
            // 设置门风
            imgDealerWind.sprite = dealerWind switch
            {
                1 => eastWind,
                2 => southWind,
                3 => westWind,
                _ => northWind
            };
            foreach (PlayerInfo player in players)
            {
                // 东南西北门风的值为1234，通过玩家门风与本家的差值，来判断每个玩家所在的位置
                int value = player.dealerWind - dealerWind;
                if (value == 0)
                {
                    txtSelfUsername.text = player.username;
                    txtSelfCoinNumber.text = player.coin.ToString();
                }
                else if (value == 2 || value == -2)
                {
                    txtOppositeUsername.text = player.username;
                    txtOppositeCoinNumber.text = player.coin.ToString();
                }
                else if (value == 1 || value == -3)
                {
                    txtRightUsername.text = player.username;
                    txtRightCoinNumber.text = player.coin.ToString();
                }
                else if (value == -1 || value == 3)
                {
                    txtLeftUsername.text = player.username;
                    txtLeftCoinNumber.text = player.coin.ToString();
                }
            }
        }
    }
}