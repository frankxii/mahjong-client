using Data;
using MVC.Base;
using MVC.Model;
using MVC.View;
using Protocol;

namespace MVC.Controller
{
    public class CreateRoomController : BaseController<CreateRoomController, CreateRoomView>
    {
        protected override string ViewName => "UI/CreateRoomPanel";

        protected override void OnViewMounted()
        {
            view.btnGeneralModeCreate.onClick.AddListener(() => CreateRoom());
        }

        private void CreateRoom()
        {
            CreateRoomReq req = new() {userId = UserModel.Instance.UserId, totalCycle = view.GetGameCycleNumber()};
            NetworkManager.Instance.AddListener(MessageId.CreateRoom, OnCreateRoom);
            NetworkManager.Instance.Send(MessageId.CreateRoom, req);
        }


        private void OnCreateRoom(string json)
        {
            Response<RoomInfo> response = ProtoUtil.Deserialize<Response<RoomInfo>>(json);
            if (response.code == 0)
            {
                RoomModel.Instance.UpdateRoomInfo(response.data, UserModel.Instance);
                RoomController.Instance.ShowUI();
                LobbyController.Instance.Destroy();
                // 销毁UI和controller时需要注销回调事件，不然会导致引用错误
                NetworkManager.Instance.RemoveListener(MessageId.CreateRoom, OnCreateRoom);
                Destroy();
            }
        }
    }
}