using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel : BaseModel<UserModel>
{
    public string Username { get; private set; }
    public short Id { get; private set; }
    public int Coin { get; private set; }
    public int Diamond { get; private set; }
}