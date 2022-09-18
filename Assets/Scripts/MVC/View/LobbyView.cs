using MVC.Base;
using MVC.Model;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class LobbyView : BaseView
    {
        [Header("顶部")]
        public Image imgAvatar;
        public Sprite boyAvatar;
        public Sprite girlAvatar;
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
            if (model.Gender == 1)
                imgAvatar.sprite = boyAvatar;
            else
                imgAvatar.sprite = girlAvatar;
        }
    }
}