using MVC.Base;
using MVC.Model;
using MVC.View;
using Protocol;
using UnityEngine;

namespace MVC.Controller
{
    public class LoginController : BaseController<LoginController, LoginView>
    {
        protected override string ViewName => "UI/LoginPanel";

        protected override void OnViewMounted()
        {
            view.btnLogin.onClick.AddListener(Login);
            NetworkManager.Instance.AddListener(MessageId.Login, OnLogin);
        }

        private void Login()
        {
            string username = view.inputUsername.text;
            string password = view.inputPassword.text;
            LoginReq req = new() {username = username, password = password};
            NetworkManager.Instance.Send(MessageId.Login, req);
        }

        private void OnLogin(Message message)
        {
            LoginAck ack = JsonUtility.FromJson<LoginAck>(message.jsonString);
            // 登录成功，更新用户数据，打开大厅，销毁登录页面，移除登录回调
            if (ack.errCode == 0)
            {
                UserModel.Instance.UpdateData(ack);
                LobbyController.Instance.ShowUI();
                NetworkManager.Instance.RemoveListener(MessageId.Login, OnLogin);
                Destroy();
            }
        }
    }
}