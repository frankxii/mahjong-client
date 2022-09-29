using System.Collections.Generic;
using Data;
using MVC.Base;
using MVC.Model;
using MVC.View;
using Protocol;

namespace MVC.Controller
{
    public class RoomController : BaseController<RoomController, RoomView>
    {
        protected override string ViewName => "UI/RoomPanel";

        protected override void OnViewMounted()
        {
            view.UpdateRoomInfo(RoomModel.Instance);
            view.UpdatePlayerInfo(RoomModel.Instance.DealerWind, RoomModel.Instance.Players);
            NetworkManager.Instance.AddListener(MessageId.UpdatePlayer, OnUpdatePlayer);
        }

        // 房间玩家信息同步回调
        private void OnUpdatePlayer(string json)
        {
            List<PlayerInfo> players = ProtoUtil.Deserialize<List<PlayerInfo>>(json);
            view.UpdatePlayerInfo(RoomModel.Instance.DealerWind, players);
        }
    }
}