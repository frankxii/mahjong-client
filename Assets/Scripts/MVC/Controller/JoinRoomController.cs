using Data;
using MVC.Base;
using MVC.Model;
using MVC.View;
using Protocol;

namespace MVC.Controller
{
    public class JoinRoomController : BaseController<JoinRoomController, JoinRoomView>
    {
        protected override string ViewName => "UI/JoinRoomPanel";

        protected override void OnViewMounted()
        {
            // 注册数字键盘回调
            view.btnNumber0.onClick.AddListener(() => view.ClickNumber(0));
            view.btnNumber1.onClick.AddListener(() => view.ClickNumber(1));
            view.btnNumber2.onClick.AddListener(() => view.ClickNumber(2));
            view.btnNumber3.onClick.AddListener(() => view.ClickNumber(3));
            view.btnNumber4.onClick.AddListener(() => view.ClickNumber(4));
            view.btnNumber5.onClick.AddListener(() => view.ClickNumber(5));
            view.btnNumber6.onClick.AddListener(() => view.ClickNumber(6));
            view.btnNumber7.onClick.AddListener(() => view.ClickNumber(7));
            view.btnNumber8.onClick.AddListener(() => view.ClickNumber(8));
            view.btnNumber9.onClick.AddListener(() => view.ClickNumber(9));
            // 注册删除回调
            view.btnDelete.onClick.AddListener(() => view.Delete());
            // 注册清空回调
            view.btnClear.onClick.AddListener(() => view.Clear());
            // 注册关闭按钮回调
            view.btnClose.onClick.AddListener(() => Destroy());
            // 注册加入按钮回调
            view.btnJoinRoom.onClick.AddListener(JoinRoom);
        }

        private void JoinRoom()
        {
            string roomNo = view.GetInputFieldString();
            int roomId = int.Parse(roomNo);
            JoinRoomReq req = new() {userId = UserModel.Instance.UserId, roomId = roomId};
            NetworkManager.Instance.AddListener(MessageId.JoinRoom, OnJoinRoom);
            NetworkManager.Instance.Send(MessageId.JoinRoom, req);
        }

        private void OnJoinRoom(string json)
        {
            Response<RoomInfo> response = ProtoUtil.Deserialize<Response<RoomInfo>>(json);
            if (response.code == 0)
            {
                RoomModel.Instance.UpdateRoomInfo(response.data, UserModel.Instance);
                RoomController.Instance.ShowUI();
                LobbyController.Instance.Destroy();
                NetworkManager.Instance.RemoveListener(MessageId.JoinRoom, OnJoinRoom);
                Destroy();
            }
        }
    }
}