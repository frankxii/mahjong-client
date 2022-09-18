using MVC.Base;
using MVC.View;

namespace MVC.Controller
{
    public class CreateRoomController : BaseController<CreateRoomController, CreateRoomView>
    {
        protected override string ViewName => "UI/CreateRoomPanel";

        protected override void OnViewMounted()
        {
        }
    }
}