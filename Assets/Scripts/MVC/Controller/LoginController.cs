using System;
using Protocol;
using UnityEngine;

public class LoginController : BaseController<LoginController, LoginView>
{
    protected override string ViewName => "UI/LoginPanel";

    protected override void OnViewMounted()
    {
        NetworkManager.Instance.AddListener(MessageId.Login, OnLogin);
        view.btnLogin.onClick.AddListener(Login);
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
        Debug.Log(message.jsonString);
    }
}