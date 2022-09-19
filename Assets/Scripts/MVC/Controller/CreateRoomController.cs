using MVC.Base;
using MVC.View;

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
            RoomController.Instance.ShowUI();
            LobbyController.Instance.Destroy();
            Destroy();
        }
    }
}