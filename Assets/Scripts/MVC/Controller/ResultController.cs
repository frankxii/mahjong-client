using MVC.Base;
using MVC.View;

namespace MVC.Controller
{
    public class ResultController : BaseController<ResultController, ResultView>
    {
        protected override string ViewName => "UI/ResultPanel";

        protected override void OnViewMounted()
        {
        }
    }
}