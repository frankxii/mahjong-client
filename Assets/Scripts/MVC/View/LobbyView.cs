using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : BaseView
{
    [Header("顶部")]
    public Image imgAvatar;
    public Text txtCoinNumber;
    public Button btnAddCoin;
    public Text txtDiamondNumber;
    public Button btnAddDiamond;

    [Header("底部")]
    public Button btnTask;
    public Button btnSameCity;
    public Button btnMall;
    public Button btnEvent;
    public Button btnSetting;
    public Button btnDailyRewards;

    [Header("创建房间")]
    public Button btnCreateRoom;
    public Button btnJoinRoom;

    public void UpdateUserProfile(UserModel model)
    {
        txtCoinNumber.text = model.Coin.ToString();
        txtDiamondNumber.text = model.Diamond.ToString();
    }
}