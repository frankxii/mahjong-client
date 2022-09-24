using System;
using Data;
using MVC.Base;

namespace MVC.Model
{
    public class UserModel : BaseModel<UserModel>
    {
        public event Action<UserModel> OnDataUpdate;
        public string Username { get; private set; }
        public int UserId { get; private set; }
        public byte Gender { get; private set; }
        public int Coin { get; private set; }
        public int Diamond { get; private set; }

        public void UpdateData(UserInfo info)
        {
            Username = info.username;
            UserId = info.userId;
            Gender = info.gender;
            Coin = info.coin;
            Diamond = info.diamond;
            if (OnDataUpdate is not null)
                OnDataUpdate(this);
        }
    }
}