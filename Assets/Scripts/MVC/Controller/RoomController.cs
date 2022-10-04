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
            // 绑定准备事件
            view.btnReady.onClick.AddListener(Ready);
        }

        public override void Destroy()
        {
            // 注销回调事件
            NetworkManager.Instance.RemoveListener(MessageId.UpdatePlayer, OnUpdatePlayer);
            NetworkManager.Instance.RemoveListener(MessageId.LeaveRoom, OnLeaveRoom);
            NetworkManager.Instance.RemoveListener(MessageId.Ready, OnReady);
            NetworkManager.Instance.RemoveListener(MessageId.Deal, OnDeal);
            // 清空房间信息
            RoomModel.Instance = null;
            base.Destroy();
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
                // 加载大厅
                LobbyController.Instance.ShowUI();
                Destroy();
            }
        }

        // 发起准备请求
        private void Ready()
        {
            NetworkManager.Instance.AddListener(MessageId.Ready, OnReady);
            NetworkManager.Instance.AddListener(MessageId.Deal, OnDeal);
            ReadyReq req = new() {userId = UserModel.Instance.UserId, roomId = RoomModel.Instance.RoomId};
            NetworkManager.Instance.Send(MessageId.Ready, req);
        }

        // 准备服务器回调
        private void OnReady(string json)
        {
            Response<object> resp = ProtoUtil.Deserialize<Response<object>>(json);
            if (resp.code == 0)
            {
                view.Ready();
            }
        }

        // 发牌回调
        private void OnDeal(string json)
        {
            List<byte> handCards = ProtoUtil.Deserialize<List<byte>>(json);

            // 离开房间按钮关闭
            // view.btnLeaveRoom.gameObject.SetActive(false);
            // 准备状态更新
            // 更新对战局数
            // 播放色子动画
            // 更新本家手牌和对手手牌
            view.DealCard(handCards);
            // 更新房间剩余牌数
        }
    }
}