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

            // 绑定离开房间事件
            view.btnLeaveRoom.onClick.AddListener(LeaveRoom);
        }

        // 房间玩家信息同步回调
        private void OnUpdatePlayer(string json)
        {
            List<PlayerInfo> players = ProtoUtil.Deserialize<List<PlayerInfo>>(json);
            RoomModel.Instance.UpdatePlayers(players);
            view.UpdatePlayerInfo(RoomModel.Instance.DealerWind, players);
        }

        // 发起离开房间请求
        private void LeaveRoom()
        {
            LeaveRoomReq req = new() {userId = UserModel.Instance.UserId, roomId = RoomModel.Instance.RoomId};
            NetworkManager.Instance.AddListener(MessageId.LeaveRoom, OnLeaveRoom);
            NetworkManager.Instance.Send(MessageId.LeaveRoom, req);
        }

        // 离开房间回调
        private void OnLeaveRoom(string json)
        {
            Response<object> resp = ProtoUtil.Deserialize<Response<object>>(json);
            if (resp.code == 0)
            {
                // 注销回调事件
                NetworkManager.Instance.RemoveListener(MessageId.UpdatePlayer, OnUpdatePlayer);
                NetworkManager.Instance.RemoveListener(MessageId.LeaveRoom, OnLeaveRoom);
                // 清空房间信息
                RoomModel.Instance = null;
                // 加载大厅
                LobbyController.Instance.ShowUI();
                // 关闭房间view
                Destroy();
            }
        }
    }
}