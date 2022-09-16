using UnityEngine;

public class LoginController : BaseController<LoginController, LoginView>
{
    protected override string ViewName => "UI/LoginPanel";

    protected override void OnViewMounted(LoginView view)
    {
        view.btnLogin.onClick.AddListener(() => Debug.Log("you are best"));
    }
}