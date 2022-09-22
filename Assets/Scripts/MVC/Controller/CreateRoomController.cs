using MVC.Base;
using MVC.Model;
using MVC.View;
using Newtonsoft.Json;
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
            CreateRoomReq req = new() {userId = UserModel.Instance.Id, totalCycle = 8};
            NetworkManager.Instance.AddListener(MessageId.CreateRoom, OnCreateRoom);
            NetworkManager.Instance.Send(MessageId.CreateRoom, req);
        }


        private void OnCreateRoom(Message message)
        {
            Response<CreateRoomAck> response;
            response = JsonConvert.DeserializeObject<Response<CreateRoomAck>>(message.jsonString);
            if (response?.code == 0)
            {
                RoomModel.Instance.UpdateRoomInfo(response.data);
                RoomController.Instance.ShowUI();
                LobbyController.Instance.Destroy();
                Destroy();
            }
        }
    }
}