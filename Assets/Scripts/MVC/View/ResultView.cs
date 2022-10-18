using System;
using System.Collections.Generic;
using MVC.Base;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class ResultView : BaseView
    {
        public Text txtWinType;

        [Header("本家")]
        public Image imgSelfAvatar;
        public Text txtSelfScore;
        public Text txtSelfWinType;
        [Header("对家")]
        public Image imgOppositeAvatar;
        public Text txtOppositeScore;
        public Text txtOppositeWinType;
        [Header("上家")]
        public Image imgLeftAvatar;
        public Text txtLeftScore;
        public Text txtLeftWinType;
        [Header("下家")]
        public Image imgRightAvatar;
        public Text txtRightScore;
        public Text txtRightWinType;

        public Button btnContinue;
        public Button btnClose;

        // 通过本家门风和玩家门风，来判断玩家位置是本家、对家、上家、下家
        private SeatPos DealerWindToSeatPos(byte selfDealerWind, byte playerDealerWind)
        {
            int value = playerDealerWind - selfDealerWind;

            if (value == 2 || value == -2)
                return SeatPos.Opposite;
            else if (value == 1 || value == -3)
                return SeatPos.Right;
            else if (value == -1 || value == 3)
                return SeatPos.Left;
            else
                return SeatPos.Self;
        }

        /// <summary>
        /// 更新结算结果
        /// </summary>
        /// <param name="dealerWind">本家门风</param>
        /// <param name="results">结算结果列表</param>
        public void UpdateResult(byte dealerWind, List<PlayerHuResult> results)
        {
            Dictionary<SeatPos, Text> scoreDict = new()
            {
                [SeatPos.Self] = txtSelfScore,
                [SeatPos.Opposite] = txtOppositeScore,
                [SeatPos.Left] = txtLeftScore,
                [SeatPos.Right] = txtRightScore
            };
            Dictionary<SeatPos, Text> winTypeDict = new()
            {
                [SeatPos.Self] = txtSelfWinType,
                [SeatPos.Opposite] = txtOppositeWinType,
                [SeatPos.Left] = txtLeftWinType,
                [SeatPos.Right] = txtRightWinType
            };
            foreach (PlayerHuResult result in results)
            {
                SeatPos seat = DealerWindToSeatPos(dealerWind, result.dealerWind);
                if (result.score != 0)
                {
                    string sign = result.score > 0 ? "+" : "-";
                    scoreDict[seat].text = sign + Math.Abs(result.score).ToString();
                }

                if (result.isHu)
                {
                    winTypeDict[seat].text = result.huType.ToString();
                    txtWinType.text = result.huType.ToString();
                }
            }
        }
    }
}