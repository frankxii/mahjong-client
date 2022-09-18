using System;
using MVC.Base;
using Protocol;

namespace MVC.Model
{
    public class UserModel : BaseModel<UserModel>
    {
        public event Action<UserModel> OnDataUpdate;
        public string Username { get; private set; }
        public short Id { get; private set; }
        public int Coin { get; private set; }
        public int Diamond { get; private set; }

        public void UpdateData(LoginAck ack)
        {
            Username = ack.username;
            Id = ack.id;
            Coin = ack.coin;
            Diamond = ack.diamond;
            if (OnDataUpdate is not null)
                OnDataUpdate(this);
        }
    }
}