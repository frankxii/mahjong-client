using MVC.Base;
using MVC.View;
using UnityEngine;

namespace MVC.Controller
{
    public class ResultController : BaseController<ResultController, ResultView>
    {
        protected override string ViewName => "UI/ResultPanel";

        protected override void OnViewMounted()
        {
            view.btnContinue.onClick.AddListener(() => Debug.Log("continue"));
            view.btnClose.onClick.AddListener(() => Destroy());
        }
    }
}