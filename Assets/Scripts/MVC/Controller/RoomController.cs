using MVC.Base;
using MVC.Model;
using MVC.View;

namespace MVC.Controller
{
    public class RoomController : BaseController<RoomController, RoomView>
    {
        protected override string ViewName => "UI/RoomPanel";

        protected override void OnViewMounted()
        {
            view.UpdateRoomInfo(RoomModel.Instance);
            view.UpdateSelfInfo(UserModel.Instance);
        }
    }
}