using System.Collections.Generic;
using System.Threading.Tasks;
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
            // 注册网络回调方法
            RegisterCallback();

            view.UpdateRoomInfo(RoomModel.Instance);
            view.UpdatePlayerInfo(RoomModel.Instance.DealerWind, RoomModel.Instance.Players);


            // 绑定离开房间事件
            view.btnLeaveRoom.onClick.AddListener(LeaveRoom);
            // 绑定准备事件
            view.btnReady.onClick.AddListener(Ready);
            // 绑定出牌事件
            view.onPlayCard += OnPlayCard;
            // 绑定操作事件
            view.onOperation += OnOperation;
        }

        private void RegisterCallback()
        {
            NetworkManager.Instance.AddListener(MessageId.UpdatePlayer, OnUpdatePlayer);
            NetworkManager.Instance.AddListener(MessageId.LeaveRoom, OnLeaveRoom);
            NetworkManager.Instance.AddListener(MessageId.Ready, OnReady);
            NetworkManager.Instance.AddListener(MessageId.DealCard, OnDealCard);
            NetworkManager.Instance.AddListener(MessageId.DrawCardEvent, OnDrawCard);
            NetworkManager.Instance.AddListener(MessageId.PlayCardEvent, OnPlayCardEvent);
            NetworkManager.Instance.AddListener(MessageId.PengGangEvent, OnPengGangEvent);
            NetworkManager.Instance.AddListener(MessageId.HuEvent, OnHuEvent);
        }

        private void RemoveCallback()
        {
            NetworkManager.Instance.RemoveListener(MessageId.UpdatePlayer, OnUpdatePlayer);
            NetworkManager.Instance.RemoveListener(MessageId.LeaveRoom, OnLeaveRoom);
            NetworkManager.Instance.RemoveListener(MessageId.Ready, OnReady);
            NetworkManager.Instance.RemoveListener(MessageId.DealCard, OnDealCard);
            NetworkManager.Instance.RemoveListener(MessageId.DrawCardEvent, OnDrawCard);
            NetworkManager.Instance.RemoveListener(MessageId.PlayCardEvent, OnPlayCardEvent);
            NetworkManager.Instance.RemoveListener(MessageId.PengGangEvent, OnPengGangEvent);
            NetworkManager.Instance.RemoveListener(MessageId.HuEvent, OnHuEvent);
        }

        public override void Destroy()
        {
            // 注销回调事件
            RemoveCallback();
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
            ReadyReq req = new() {userId = UserModel.Instance.UserId, roomId = RoomModel.Instance.RoomId};
            NetworkManager.Instance.Send(MessageId.Ready, req);
        }

        // 准备服务器回调
        private void OnReady(string json)
        {
            Response<object> resp = ProtoUtil.Deserialize<Response<object>>(json);
            if (resp.code == 0)
            {
                view.OnReady();
            }
        }

        // 发牌回调
        private async void OnDealCard(string json)
        {
            List<byte> handCards = ProtoUtil.Deserialize<List<byte>>(json);

            // 离开房间按钮关闭
            // view.btnLeaveRoom.gameObject.SetActive(false);
            // 关闭所有准备状态
            view.HideAllReadyFlag();
            // 更新对战局数
            // 播放色子动画
            // 更新本家手牌和对手手牌
            view.OnDealCard(handCards);
            await Task.Delay(1000);
            // 理牌
            view.SortCard(handCards);

            // 通知服务器已理牌完毕，可以开始摸牌
            NetworkManager.Instance.Send(MessageId.SortCardFinished, new SortCardReq()
            {
                userId = UserModel.Instance.UserId,
                roomId = RoomModel.Instance.RoomId
            });
            // 更新房间剩余牌数
            view.txtRemainCard.text = "56";
        }

        // 摸牌
        private void OnDrawCard(string json)
        {
            DrawCardEvent data = ProtoUtil.Deserialize<DrawCardEvent>(json);
            // 更新本家或其他玩家摸牌
            view.OnDrawCard(RoomModel.Instance.DealerWind, data);
        }

        // 本家出牌
        private void OnPlayCard(byte card)
        {
            PlayCardReq req = new()
            {
                roomId = RoomModel.Instance.RoomId,
                userId = UserModel.Instance.UserId,
                card = card
            };
            NetworkManager.Instance.Send(MessageId.PlayCard, req);
        }

        // 服务器通知有人出牌
        private void OnPlayCardEvent(string json)
        {
            view.OnPlayCardEvent(RoomModel.Instance.DealerWind, ProtoUtil.Deserialize<PlayCardEvent>(json));
        }

        private void OnOperation(string operation)
        {
            OperationCode code = operation switch
            {
                "peng" => OperationCode.Peng,
                "gang" => OperationCode.Gang,
                "hu" => OperationCode.Hu,
                _ => OperationCode.Pass
            };

            // 通知服务器，玩家执行了对应操作
            NetworkManager.Instance.Send(MessageId.Operation, new OperationReq()
            {
                operationCode = code,
                dealerWind = RoomModel.Instance.DealerWind,
                roomId = RoomModel.Instance.RoomId
            });
        }

        private void OnPengGangEvent(string json)
        {
            PengGangEvnet data = ProtoUtil.Deserialize<PengGangEvnet>(json);
            view.OnPengGangEvent(RoomModel.Instance.DealerWind, data);
        }

        private void OnHuEvent(string json)
        {
            List<PlayerHuResult> results = ProtoUtil.Deserialize<List<PlayerHuResult>>(json);
            view.OnHuEvent(RoomModel.Instance.DealerWind, results);
            ResultController.Instance.ShowUI();
            ResultController.Instance.UpdateResult(RoomModel.Instance.DealerWind, results);
        }
    }
}