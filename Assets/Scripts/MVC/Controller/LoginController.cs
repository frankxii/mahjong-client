using System;
using Protocol;
using UnityEngine;

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
        if (ack.errCode == 0)
        {
            UserModel.Instance.UpdateData(ack);
            LobbyController.Instance.ShowUI();
        }
    }
}