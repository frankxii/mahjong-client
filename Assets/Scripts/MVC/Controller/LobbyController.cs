using MVC.Base;
using MVC.Model;
using MVC.View;

namespace MVC.Controller
{
    public class LobbyController : BaseController<LobbyController, LobbyView>
    {
        protected override string ViewName => "UI/LobbyPanel";

        protected override void OnViewMounted()
        {
            view.UpdateUserProfile(UserModel.Instance);
            view.btnCreateRoom.onClick.AddListener(() => { CreateRoomController.Instance.ShowUI(); });
            view.btnJoinRoom.onClick.AddListener(() => JoinRoomController.Instance.ShowUI());
        }
    }
}