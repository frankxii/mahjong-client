using MVC.Base;
using MVC.View;

namespace MVC.Controller
{
    public class JoinRoomController : BaseController<JoinRoomController, JoinRoomView>
    {
        protected override string ViewName => "UI/JoinRoomPanel";

        protected override void OnViewMounted()
        {
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
            view.btnClear.onClick.AddListener(() => view.Clear());
        }
    }
}