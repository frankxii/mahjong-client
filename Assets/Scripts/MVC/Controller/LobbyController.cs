using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : BaseController<LobbyController, LobbyView>
{
    protected override string ViewName => "UI/LobbyPanel";

    protected override void OnViewMounted()
    {
        view.UpdateUserProfile(UserModel.Instance);
    }
}